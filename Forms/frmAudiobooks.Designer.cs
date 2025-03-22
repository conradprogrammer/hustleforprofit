namespace WJ_HustleForProfit_003.Forms
{
    partial class frmAudiobooks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAudiobooks));
            this.txtInput = new Wisej.Web.TextBox();
            this.buttonSynthesize = new Wisej.Web.Button();
            this.widget1 = new Wisej.Web.Widget();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Dock = Wisej.Web.DockStyle.Left;
            this.txtInput.Location = new System.Drawing.Point(0, 0);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(378, 571);
            this.txtInput.TabIndex = 0;
            // 
            // buttonSynthesize
            // 
            this.buttonSynthesize.Location = new System.Drawing.Point(787, 24);
            this.buttonSynthesize.Name = "buttonSynthesize";
            this.buttonSynthesize.Size = new System.Drawing.Size(100, 37);
            this.buttonSynthesize.TabIndex = 2;
            this.buttonSynthesize.Text = "Synthesize";
            this.buttonSynthesize.Click += new System.EventHandler(this.buttonSynthesize_Click);
            // 
            // widget1
            // 
            this.widget1.Dock = Wisej.Web.DockStyle.Bottom;
            this.widget1.InitScript = resources.GetString("widget1.InitScript");
            this.widget1.Location = new System.Drawing.Point(378, 259);
            this.widget1.Name = "widget1";
            this.widget1.Options = ((Wisej.Core.DynamicObject)(Wisej.Core.WisejSerializer.Parse("{}")));
            this.widget1.Size = new System.Drawing.Size(512, 312);
            this.widget1.TabIndex = 5;
            this.widget1.Text = "widget1";
            // 
            // frmAudiobook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 571);
            this.Controls.Add(this.widget1);
            this.Controls.Add(this.buttonSynthesize);
            this.Controls.Add(this.txtInput);
            this.Name = "frmAudiobook";
            this.Text = "frmAudiobook";
            this.Load += new System.EventHandler(this.frmAudiobook_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wisej.Web.TextBox txtInput;
        private Wisej.Web.Button buttonSynthesize;
        private Wisej.Web.Widget widget1;
    }
}