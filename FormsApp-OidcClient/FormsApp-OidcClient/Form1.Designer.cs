namespace FormsApp_OidcClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtLogin = new System.Windows.Forms.Button();
            this.TbResponse = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtLogin
            // 
            this.BtLogin.Location = new System.Drawing.Point(4, 6);
            this.BtLogin.Name = "BtLogin";
            this.BtLogin.Size = new System.Drawing.Size(102, 28);
            this.BtLogin.TabIndex = 0;
            this.BtLogin.Text = "Login";
            this.BtLogin.UseVisualStyleBackColor = true;
            this.BtLogin.Click += new System.EventHandler(this.BtLogin_Click);
            // 
            // TbResponse
            // 
            this.TbResponse.Location = new System.Drawing.Point(4, 39);
            this.TbResponse.Multiline = true;
            this.TbResponse.Name = "TbResponse";
            this.TbResponse.Size = new System.Drawing.Size(792, 409);
            this.TbResponse.TabIndex = 1;
            this.TbResponse.Text = "Login response will be shown here";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TbResponse);
            this.Controls.Add(this.BtLogin);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button BtLogin;
        private TextBox TbResponse;
    }
}