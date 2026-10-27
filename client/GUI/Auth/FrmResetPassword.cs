using System;
using System.Windows.Forms;
using RetailClient.DTO;
using RetailClient.Services;

namespace RetailClient.GUI.Auth
{
    public partial class FrmResetPassword : Form
    {
        private readonly AuthService _authService =
            new AuthService();

        public FrmResetPassword()
        {
            InitializeComponent();
        }

        private async void btnResetPassword_Click(
            object sender,
            EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string resetKey = txtResetKey.Text.Trim();
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(resetKey) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ thông tin.",
                    "Thiếu thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (newPassword.Length < 8)
            {
                MessageBox.Show(
                    "Mật khẩu mới phải có ít nhất 8 ký tự.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show(
                    "Xác nhận mật khẩu không trùng khớp.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnResetPassword.Enabled = false;

            try
            {
                var request = new ResetPasswordRequest
                {
                    Username = username,
                    Key = resetKey,
                    NewPassword = newPassword
                };

                var result =
                    await _authService.ResetPasswordAsync(request);

                // Clear sensitive fields
                txtResetKey.Text = "";
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";

                MessageBox.Show(
                    result.Message ?? "Đã đặt lại mật khẩu thành công! Vui lòng đăng nhập lại.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Đặt lại mật khẩu thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                var retry = MessageBox.Show(
                    "Bạn có muốn xin mã mới không?",
                    "Mã không hợp lệ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (retry == DialogResult.Yes)
                {
                    using (var frm = new FrmForgotPassword())
                    {
                        this.Hide();
                        frm.ShowDialog();
                        this.Close();
                    }
                }
            }
            finally
            {
                btnResetPassword.Enabled = true;
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
