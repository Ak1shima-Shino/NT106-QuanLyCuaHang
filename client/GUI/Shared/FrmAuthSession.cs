using RetailClient.GUI.Auth;
using RetailClient.Helpers;
using RetailClient.Services;
using System;
using System.Windows.Forms;

namespace RetailClient.GUI.Shared
{
    public partial class FrmAuthSession : Form
    {
        public FrmAuthSession()
        {
            InitializeComponent();
            LoadUser();
        }

        private void LoadUser()
        {
            lblUsername.Text =
                SessionManager.CurrentUser.Username;

            lblEmail.Text =
                SessionManager.CurrentUser.Email;

            lblRoles.Text =
                string.Join(
                    ", ",
                    SessionManager.CurrentUser.Roles
                );
        }

        private async void btnRefreshMe_Click(object sender,EventArgs e)
        {
            try
            {
                var authService = new AuthService();

                var me = await authService.GetMeAsync(SessionManager.AccessToken
                );

                SessionManager.UpdateUser(me.User);

                LoadUser();
            }
            catch (UnauthorizedAccessException)
            {
                SessionManager.Clear();

                MessageBox.Show(
                    "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                FrmLogin login = new FrmLogin();
                login.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async void btnLogout_Click(
    object sender,
    EventArgs e)
        {
            try
            {
                var authService = new AuthService();

                await authService.LogoutAsync(
                    SessionManager.AccessToken
                );

                SessionManager.Clear();

                FrmLogin login = new FrmLogin();

                login.Show();

                this.Close();
            }
            catch (Exception)
            {
                var result = MessageBox.Show(
                    "Không thể kết nối server.\n" +
                    "Bạn có muốn đăng xuất cục bộ không?",
                    "Mất kết nối",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    SessionManager.Clear();

                    FrmLogin login =
                        new FrmLogin();

                    login.Show();

                    this.Close();
                }
            }
        }

        private void FrmAuthSession_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}