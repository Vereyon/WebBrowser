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
    public partial class Form1 : Form
    {

        public ScriptingBridge Bridge;

        public Form1()
        {
            InitializeComponent();

            Bridge = new ScriptingBridge(webBrowser, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            versionLabel.Text = string.Format("Version: {0}", webBrowser.Version);

            // Load the example page.
            webBrowser.Url = new Uri(String.Format("file:///{0}/ExamplePage.html", Directory.GetCurrentDirectory()));
            
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            var documentMode = webBrowser.Document.InvokeScript("getDocumentMode");
            documentModeLabel.Text = string.Format("Document mode: {0}", documentMode);

            //MessageBox.Show(WinInetCacheControl.UrlCacheGroups().Count().ToString() + "\r\n" + WinInetCacheControl.UrlCacheEntries().Count().ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {

            webBrowser.Url = new Uri(String.Format("file:///{0}/ExamplePage.html", Directory.GetCurrentDirectory()));
        }

        private void button2_Click(object sender, EventArgs e)
        {

            WinInetCacheControl.ClearCache();
            MessageBox.Show(WinInetCacheControl.UrlCacheGroups().Count().ToString() + "\r\n" + WinInetCacheControl.UrlCacheEntries().Count().ToString());
        }
    }
}
