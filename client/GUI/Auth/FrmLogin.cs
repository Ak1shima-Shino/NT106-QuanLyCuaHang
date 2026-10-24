using System;
using System.Windows.Forms;
using RetailClient.Services;

namespace RetailClient.GUI.Auth
{
    public partial class FrmLogin : Form
    {
        private readonly FirebaseAuthService _firebaseAuthService =
            new FirebaseAuthService();

        public FrmLogin()
        {
            InitializeComponent();
            Username.Text = "Email";
            Text = "Firebase Login";
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập email và mật khẩu.");
                return;
            }

            btnLogin.Enabled = false;

            try
            {
                await _firebaseAuthService.LoginAsync(
                    txtUsername.Text.Trim(),
                    txtPassword.Text
                );

                MessageBox.Show("Đăng nhập Firebase thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}