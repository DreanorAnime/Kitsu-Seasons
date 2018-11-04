using Kitsu;
using Kitsu.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitsuSeasons
{
    public class Sync
    {
        private Action<List<SeasonalAnime>> AddToView;

        public Sync(Action<List<SeasonalAnime>> addToView)
        {
            AddToView = addToView;
        }

        public async Task<List<SeasonalAnime>> LoadEntireSeason(Season season, int year, int userId)
        {
            var result = await Anime.GetSeason(season, year);
            List<SeasonalAnime> list = await GetSeasonData(userId, result);
            AddToView(list);
            list.AddRange(await LoopSeasons((string)result.links.next, userId));
            return list;
        }

        public async Task<List<SeasonalAnime>> LoopSeasons(string next, int userId)
        {
            var result = await Anime.GetSeasonNextPage(next);

            List<SeasonalAnime> season = await GetSeasonData(userId, result);

            AddToView(season);

            try
            {
                season.AddRange(await LoopSeasons((string)result.links.next, userId));
            }
            catch (Exception)
            {
            }

            return season;
        }

        public async Task<List<SeasonalAnime>> GetSeasonData(int userId, dynamic result)
        {
            List<SeasonalAnime> season = new List<SeasonalAnime>();

            foreach (var item in result.data)
            {
                int id = (int)item.id;
                string name = (string)item.attributes.canonicalTitle;
                var anime = await Library.GetAnime(userId, id);

                if (anime.data.Count > 0)
                {
                    var status = anime.data[0].attributes.status;
                    season.Add(new SeasonalAnime(id, name, (string)status, true));
                }
                else
                {
                    season.Add(new SeasonalAnime(id, name, (string)item.attributes.status, false));
                }
            }

            return season;
        }
    }
}
