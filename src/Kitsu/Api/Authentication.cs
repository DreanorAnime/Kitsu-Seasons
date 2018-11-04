using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Kitsu.Api
{
    public static class Authentication
    {
        public static async Task<dynamic> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) throw new Exception("username or password can't be empty");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{Kitsu.BaseAuthUri}/token")
            {
                Content = new StringContent(
                    $"{{\"grant_type\": \"password\", \"username\": \"{username}\", \"password\": \"{password}\"}}",
                    Encoding.UTF8,
                    "application/vnd.api+json"
                )
            };

            var response = await Kitsu.Client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            dynamic auth = JsonConvert.DeserializeObject(json);
            if (string.IsNullOrEmpty((string)auth.access_token))
            {
                dynamic invalidAuth = JsonConvert.DeserializeObject(json);
                throw new Exception(invalidAuth.error_description);
            }

            Kitsu.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(UppercaseFirst((string)auth.token_type), (string)auth.access_token);
            return auth;
        }

        private static string UppercaseFirst(string authToken)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                return string.Empty;
            }
            return char.ToUpper(authToken[0]) + authToken.Substring(1);
        }
    }
}
