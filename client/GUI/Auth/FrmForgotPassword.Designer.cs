namespace RetailClient.GUI.Auth
{
    partial class FrmForgotPassword
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnSendReset = new System.Windows.Forms.Button();
            this.lnkLogin = new System.Windows.Forms.LinkLabel();
            this.lnkHaveCode = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(300, 40);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quên Mật Khẩu";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEmail
            // 
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblEmail.Location = new System.Drawing.Point(280, 130);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(130, 30);
            this.lblEmail.TabIndex = 1;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtEmail.Location = new System.Drawing.Point(430, 127);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(280, 30);
            this.txtEmail.TabIndex = 2;
            // 
            // btnSendReset
            // 
            this.btnSendReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.btnSendReset.Location = new System.Drawing.Point(380, 200);
            this.btnSendReset.Name = "btnSendReset";
            this.btnSendReset.Size = new System.Drawing.Size(240, 45);
            this.btnSendReset.TabIndex = 3;
            this.btnSendReset.Text = "Gửi Yêu Cầu";
            this.btnSendReset.UseVisualStyleBackColor = true;
            this.btnSendReset.Click += new System.EventHandler(this.btnSendReset_Click);
            // 
            // lnkLogin
            // 
            this.lnkLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lnkLogin.Location = new System.Drawing.Point(340, 280);
            this.lnkLogin.Name = "lnkLogin";
            this.lnkLogin.Size = new System.Drawing.Size(150, 25);
            this.lnkLogin.TabIndex = 4;
            this.lnkLogin.TabStop = true;
            this.lnkLogin.Text = "Quay về Đăng nhập";
            this.lnkLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLogin_LinkClicked);
            // 
            // lnkHaveCode
            // 
            this.lnkHaveCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lnkHaveCode.Location = new System.Drawing.Point(500, 280);
            this.lnkHaveCode.Name = "lnkHaveCode";
            this.lnkHaveCode.Size = new System.Drawing.Size(170, 25);
            this.lnkHaveCode.TabIndex = 5;
            this.lnkHaveCode.TabStop = true;
            this.lnkHaveCode.Text = "Tôi đã có mã";
            this.lnkHaveCode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHaveCode_LinkClicked);
            // 
            // FrmForgotPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 400);
            this.Controls.Add(this.lnkHaveCode);
            this.Controls.Add(this.lnkLogin);
            this.Controls.Add(this.btnSendReset);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblTitle);
            this.Name = "FrmForgotPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quên Mật Khẩu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnSendReset;
        private System.Windows.Forms.LinkLabel lnkLogin;
        private System.Windows.Forms.LinkLabel lnkHaveCode;
    }
}
