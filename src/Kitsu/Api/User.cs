using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Kitsu.Api
{
    public class User
    {
        public static async Task<dynamic> GetUserAsync(string username)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/users?filter[name]={username}");
            return JsonConvert.DeserializeObject(json);
        }
    }
}
