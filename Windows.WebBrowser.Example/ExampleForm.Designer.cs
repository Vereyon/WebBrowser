namespace Vereyon.Windows
{
    partial class ExampleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.jsonSupportLabel = new System.Windows.Forms.Label();
            this.bridgeStatusLabel = new System.Windows.Forms.Label();
            this.documentModeLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.reloadPageButton = new System.Windows.Forms.Button();
            this.clearCacheButton = new System.Windows.Forms.Button();
            this.scriptingButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(12, 12);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(557, 467);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Url = new System.Uri("http://ExamplePage.html", System.UriKind.Absolute);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.jsonSupportLabel);
            this.groupBox1.Controls.Add(this.bridgeStatusLabel);
            this.groupBox1.Controls.Add(this.documentModeLabel);
            this.groupBox1.Controls.Add(this.versionLabel);
            this.groupBox1.Location = new System.Drawing.Point(575, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 118);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // jsonSupportLabel
            // 
            this.jsonSupportLabel.AutoSize = true;
            this.jsonSupportLabel.Location = new System.Drawing.Point(6, 42);
            this.jsonSupportLabel.Name = "jsonSupportLabel";
            this.jsonSupportLabel.Size = new System.Drawing.Size(88, 13);
            this.jsonSupportLabel.TabIndex = 3;
            this.jsonSupportLabel.Text = "JSON supported:";
            // 
            // bridgeStatusLabel
            // 
            this.bridgeStatusLabel.AutoSize = true;
            this.bridgeStatusLabel.Location = new System.Drawing.Point(6, 55);
            this.bridgeStatusLabel.Name = "bridgeStatusLabel";
            this.bridgeStatusLabel.Size = new System.Drawing.Size(83, 13);
            this.bridgeStatusLabel.TabIndex = 2;
            this.bridgeStatusLabel.Text = "Scripting bridge:";
            // 
            // documentModeLabel
            // 
            this.documentModeLabel.AutoSize = true;
            this.documentModeLabel.Location = new System.Drawing.Point(6, 29);
            this.documentModeLabel.Name = "documentModeLabel";
            this.documentModeLabel.Size = new System.Drawing.Size(88, 13);
            this.documentModeLabel.TabIndex = 1;
            this.documentModeLabel.Text = "Document mode:";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(6, 16);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(48, 13);
            this.versionLabel.TabIndex = 0;
            this.versionLabel.Text = "Version: ";
            // 
            // reloadPageButton
            // 
            this.reloadPageButton.Location = new System.Drawing.Point(6, 19);
            this.reloadPageButton.Name = "reloadPageButton";
            this.reloadPageButton.Size = new System.Drawing.Size(211, 23);
            this.reloadPageButton.TabIndex = 2;
            this.reloadPageButton.Text = "Reload page";
            this.reloadPageButton.UseVisualStyleBackColor = true;
            this.reloadPageButton.Click += new System.EventHandler(this.reloadPageButton_Click);
            // 
            // clearCacheButton
            // 
            this.clearCacheButton.Location = new System.Drawing.Point(6, 48);
            this.clearCacheButton.Name = "clearCacheButton";
            this.clearCacheButton.Size = new System.Drawing.Size(211, 23);
            this.clearCacheButton.TabIndex = 3;
            this.clearCacheButton.Text = "Clean cache";
            this.clearCacheButton.UseVisualStyleBackColor = true;
            this.clearCacheButton.Click += new System.EventHandler(this.clearCacheButton_Click);
            // 
            // scriptingButton
            // 
            this.scriptingButton.Location = new System.Drawing.Point(6, 77);
            this.scriptingButton.Name = "scriptingButton";
            this.scriptingButton.Size = new System.Drawing.Size(211, 23);
            this.scriptingButton.TabIndex = 4;
            this.scriptingButton.Text = "Scripting Bridge";
            this.scriptingButton.UseVisualStyleBackColor = true;
            this.scriptingButton.Click += new System.EventHandler(this.scriptingButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.reloadPageButton);
            this.groupBox2.Controls.Add(this.scriptingButton);
            this.groupBox2.Controls.Add(this.clearCacheButton);
            this.groupBox2.Location = new System.Drawing.Point(575, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 112);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Control";
            // 
            // ExampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 491);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.webBrowser);
            this.Name = "ExampleForm";
            this.Text = "Example";
            this.Load += new System.EventHandler(this.ExampleForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label documentModeLabel;
        private System.Windows.Forms.Button reloadPageButton;
        private System.Windows.Forms.Button clearCacheButton;
        private System.Windows.Forms.Button scriptingButton;
        private System.Windows.Forms.Label bridgeStatusLabel;
        private System.Windows.Forms.Label jsonSupportLabel;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

