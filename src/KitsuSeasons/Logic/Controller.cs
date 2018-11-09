using Kitsu;
using Kitsu.Api;
using KitsuSeasons.Enums;
using KitsuSeasons.Interfaces;
using KitsuSeasons.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KitsuSeasons.Logic
{
    public class Controller : IController
    {
        public Controller()
        {
            DataStructure.SetupFolders();
        }

        public ISelectSeason GetPreviousSeason(ISelectSeason selectedSeason, ObservableCollection<ISelectSeason> seasonList)
        {
            if (selectedSeason == null)
            {
                selectedSeason = seasonList[0];
            }
            else
            {
                int index = seasonList.IndexOf(selectedSeason);
                selectedSeason = index < seasonList.Count - 1 ? seasonList[++index] : seasonList.Last();
            }

            return selectedSeason;
        }

        public ISelectSeason GetNextSeason(ISelectSeason selectedSeason, ObservableCollection<ISelectSeason> seasonList)
        {
            if (selectedSeason == null)
            {
                selectedSeason = seasonList[0];
            }
            else
            {
                int index = seasonList.IndexOf(selectedSeason);
                selectedSeason = index > 0 ? seasonList[--index] : seasonList.First();
            }

            return selectedSeason;
        }

        public ObservableCollection<ISelectSeason> PopulateSeasonSelection()
        {
            var list = new List<ISelectSeason>();

            for (int year = DateTime.Now.Year + 1; year > 1959; year--)
            {
                list.Add(new SelectSeason(Season.fall, year));
                list.Add(new SelectSeason(Season.summer, year));
                list.Add(new SelectSeason(Season.spring, year));
                list.Add(new SelectSeason(Season.winter, year));
            }

            return new ObservableCollection<ISelectSeason>(list);
        }

        public void SaveUsername(string username)
        {
            DataStructure.Save(new SaveData(username, string.Empty));
        }

        public SaveData LoadSaveData()
        {
            return DataStructure.Load();
        }

        public async void LoadSeasons(ObservableCollection<ISeasonExpander> seasonExpanders, ISelectSeason selectedSeason)
        {
            var data = DataStructure.Load();
            var auth = await Authentication.Authenticate(data.Username, new AES().Decrypt(data.Password));
            var user = await User.GetUserAsync(data.Username);

            LoadEntireSeason(seasonExpanders, selectedSeason.SeasonDisplay, selectedSeason.Year, (int)user.data[0].id);
        }

        private async void LoadEntireSeason(ObservableCollection<ISeasonExpander> seasonExpanders, Season season, int year, int userId)
        {
            var result = await Anime.GetSeason(season, year);
            await GetSeasonData(seasonExpanders, userId, result);
            await LoopSeasons(seasonExpanders, (string)result.links.next, userId);
        }

        private async Task<List<SeasonalAnime>> LoopSeasons(ObservableCollection<ISeasonExpander> seasonExpanders, string next, int userId)
        {
            var result = await Anime.GetSeasonNextPage(next);

            List<SeasonalAnime> season = await GetSeasonData(seasonExpanders, userId, result);

            try
            {
                await LoopSeasons(seasonExpanders, (string)result.links.next, userId);
            }
            catch (Exception)
            {
            }

            return season;
        }

        private async Task<List<SeasonalAnime>> GetSeasonData(ObservableCollection<ISeasonExpander> seasonExpanders, int userId, dynamic result)
        {
            List<SeasonalAnime> season = new List<SeasonalAnime>();

            foreach (var item in result.data)
            {
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

                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => AddSeasonalAnimeToList(seasonExpanders, seasonalAnime, animeDetails)));
            }

            return season;
        }

        private void AddSeasonalAnimeToList(ObservableCollection<ISeasonExpander> seasonExpanders, SeasonalAnime anime, dynamic animeDetails)
        {
            string smallImage = (string)animeDetails.data.attributes.posterImage.small;

            var imageLocation = DownloadImage(smallImage, anime.Id);

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
          
            var seasonEntry = new SeasonEntry(anime.Name, 1, imageLocation, SeasonType.movie, AiringStatus.current, 1, "", AgeRating.G);
            seasonExpanders[index].SeasonEntries.Add(seasonEntry);
        }

        private string DownloadImage(string url, int animeId)
        {
            string location = Path.Combine(DataStructure.ImageFolder, $"{animeId}.jpg");

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), location);
            }

            return location;
        }
    }
}
