using System;
using System.Windows.Forms;
using RetailClient.Services;
using RetailClient.Helpers;
using RetailClient.GUI.Shared;

namespace RetailClient.GUI.Auth
{
    public partial class FrmLogin : Form
    {
        private readonly AuthService _authService =
            new AuthService();
        private bool _loginInProgress;
        private bool _sessionOpened;

        private bool CanNavigate => !_loginInProgress && !_sessionOpened &&
            string.IsNullOrEmpty(SessionManager.AccessToken) && !IsDisposed && !Disposing;

        private void UpdateAuthButtons()
        {
            btnLogin.Enabled = CanNavigate;
            lnkRegister.Enabled = CanNavigate;
            lnkForgotPassword.Enabled = CanNavigate;
        }

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (!CanNavigate) return;

            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên đăng nhập và mật khẩu."
                );
                return;
            }

            _loginInProgress = true;
            UpdateAuthButtons();

            try
            {
                var result = await _authService.LoginAsync(
                    txtUsername.Text.Trim(),
                    txtPassword.Text
                );

                if (IsDisposed || Disposing) return;
                var me = await _authService.GetMeAsync(result.AccessToken);
                if (IsDisposed || Disposing) return;

                SessionManager.SaveSession(
                    result.AccessToken,
                    me.User,
                    result.ExpiresIn
                );

                txtPassword.Text = "";

                FrmAuthSession frm = new FrmAuthSession();
                frm.Show();
                _sessionOpened = true;

                this.Hide();
            }

            catch (Exception ex)
            {
                SessionManager.Clear();
                if (IsDisposed || Disposing) return;
                MessageBox.Show(
                    ex.Message,
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                _loginInProgress = false;
                if (!IsDisposed && !Disposing) UpdateAuthButtons();
            }
        }

        private void lnkRegister_Click(object sender, EventArgs e)
        {
            if (!CanNavigate) return;
            using (FrmRegister frm = new FrmRegister())
            {
                ShowAuthDialog(frm);
            }
        }

        private void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            if (!CanNavigate) return;
            using (FrmForgotPassword frm = new FrmForgotPassword())
            {
                ShowAuthDialog(frm);
            }
        }

        private void ShowAuthDialog(Form form)
        {
            Hide();
            try
            {
                form.ShowDialog(this);
            }
            finally
            {
                // A completed session must never reveal this login form again.
                if (CanNavigate) Show();
            }
        }
    }
}
