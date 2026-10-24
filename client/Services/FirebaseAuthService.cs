using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RetailClient.Services
{
    public class FirebaseAuthService
    {
        private static readonly HttpClient Http = new HttpClient();
        private const string ApiKey = "AIzaSyDSB1ionC_1Xfi5YGw6028aQF9GMF-parM";

        public async Task<string> LoginAsync(string email, string password)
        {
            string url =
                "https://identitytoolkit.googleapis.com/v1/" +
                "accounts:signInWithPassword?key=" + ApiKey;

            string body = JsonConvert.SerializeObject(new
            {
                email,
                password,
                returnSecureToken = true
            });

            using (var content = new StringContent(
                body, Encoding.UTF8, "application/json"))
            using (var response = await Http.PostAsync(url, content))
            {
                string responseBody =
                    await response.Content.ReadAsStringAsync();

                var data = JObject.Parse(responseBody);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        (string)data["error"]?["message"]
                        ?? "Đăng nhập Firebase thất bại.");
                }

                string idToken = (string)data["idToken"];

                if (string.IsNullOrWhiteSpace(idToken))
                {
                    throw new Exception(
                        "Firebase không trả về ID token.");
                }

                return idToken;
            }
        }
    }
}