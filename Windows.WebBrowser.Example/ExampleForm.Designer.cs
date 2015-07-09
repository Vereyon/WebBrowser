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
            this.documentModeLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.scriptingButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.bridgeStatusLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(12, 12);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(557, 467);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Url = new System.Uri("http://ExamplePage.html", System.UriKind.Absolute);
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // groupBox1
            // 
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(602, 231);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(602, 260);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // scriptingButton
            // 
            this.scriptingButton.Location = new System.Drawing.Point(602, 289);
            this.scriptingButton.Name = "scriptingButton";
            this.scriptingButton.Size = new System.Drawing.Size(75, 23);
            this.scriptingButton.TabIndex = 4;
            this.scriptingButton.Text = "Scripting Bridge";
            this.scriptingButton.UseVisualStyleBackColor = true;
            this.scriptingButton.Click += new System.EventHandler(this.scriptingButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(602, 318);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // bridgeStatusLabel
            // 
            this.bridgeStatusLabel.AutoSize = true;
            this.bridgeStatusLabel.Location = new System.Drawing.Point(6, 42);
            this.bridgeStatusLabel.Name = "bridgeStatusLabel";
            this.bridgeStatusLabel.Size = new System.Drawing.Size(83, 13);
            this.bridgeStatusLabel.TabIndex = 2;
            this.bridgeStatusLabel.Text = "Scripting bridge:";
            // 
            // ExampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 491);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.scriptingButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.webBrowser);
            this.Name = "ExampleForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label documentModeLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button scriptingButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label bridgeStatusLabel;
    }
}

