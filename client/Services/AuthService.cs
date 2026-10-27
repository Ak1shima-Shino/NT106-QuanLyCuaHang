using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RetailClient.DTO;

namespace RetailClient.Services
{
    public class AuthService
    {
        private static readonly HttpClient Http = new HttpClient
        {
            BaseAddress = new Uri(
                "http://retail-management.test/wp-json/retail/v1/"
            )
        };

        // ── Login (Mẫu ban đầu) ──────────────────────────────
        public async Task<LoginResponse> LoginAsync(
            string username,
            string password)
        {
            var request = new LoginRequest
            {
                Username = username,
                Password = password
            };

            string json = JsonConvert.SerializeObject(request);

            using (var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"))
            using (var response = await Http.PostAsync("login", content))
            {
                string body =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error =
                        JsonConvert.DeserializeObject<ApiErrorResponse>(body);

                    throw new Exception(
                        error?.Message ?? "Đăng nhập thất bại."
                    );
                }

                var result =
                    JsonConvert.DeserializeObject<LoginResponse>(body);

                if (result == null ||
                    string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    throw new Exception(
                        "Phản hồi đăng nhập không hợp lệ."
                    );
                }

                return result;
            }
        }

        // ── Register (Task 3.2 - Tỷ) ──────────────────────────
        public async Task<RegisterResponse> RegisterAsync(
            RegisterRequest request)
        {
            string json = JsonConvert.SerializeObject(request);

            using (var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"))
            using (var response = await Http.PostAsync("register", content))
            {
                string body =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error =
                        JsonConvert.DeserializeObject<ApiErrorResponse>(body);

                    throw new Exception(
                        error?.Message ?? "Đăng ký thất bại."
                    );
                }

                var result =
                    JsonConvert.DeserializeObject<RegisterResponse>(body);

                if (result == null)
                {
                    throw new Exception(
                        "Phản hồi đăng ký không hợp lệ."
                    );
                }

                return result;
            }
        }

        // ── Forgot Password (Task 3.4 - Tỷ) ──────────────────
        public async Task<MessageResponse> ForgotPasswordAsync(
            ForgotPasswordRequest request)
        {
            string json = JsonConvert.SerializeObject(request);

            using (var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"))
            using (var response = await Http.PostAsync(
                "forgot-password", content))
            {
                string body =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error =
                        JsonConvert.DeserializeObject<ApiErrorResponse>(body);

                    throw new Exception(
                        error?.Message ?? "Yêu cầu khôi phục mật khẩu thất bại."
                    );
                }

                var result =
                    JsonConvert.DeserializeObject<MessageResponse>(body);

                if (result == null)
                {
                    throw new Exception(
                        "Phản hồi không hợp lệ."
                    );
                }

                return result;
            }
        }

        // ── Reset Password (Task 3.5 - Tỷ) ───────────────────
        public async Task<MessageResponse> ResetPasswordAsync(
            ResetPasswordRequest request)
        {
            string json = JsonConvert.SerializeObject(request);

            using (var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"))
            using (var response = await Http.PostAsync(
                "reset-password", content))
            {
                string body =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error =
                        JsonConvert.DeserializeObject<ApiErrorResponse>(body);

                    throw new Exception(
                        error?.Message ?? "Đặt lại mật khẩu thất bại."
                    );
                }

                var result =
                    JsonConvert.DeserializeObject<MessageResponse>(body);

                if (result == null)
                {
                    throw new Exception(
                        "Phản hồi không hợp lệ."
                    );
                }

                return result;
            }
        }
    }
}
