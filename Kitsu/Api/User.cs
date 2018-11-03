using Newtonsoft.Json;
using System.Threading.Tasks;

namespace KitsuApi.Api
{
    public class User
    {
        public static async Task<dynamic> GetUserAsync( string text)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/users?filter[name]={text}");
            return JsonConvert.DeserializeObject(json);
        }
    }
}
