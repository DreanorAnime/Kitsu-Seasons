using System.Net.Http;
using System.Net.Http.Headers;

namespace Kitsu
{
    public class Kitsu
    {
        private static readonly string UserAgent = $"KitsuSeasons";

        public const string BaseUri = "https://kitsu.io/api/edge";
        public const string BaseAuthUri = "https://kitsu.io/api/oauth";

        public static readonly HttpClient Client = RequestClient();

        private static HttpClient RequestClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            return client;
        }
    }
}
