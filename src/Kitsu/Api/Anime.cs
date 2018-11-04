using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Kitsu.Api
{
    public class Anime
    {
        public static async Task<dynamic> GetSeason(Season season, int year)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/anime?filter[seasonYear]={year}&filter[season]={season}&page[limit]=20&page[offset]=0");
            return JsonConvert.DeserializeObject(json);
        }

        public static async Task<dynamic> GetSeasonNextPage(string next)
        {
            var json = await Kitsu.Client.GetStringAsync(next);
            return JsonConvert.DeserializeObject(json);
        }
    }
}
