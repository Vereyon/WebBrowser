using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vereyon.Windows
{
    public partial class ExampleForm : Form
    {

        public ScriptingBridge Bridge { get; private set; }

        public ExampleForm()
        {
            InitializeComponent();

            Bridge = new ScriptingBridge(webBrowser, true);
            Bridge.Initialized += new EventHandler(Bridge_Initialized);
        }

        void Bridge_Initialized(object sender, EventArgs e)
        {
            documentModeLabel.Text = string.Format("Document mode: {0}", Bridge.DocumentMode);
            jsonSupportLabel.Text = string.Format("JSON supported: {0}", Bridge.JsonSupported);
            bridgeStatusLabel.Text = string.Format("Scripting bridge initialized: {0}", Bridge.IsInitialized);
        }

        private void ExampleForm_Load(object sender, EventArgs e)
        {

            versionLabel.Text = string.Format("Version: {0}", webBrowser.Version);

            // Load the example page.
            webBrowser.Url = new Uri(String.Format("file:///{0}/ExamplePage.html", Directory.GetCurrentDirectory()));
        }

        private void reloadPageButton_Click(object sender, EventArgs e)
        {

            webBrowser.Url = new Uri(String.Format("file:///{0}/ExamplePage.html", Directory.GetCurrentDirectory()));
        }

        private void clearCacheButton_Click(object sender, EventArgs e)
        {

            WinInetCacheControl.ClearCache();
            MessageBox.Show("Cleared WinInet cache.");
        }

        private void scriptingButton_Click(object sender, EventArgs e)
        {

            var parameter = new ScriptingParameterData
            {
                Message = "Test message"
            };

            var data = Bridge.InvokeFunction<ScriptingReturnData>("myObject.myFunction", parameter);
        }
    }

    public class ScriptingParameterData
    {
        public string Message { get; set; }
    }

    public class ScriptingReturnData
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
    }
}
