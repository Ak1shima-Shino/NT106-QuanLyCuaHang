using Newtonsoft.Json;
using RetailClient.DTO;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        // Login
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

        // Get Me
        public async Task<MeResponse> GetMeAsync(string accessToken)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "me"
            );

            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                accessToken
            );

            using (var response = await Http.SendAsync(request))
            {
                string body = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException(
                        "Phiên đăng nhập không còn hợp lệ."
                    );
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        "Không lấy được thông tin người dùng."
                    );
                }

                var result = JsonConvert.DeserializeObject<MeResponse>(body);

                if (result == null || !result.Success || result.User == null)
                {
                    throw new Exception("Phản hồi thông tin người dùng không hợp lệ.");
                }

                return result;
            }
        }

        // Logout
        public async Task<MessageResponse> LogoutAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "logout");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            using (var response = await Http.SendAsync(request))
            {
                string body = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new MessageResponse
                    {
                        Success = true,
                        Message = "Token đã hết hạn."
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Đăng xuất thất bại.");
                }

                return JsonConvert.DeserializeObject<MessageResponse>(body);
            }
        }

        // Register
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

        // Forgot Password
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

        // Reset Password
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
