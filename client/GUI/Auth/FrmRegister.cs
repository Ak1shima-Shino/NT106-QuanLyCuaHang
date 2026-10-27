using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RetailClient.DTO;
using RetailClient.Services;

namespace RetailClient.GUI.Auth
{
    public partial class FrmRegister : Form
    {
        private readonly AuthService _authService =
            new AuthService();

        public FrmRegister()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(
            object sender,
            EventArgs e)
        {
            // Validate input
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirm = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirm))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ thông tin.",
                    "Thiếu thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Validate email format
            if (!Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show(
                    "Địa chỉ email không hợp lệ.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show(
                    "Mật khẩu phải có ít nhất 8 ký tự.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show(
                    "Xác nhận mật khẩu không trùng khớp.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnRegister.Enabled = false;

            try
            {
                var request = new RegisterRequest
                {
                    Username = username,
                    Email = email,
                    Password = password
                };

                var result = await _authService.RegisterAsync(request);

                MessageBox.Show(
                    "Đăng ký thành công! Vui lòng đăng nhập.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Đăng ký thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
            }
        }

        private void lnkLogin_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
