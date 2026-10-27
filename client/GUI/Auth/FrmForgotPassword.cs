using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RetailClient.DTO;
using RetailClient.Services;

namespace RetailClient.GUI.Auth
{
    public partial class FrmForgotPassword : Form
    {
        private readonly AuthService _authService =
            new AuthService();

        public FrmForgotPassword()
        {
            InitializeComponent();
        }

        private async void btnSendReset_Click(
            object sender,
            EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(
                    "Vui lòng nhập địa chỉ email.",
                    "Thiếu thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

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

            btnSendReset.Enabled = false;

            try
            {
                var request = new ForgotPasswordRequest
                {
                    Email = email
                };

                var result =
                    await _authService.ForgotPasswordAsync(request);

                MessageBox.Show(
                    result.Message ?? "Nếu email tồn tại, hướng dẫn khôi phục sẽ được gửi.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSendReset.Enabled = true;
            }
        }

        private void lnkLogin_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void lnkHaveCode_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new FrmResetPassword())
            {
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
        }
    }
}
