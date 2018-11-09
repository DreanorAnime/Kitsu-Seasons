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

                if (anime.data.Count > 0)
                {
                    var status = anime.data[0].attributes.status;
                    seasonalAnime = new SeasonalAnime(id, name, (string)status, true);
                }
                else
                {
                    seasonalAnime = new SeasonalAnime(id, name, (string)item.attributes.status, false);
                }

                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => AddSeasonalAnimeToList(seasonExpanders, seasonalAnime)));
            }

            return season;
        }

        private void AddSeasonalAnimeToList(ObservableCollection<ISeasonExpander> seasonExpanders, SeasonalAnime anime)
        {
            var placeholder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "placeholder.jpg");

            if (!anime.IsInList)
            {
                seasonExpanders[0].SeasonEntries.Add(new SeasonEntry(anime.Name, 1, placeholder, SeasonType.movie, AiringStatus.current, 1, "", AgeRating.G));
            }
            else
            {
                switch (anime.StatusInlist)
                {
                    case Status.current:
                        seasonExpanders[1].SeasonEntries.Add(new SeasonEntry(anime.Name, 1, placeholder, SeasonType.movie, AiringStatus.current, 1, "", AgeRating.G));
                        break;
                    case Status.completed:
                        seasonExpanders[2].SeasonEntries.Add(new SeasonEntry(anime.Name, 1, placeholder, SeasonType.movie, AiringStatus.current, 1, "", AgeRating.G));
                        break;
                    case Status.on_hold:
                        seasonExpanders[3].SeasonEntries.Add(new SeasonEntry(anime.Name, 1, placeholder, SeasonType.movie, AiringStatus.current, 1, "", AgeRating.G));
                        break;
                    case Status.dropped:
                        seasonExpanders[4].SeasonEntries.Add(new SeasonEntry(anime.Name, 1, placeholder, SeasonType.movie, AiringStatus.current, 1, "", AgeRating.G));
                        break;
                    case Status.planned:
                        seasonExpanders[5].SeasonEntries.Add(new SeasonEntry(anime.Name, 1, placeholder, SeasonType.movie, AiringStatus.current, 1, "", AgeRating.G));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
