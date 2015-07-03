using System;
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

        [Fact]
        public void TestBrowserTestForm()
        {

            using (var form = new BrowserTestForm())
            {

                var waiter = new WaitForFormsEvent<WebBrowserDocumentCompletedEventArgs>();
                form.WebBrowser.DocumentCompleted += (sender, args) => { waiter.SetEvent(); };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        [Fact]
        public void TestInitialization()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, false);
                var waiter = new WaitForFormsEvent<WebBrowserDocumentCompletedEventArgs>();
                form.WebBrowser.DocumentCompleted += (sender, args) => {

                    // Initialize the bridge and check if it worked.
                    bridge.Initialize();
                    Assert.True(bridge.Initialized);

                    waiter.SetEvent(); 
                };

                form.Show();
                form.WebBrowser.Url = new Uri(String.Format("file:///{0}/TestPage.html", Directory.GetCurrentDirectory()));

                // Wait for the document to load.
                Assert.True(waiter.WaitForEvent(2000));
            }
        }

        [Fact]
        public void TestAutoInitialization()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent<WebBrowserDocumentCompletedEventArgs>();
                form.WebBrowser.DocumentCompleted += (sender, args) =>
                {

                    // Check that the bridge is initialized.
                    Assert.True(bridge.Initialized);

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
        [Fact]
        public void TestGlobalFunctionCall()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent<WebBrowserDocumentCompletedEventArgs>();
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
        /// Tests calling a member function.
        /// </summary>
        [Fact]
        public void TestMemberFunctionCall()
        {

            using (var form = new BrowserTestForm())
            {

                var bridge = new ScriptingBridge(form.WebBrowser, true);
                var waiter = new WaitForFormsEvent<WebBrowserDocumentCompletedEventArgs>();
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
    }
}
