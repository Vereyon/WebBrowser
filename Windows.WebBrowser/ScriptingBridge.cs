using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Vereyon.Windows
{

    /// <summary>
    /// The ScriptingBridge class enhances scripting and communication between the browser JavaScript code and the managed host application.
    /// </summary>
    /// <remarks>
    /// Unfortunately it's not possible to call an object member function using HtmlDocument.InvokeScript. Only functions 
    /// in the global scope can be invoked. It's thus up to the client side of the browser bridge to define a function
    /// in the global scrope and to register it with the host side BrowserBridge. The host side BrowserBridge will from
    /// that point on always invoke the registered function when code is to be invoked.
    /// </remarks>
    [ComVisible(true)]
    public class ScriptingBridge
    {

        private const string JavaScriptDefaultFuncName = "scriptingBridge";

        /// <summary>
        /// Gets / sets the settings used for internal json serialization.
        /// </summary>
        protected JsonSerializerSettings InternalJsonSettings { get; set; }

        /// <summary>
        /// Gets / sets the settings used for parameter and return value serialization / deserialization.
        /// </summary>
        [ComVisible(false)]
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        /// <summary>
        /// Gets the WebBrowser control this scripting bridge is bound to.
        /// </summary>
        [ComVisible(false)]
        public WebBrowser WebBrowser { get; private set; }

        /// <summary>
        /// Gets / sets the name of the client side JavaScript function acting as the call gate.
        /// </summary>
        [ComVisible(false)]
        public string ClientCallGate { get; set; }

        /// <summary>
        /// Reports back a version string of the application for use on the client side.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets if the browser bridge is initialized.
        /// </summary>
        [ComVisible(false)]
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets / sets wether script dependencies should be injected on initialization.
        /// </summary>
        public bool InjectScriptDependencies { get; set; }

        /// <summary>
        /// Gets the WebBrowser document mode (also known as the IE version).
        /// </summary>
        [ComVisible(false)]
        public string DocumentMode { get; private set; }

        /// <summary>
        /// Indicates if JSON is supported by the client (either natively or via a polyfill).
        /// </summary>
        [ComVisible(false)]
        public bool JsonSupported { get; private set; }

        /// <summary>
        /// Constructs a new instanc of the WebBrowserBridge binding to the passed WebBrowser control.
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="autoInitialize">Controls if the scripting bridge is to be initialzed automatically.</param>
        public ScriptingBridge(WebBrowser webBrowser, bool autoInitialize)
        {

            InternalJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            WebBrowser = webBrowser;
            WebBrowser.ObjectForScripting = this;

            if(autoInitialize)
                webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(AutoInitializeHandler);
        }

        private void AutoInitializeHandler(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            // Attempt to automatically initialize the scripting bridge.
            Initialize();
        }

        /// <summary>
        /// Returns the scripting bridge souce code. Override to provide a custom implementation.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetScriptingBridgeSource()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Vereyon.Windows.Content.scripting-bridge.js";


            using(var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using(var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Return the JSON polyfill source code. Override to provide a custom implemenation.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetJsonSource()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Vereyon.Windows.Content.json.js";


            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        [ComVisible(false)]
        public void Initialize()
        {

            // Ensure that the JSON object is available.
            var jsonSource = GetJsonSource();
            WebBrowser.Document.InvokeScript("eval", new object[] { jsonSource });

            // Make the scripting bridge available on the client side.
            var bridgeSource = GetScriptingBridgeSource();
            WebBrowser.Document.InvokeScript("eval", new object[] { bridgeSource });

            // Attempt to invoke the scripting bridge on the client side. This should result in a call back to RegisterClient().
            WebBrowser.Document.InvokeScript(JavaScriptDefaultFuncName);
        }

        /// <summary>
        /// Helper function which checks if the bridge is initialized.
        /// </summary>
        private void RequireInitialized()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("The browser bridge is not initialized.");
        }

        /// <summary>
        /// Invokes the specified function in the hosted browser environment returning the specified return type.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="functionName">Name of function to call. In the format of object.property.memberFunction(). The call context will by default equal object.property part.</param>
        /// <param name="args">Variable list of arguments to pass to the function.</param>
        /// <returns></returns>
        [ComVisible(false)]
        public T InvokeFunction<T>(string functionName, params object[] args)
        {

            // Invoke the function.
            var result = InvokeFunctionInternal(functionName, args);
            if (string.IsNullOrEmpty(result.Result))
                return default(T);

            // Deserialize the function call result using the user provided serialization settings.
            var clientResult = JsonConvert.DeserializeObject<T>(result.Result, JsonSerializerSettings);
            return clientResult;
        }

        /// <summary>
        /// Invokes the specified function in the hosted browser environment without returning any data.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="args"></param>
        public void InvokeFunction(string functionName, params object[] args)
        {

            // Invoke the function.
            var result = InvokeFunctionInternal(functionName, args);
        }

        /// <summary>
        /// Internal implementation of code invoking the client side function while remaining agnostic about the returned
        /// data.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected InvokeFunctionResult InvokeFunctionInternal(string functionName, params object[] args)
        {

            // Serialize function parameters as JSON bacause we aren't able to pass objects
            // to the browser using InvokeScript.
            var json = JsonConvert.SerializeObject(args, JsonSerializerSettings);

            // Build parameter array.
            var parameters = new List<object>();
            parameters.Add(functionName);
            parameters.Add(json);

            // Invoke the browser side call gate.
            RequireInitialized();
            var resultJson = WebBrowser.Document.InvokeScript(ClientCallGate, parameters.ToArray());

            // Do basic inspection of the result and deserialize it.
            if (resultJson == null)
                throw new Exception("Did not receive any response from client side function invocation. Check if scripting is setup correctly.");
            if (!(resultJson is string))
                throw new Exception("Received unexpected response from client side.");
            var result = JsonConvert.DeserializeObject<InvokeFunctionResult>((string)resultJson, InternalJsonSettings);

            // Inspect the result.
            if (!result.Success)
                throw new Exception(result.Error);

            return result;
        }

        /// <summary>
        /// Registers the specified method name as the method to invoke at the browser client side when calling code.
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        [ComVisible(true)]
        public bool RegisterClient(string args)
        {

            // Deserialize the configuration.
            var configuration = JsonConvert.DeserializeObject<ScriptingBridgeRegistrationConfiguration>(args, InternalJsonSettings);

            ClientCallGate = configuration.CallGateName;
            DocumentMode = configuration.DocumentMode;
            JsonSupported = configuration.JsonSupported;

            // Set the initialized flag.
            IsInitialized = true;

            // Invoke the initialized event.
            if (Initialized != null)
                Initialized.Invoke(this, EventArgs.Empty);

            return true;
        }

        private class ScriptingBridgeRegistrationConfiguration
        {

            public string CallGateName { get; set; }
            public string DocumentMode { get; set; }
            public bool JsonSupported { get; set; }
        }

        protected class InvokeFunctionResult
        {
            public bool Success { get; set; }
            public string Error { get; set; }

            /// <summary>
            /// JSON string of function call result.
            /// </summary>
            public string Result { get; set; }
        }

        /// <summary>
        /// Invoked when the scripting bridge has been initialized.
        /// </summary>
        public event EventHandler Initialized;
    }
}
