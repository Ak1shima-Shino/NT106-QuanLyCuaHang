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
        public async Task<MeResponse> GetMeAsync(
    string accessToken)
        {
            var request =
                new HttpRequestMessage(
                    HttpMethod.Get,
                    "me"
                );

            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken
                );

            using (var response =
                await Http.SendAsync(request))
            {
                string body =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        "Không lấy được thông tin người dùng."
                    );
                }

                var result =
                    JsonConvert.DeserializeObject<MeResponse>(body);

                return result;
            }
        }

        public async Task<MessageResponse> LogoutAsync(
        string accessToken)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "logout"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken
                );

            using (var response =
                await Http.SendAsync(request))
            {
                string body =
                    await response.Content.ReadAsStringAsync();

                if (response.StatusCode ==
                    HttpStatusCode.Unauthorized)
                {
                    return new MessageResponse
                    {
                        Success = true,
                        Message = "Token đã hết hạn."
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        "Đăng xuất thất bại."
                    );
                }

                return JsonConvert.DeserializeObject<MessageResponse>(
                    body
                );
            }
        }
    }
}