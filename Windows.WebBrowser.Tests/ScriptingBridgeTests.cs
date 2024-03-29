﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xunit;
using Windows.WebBrowser;

namespace Vereyon.Windows
{
    public class ScriptingBridgeTests
    {

        [StaFact]
        public void TestBrowserTestForm()
        {

            using (var form = new BrowserTestForm())
            {

                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) => { waiter.SetEvent(); };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        [StaFact]
        public void TestInitialization()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, false);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) => {

                    // Initialize the bridge and check if it worked.
                    bridge.Initialize();
                    Assert.True(bridge.IsInitialized);

                    waiter.SetEvent(); 
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        [StaFact]
        public void TestAutoInitialization()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    // Check that the bridge is initialized.
                    Assert.True(bridge.IsInitialized);

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        /// <summary>
        /// Tests calling a global function.
        /// </summary>
        [StaFact]
        public void TestGlobalFunctionCall()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    var result = bridge.InvokeFunction<string>("globalFunction");
                    Assert.Equal("globalFunction", result);

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        /// <summary>
        /// Tests calling a function returning void.
        /// </summary>
        [StaFact]
        public void TestVoidFunctionCall()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    var result = bridge.InvokeFunction<string>("voidReturnFunction");
                    Assert.Null(result);

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        /// <summary>
        /// Tests calling an undefined function.
        /// </summary>
        [StaFact]
        public void TestUndefinedFunctionCall()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    Assert.Throws<Exception>(() => bridge.InvokeFunction<string>("undefinedFunction"));

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        /// <summary>
        /// Tests calling an undefined context.
        /// </summary>
        [StaFact]
        public void TestUndefinedContextCall()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    Assert.Throws<Exception>(() => bridge.InvokeFunction<string>("undefinedContext.someFunc"));
                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        /// <summary>
        /// Tests calling a member function.
        /// </summary>
        [StaFact]
        public void TestMemberFunctionCall()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    var result = bridge.InvokeFunction<string>("anObject.memberFunction");
                    Assert.Equal("memberFunction", result);

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        /// <summary>
        /// Tests calling a member function with a simple parameter.
        /// </summary>
        [StaFact]
        public void TestSimpleFunctionArgument()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    var result = bridge.InvokeFunction<int>("anObject.simpleArgumentFunction", 5);
                    Assert.Equal(5, result);

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        /// <summary>
        /// Tests calling a member function with a array parameter.
        /// </summary>
        [StaFact]
        public void TestSimpleFunctionArrayArgument()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    var parameter = new int[] { 1, 2, 3, 4, 5 };
                    var result = bridge.InvokeFunction<int[]>("anObject.simpleArgumentFunction", parameter);
                    Assert.Equal(parameter, result);

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        private class ParameterObject
        {
            public string TestString;
            public DateTime TestDate;

            public ParameterObject()
            {
                TestString = "123bvSDGdh^\\//.ë";
                TestDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Tests calling a member function with an object parameter.
        /// </summary>
        [StaFact]
        public void TestSimpleFunctionObjectArgument()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    var parameter = new ParameterObject();
                    var result = bridge.InvokeFunction<ParameterObject>("anObject.simpleArgumentFunction", parameter);
                    Assert.Equal(parameter.TestDate, result.TestDate);
                    Assert.Equal(parameter.TestString, result.TestString);

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        /// <summary>
        /// Tests passing a large array of objects to and from a javascript function.
        /// </summary>
        [StaFact]
        public void TestSimpleLargeObjectArrayArgument()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    var parameter = new List<ParameterObject>();
                    for (int i = 0; i < 1000; i++)
                        parameter.Add(new ParameterObject());

                    var result = bridge.InvokeFunction<List<ParameterObject>>("anObject.simpleArgumentFunction", parameter);
                    Assert.Equal(parameter.Count, result.Count);

                    waiter.SetEvent();
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }
    }
}
