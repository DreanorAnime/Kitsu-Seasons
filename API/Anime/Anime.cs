using System.Threading.Tasks;
using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace Kitsu.Anime
{
    public static class Anime
    {
        /// <summary>
        /// Search for an anime with the name
        /// </summary>
        /// <param name="name">Anime name</param>
        /// <returns>List with anime data objects</returns>
        /// <exception cref="NoDataFoundException"></exception>
        public static async Task<AnimeByNameModel> GetAnimeAsync(string name)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/anime?filter[text]={name}");
            var anime = JsonConvert.DeserializeObject<AnimeByNameModel>(json);
            if (anime.Data.Count <= 0) throw new NoDataFoundException($"No anime was found with the name {name}");
            return anime;
        }
        
        /// <summary>
        /// Search for an anime with the name and page offset
        /// </summary>
        /// <param name="name">Anime name</param>
        /// <param name="offset">Page offset</param>
        /// <returns>List with anime data objects</returns>
        /// <exception cref="NoDataFoundException"></exception>
        public static async Task<AnimeByNameModel> GetAnimeAsync(string name, int offset)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/anime?filter[text]={name}&page[offset]={offset}");
            var anime = JsonConvert.DeserializeObject<AnimeByNameModel>(json);
            if (anime.Data.Count <= 0) throw new NoDataFoundException($"No anime was found with the name {name} and offset {offset}");
            return anime;
        }

        /// <summary>
        /// Search for an anime with its id
        /// </summary>
        /// <param name="id">Anime id</param>
        /// <returns>Object with anime data</returns>
        public static async Task<AnimeByIdModel> GetAnimeAsync(int id)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/anime/{id}");
            var anime = JsonConvert.DeserializeObject<AnimeByIdModel>(json);
            return anime;
        }

        public static async Task<dynamic> GetAnimeFromLibrary(int entry)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/library-entries/{entry}/anime");
            return JsonConvert.DeserializeObject(json);
        }

        public enum Season
        {
            None,
            summer,
            winter,
            spring,
            fall
        }

        public static async Task<dynamic> GetSeason(Season season, int year)
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/anime?filter[seasonYear]={year}&filter[season]={season}&page[limit]=20&page[offset]=0");
            return JsonConvert.DeserializeObject(json);
        }

        public static async Task<dynamic> GetSeasonNext(string next)
        {
            var json = await Kitsu.Client.GetStringAsync(next);
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// Get the trending anime
        /// </summary>
        /// <returns>List with anime data objects</returns>
        /// <exception cref="NoDataFoundException"></exception>
        public static async Task<AnimeByNameModel> GetTrendingAsync()
        {
            var json = await Kitsu.Client.GetStringAsync($"{Kitsu.BaseUri}/trending/anime");
            var trending = JsonConvert.DeserializeObject<AnimeByNameModel>(json);
            if (trending.Data.Count <= 0) throw new NoDataFoundException("Could not find any trending anime");
            return trending;
        }
    }
}