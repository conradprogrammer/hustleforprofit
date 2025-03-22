namespace WJ_HustleForProfit_003.Test
{
    partial class frmTwitterAPI
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

        #region Wisej Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusBar1 = new Wisej.Web.StatusBar();
            this.tabControl1 = new Wisej.Web.TabControl();
            this.tabPage1 = new Wisej.Web.TabPage();
            this.tableLayoutPanel1 = new Wisej.Web.TableLayoutPanel();
            this.textBox1 = new Wisej.Web.TextBox();
            this.txtAccessTokenKey = new Wisej.Web.TextBox();
            this.txtAccessToken = new Wisej.Web.TextBox();
            this.txtConsumerSecret = new Wisej.Web.TextBox();
            this.txtConsumerKey = new Wisej.Web.TextBox();
            this.btnSubmitToConradOnlineAPI = new Wisej.Web.Button();
            this.tabPage2 = new Wisej.Web.TabPage();
            this.loginButton = new Wisej.Web.Button();
            this.verifyButton = new Wisej.Web.Button();
            this.authCodeTextBox = new Wisej.Web.TextBox();
            this.postTweetButton = new Wisej.Web.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 603);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(743, 22);
            this.statusBar1.TabIndex = 1;
            this.statusBar1.Text = "statusBar1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = Wisej.Web.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.PageInsets = new Wisej.Web.Padding(0, 39, 0, 0);
            this.tabControl1.Size = new System.Drawing.Size(743, 603);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(0, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(743, 564);
            this.tabPage1.Text = "ConradOnline API";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new Wisej.Web.ColumnStyle(Wisej.Web.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtAccessTokenKey, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtAccessToken, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtConsumerSecret, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtConsumerKey, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSubmitToConradOnlineAPI, 0, 0);
            this.tableLayoutPanel1.Dock = Wisej.Web.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new Wisej.Web.RowStyle(Wisej.Web.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new Wisej.Web.RowStyle(Wisej.Web.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new Wisej.Web.RowStyle(Wisej.Web.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new Wisej.Web.RowStyle(Wisej.Web.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new Wisej.Web.RowStyle(Wisej.Web.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new Wisej.Web.RowStyle(Wisej.Web.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new Wisej.Web.RowStyle(Wisej.Web.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 562);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.AutoSize = false;
            this.textBox1.Dock = Wisej.Web.DockStyle.Fill;
            this.textBox1.LabelText = "Tweet:";
            this.textBox1.Location = new System.Drawing.Point(3, 318);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(364, 144);
            this.textBox1.TabIndex = 5;
            // 
            // txtAccessTokenKey
            // 
            this.txtAccessTokenKey.Dock = Wisej.Web.DockStyle.Fill;
            this.txtAccessTokenKey.LabelText = "Access Token Key:";
            this.txtAccessTokenKey.Location = new System.Drawing.Point(3, 248);
            this.txtAccessTokenKey.Name = "txtAccessTokenKey";
            this.txtAccessTokenKey.Size = new System.Drawing.Size(364, 64);
            this.txtAccessTokenKey.TabIndex = 4;
            // 
            // txtAccessToken
            // 
            this.txtAccessToken.Dock = Wisej.Web.DockStyle.Fill;
            this.txtAccessToken.LabelText = "Access Token:";
            this.txtAccessToken.Location = new System.Drawing.Point(3, 178);
            this.txtAccessToken.Name = "txtAccessToken";
            this.txtAccessToken.Size = new System.Drawing.Size(364, 64);
            this.txtAccessToken.TabIndex = 3;
            // 
            // txtConsumerSecret
            // 
            this.txtConsumerSecret.Dock = Wisej.Web.DockStyle.Fill;
            this.txtConsumerSecret.LabelText = "API Key Secret:";
            this.txtConsumerSecret.Location = new System.Drawing.Point(3, 108);
            this.txtConsumerSecret.Name = "txtConsumerSecret";
            this.txtConsumerSecret.Size = new System.Drawing.Size(364, 64);
            this.txtConsumerSecret.TabIndex = 2;
            // 
            // txtConsumerKey
            // 
            this.txtConsumerKey.Dock = Wisej.Web.DockStyle.Fill;
            this.txtConsumerKey.LabelText = "API Key:";
            this.txtConsumerKey.Location = new System.Drawing.Point(3, 38);
            this.txtConsumerKey.Name = "txtConsumerKey";
            this.txtConsumerKey.Size = new System.Drawing.Size(364, 64);
            this.txtConsumerKey.TabIndex = 1;
            // 
            // btnSubmitToConradOnlineAPI
            // 
            this.btnSubmitToConradOnlineAPI.Dock = Wisej.Web.DockStyle.Fill;
            this.btnSubmitToConradOnlineAPI.Location = new System.Drawing.Point(3, 3);
            this.btnSubmitToConradOnlineAPI.Name = "btnSubmitToConradOnlineAPI";
            this.btnSubmitToConradOnlineAPI.Size = new System.Drawing.Size(364, 29);
            this.btnSubmitToConradOnlineAPI.TabIndex = 0;
            this.btnSubmitToConradOnlineAPI.Text = "Submit to Twitter API";
            this.btnSubmitToConradOnlineAPI.Click += new System.EventHandler(this.btnSubmitToConradOnlineAPI_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.postTweetButton);
            this.tabPage2.Controls.Add(this.authCodeTextBox);
            this.tabPage2.Controls.Add(this.verifyButton);
            this.tabPage2.Controls.Add(this.loginButton);
            this.tabPage2.Location = new System.Drawing.Point(0, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(743, 564);
            this.tabPage2.Text = "OAuth 2.0";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(56, 56);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(100, 37);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // verifyButton
            // 
            this.verifyButton.Location = new System.Drawing.Point(56, 125);
            this.verifyButton.Name = "verifyButton";
            this.verifyButton.Size = new System.Drawing.Size(100, 37);
            this.verifyButton.TabIndex = 1;
            this.verifyButton.Text = "Verify";
            this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
            // 
            // authCodeTextBox
            // 
            this.authCodeTextBox.Location = new System.Drawing.Point(182, 133);
            this.authCodeTextBox.Name = "authCodeTextBox";
            this.authCodeTextBox.Size = new System.Drawing.Size(243, 30);
            this.authCodeTextBox.TabIndex = 2;
            // 
            // postTweetButton
            // 
            this.postTweetButton.Location = new System.Drawing.Point(56, 200);
            this.postTweetButton.Name = "postTweetButton";
            this.postTweetButton.Size = new System.Drawing.Size(100, 37);
            this.postTweetButton.TabIndex = 3;
            this.postTweetButton.Text = "Post Tweet";
            this.postTweetButton.Click += new System.EventHandler(this.postTweetButton_Click);
            // 
            // frmTwitterAPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 625);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusBar1);
            this.Name = "frmTwitterAPI";
            this.Text = "frmTwitterAPI";
            this.Load += new System.EventHandler(this.frmTwitterAPI_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Wisej.Web.StatusBar statusBar1;
        private Wisej.Web.TabControl tabControl1;
        private Wisej.Web.TabPage tabPage1;
        private Wisej.Web.TabPage tabPage2;
        private Wisej.Web.TableLayoutPanel tableLayoutPanel1;
        private Wisej.Web.TextBox txtAccessTokenKey;
        private Wisej.Web.TextBox txtAccessToken;
        private Wisej.Web.TextBox txtConsumerSecret;
        private Wisej.Web.TextBox txtConsumerKey;
        private Wisej.Web.Button btnSubmitToConradOnlineAPI;
        private Wisej.Web.TextBox textBox1;
        private Wisej.Web.Button loginButton;
        private Wisej.Web.Button verifyButton;
        private Wisej.Web.TextBox authCodeTextBox;
        private Wisej.Web.Button postTweetButton;
    }
}