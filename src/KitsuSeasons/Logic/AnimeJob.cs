using Kitsu;
using Kitsu.Api;
using KitsuSeasons.Interfaces;
using KitsuSeasons.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KitsuSeasons.Logic
{
    public class AnimeJob
    {
        private CancellationTokenSource cts;

        public void LoadSeasons(ObservableCollection<ISeasonExpander> seasonExpanders, ISelectSeason selectedSeason, Action<int> setMaxProgress)
        {
            if (cts != null)
            {
                cts.Cancel();
            }
            cts = new CancellationTokenSource();

            new Task(async () =>
            {
                var data = DataStructure.Load();
                var auth = await Authentication.Authenticate(data.Username, new AES().Decrypt(data.Password));
                var user = await User.GetUserAsync(data.Username);

                LoadEntireSeason(seasonExpanders, selectedSeason.SeasonDisplay, selectedSeason.Year, (int)user.data[0].id, cts.Token, setMaxProgress);
            }, cts.Token).Start();
        }

        private async void LoadEntireSeason(ObservableCollection<ISeasonExpander> seasonExpanders, Season season, int year, int userId, CancellationToken token, Action<int> setMaxProgress)
        {
            try
            {
                ClearList(seasonExpanders);

                var result = await Anime.GetSeason(season, year);

                await GetSeasonData(seasonExpanders, userId, result, token);
                ExecuteWithDispatcher(() => setMaxProgress((int)result.meta.count));

                await LoopSeasons(seasonExpanders, (string)result.links.next, userId, token);

                if (token.IsCancellationRequested)
                {
                    ClearList(seasonExpanders);
                }
            }
            catch (Exception ex)
            {
            }
        }
   
        private async Task<List<SeasonalAnime>> LoopSeasons(ObservableCollection<ISeasonExpander> seasonExpanders, string next, int userId, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return new List<SeasonalAnime>();
            }

            var result = await Anime.GetSeasonNextPage(next);

            List<SeasonalAnime> season = await GetSeasonData(seasonExpanders, userId, result, token);

            try
            {
                await LoopSeasons(seasonExpanders, (string)result.links.next, userId, token);
            }
            catch (Exception)
            {
            }

            return season;
        }

        private async Task<List<SeasonalAnime>> GetSeasonData(ObservableCollection<ISeasonExpander> seasonExpanders, int userId, dynamic result, CancellationToken token)
        {
            List<SeasonalAnime> season = new List<SeasonalAnime>();

            foreach (var item in result.data)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                SeasonalAnime seasonalAnime = null;

                int id = (int)item.id;
                string name = (string)item.attributes.canonicalTitle;
                var anime = await Library.GetAnime(userId, id);

                var animeDetails = await Anime.GetAnime(id);

                if (anime.data.Count > 0)
                {
                    var status = anime.data[0].attributes.status;
                    seasonalAnime = new SeasonalAnime(id, name, (string)status, true);
                }
                else
                {
                    seasonalAnime = new SeasonalAnime(id, name, (string)item.attributes.status, false);
                }

                ExecuteWithDispatcher(() => AddSeasonalAnimeToList(seasonExpanders, seasonalAnime, animeDetails.data));
            }

            return season;
        }

        private async void ExecuteWithDispatcher(Action action)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
        }

        private void AddSeasonalAnimeToList(ObservableCollection<ISeasonExpander> seasonExpanders, SeasonalAnime anime, dynamic animeDetails)
        {
            string smallImage = (string)animeDetails.attributes.posterImage.small;

            var imageLocation = LoadImage(smallImage, anime.Id);

            int index = GetListIndexToModify(anime);

            var seasonEntry = new SeasonEntry(anime.Name,
                (string)animeDetails.attributes.episodeCount,
                imageLocation,
                (string)animeDetails.attributes.subtype,
                (string)animeDetails.attributes.status,
                (string)animeDetails.attributes.averageRating,
                (string)animeDetails.attributes.startDate,
                (string)animeDetails.attributes.endDate,
                (string)animeDetails.attributes.ageRating);

            seasonExpanders[index].SeasonEntries.Add(seasonEntry);
        }

        private void ClearList(ObservableCollection<ISeasonExpander> seasonExpanders)
        {
            ExecuteWithDispatcher(() => 
            {
                for (int i = 0; i < seasonExpanders.Count(); i++)
                {
                    seasonExpanders[i].SeasonEntries.Clear();
                }
            });
        }

        private int GetListIndexToModify(SeasonalAnime anime)
        {
            int index = 0;
            if (anime.IsInList)
            {
                switch (anime.StatusInlist)
                {
                    case Status.current:
                        index = 1;
                        break;
                    case Status.completed:
                        index = 2;
                        break;
                    case Status.on_hold:
                        index = 3;
                        break;
                    case Status.dropped:
                        index = 4;
                        break;
                    case Status.planned:
                        index = 5;
                        break;
                }
            }

            return index;
        }

        private string LoadImage(string url, int animeId)
        {
            string location = Path.Combine(DataStructure.ImageFolder, $"{animeId}.jpg");
            if (File.Exists(location))
            {
                return location;
            }

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), location);
            }

            return location;
        }
    }
}
