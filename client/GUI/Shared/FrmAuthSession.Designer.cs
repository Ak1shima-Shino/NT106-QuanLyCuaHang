namespace RetailClient.GUI.Shared
{
    partial class FrmAuthSession
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
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblRoles = new System.Windows.Forms.Label();
            this.btnRefreshMe = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.eventLog1 = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(58, 40);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(70, 16);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(58, 109);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(41, 16);
            this.lblEmail.TabIndex = 1;
            this.lblEmail.Text = "Email";
            // 
            // lblRoles
            // 
            this.lblRoles.AutoSize = true;
            this.lblRoles.Location = new System.Drawing.Point(58, 189);
            this.lblRoles.Name = "lblRoles";
            this.lblRoles.Size = new System.Drawing.Size(43, 16);
            this.lblRoles.TabIndex = 2;
            this.lblRoles.Text = "Roles";
            // 
            // btnRefreshMe
            // 
            this.btnRefreshMe.Location = new System.Drawing.Point(483, 28);
            this.btnRefreshMe.Name = "btnRefreshMe";
            this.btnRefreshMe.Size = new System.Drawing.Size(57, 27);
            this.btnRefreshMe.TabIndex = 3;
            this.btnRefreshMe.Text = "Refresh";
            this.btnRefreshMe.UseVisualStyleBackColor = true;
            this.btnRefreshMe.Click += new System.EventHandler(this.btnRefreshMe_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(484, 175);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(56, 30);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // FrmAuthSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnRefreshMe);
            this.Controls.Add(this.lblRoles);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblUsername);
            this.Name = "FrmAuthSession";
            this.Text = "FrmAuthSession";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAuthSession_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblRoles;
        private System.Windows.Forms.Button btnRefreshMe;
        private System.Windows.Forms.Button btnLogout;
        private System.Diagnostics.EventLog eventLog1;
    }
}