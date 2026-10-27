using System;
using System.Windows.Forms;
using RetailClient.Services;

namespace RetailClient.GUI.Auth
{
    public partial class FrmLogin : Form
    {
        private readonly AuthService _authService =
            new AuthService();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private async void btnLogin_Click(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên đăng nhập và mật khẩu."
                );
                return;
            }

            btnLogin.Enabled = false;

            try
            {
                var result = await _authService.LoginAsync(
                    txtUsername.Text.Trim(),
                    txtPassword.Text
                );

                MessageBox.Show(
                    "Đăng nhập thành công!\nUser ID: " +
                    result.UserId
                );
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
