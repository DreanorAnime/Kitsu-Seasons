using Kitsu;
using Kitsu.Api;
using KitsuSeasons.Interfaces;
using KitsuSeasons.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private const string PlaceHolderImage = "placeholder.jpg";

        private CancellationTokenSource cts;
        private int UserId { get; set; }
        private Action<int> SetMaxProgress { get; set; }
        private ObservableCollection<ISeasonExpander> SeasonExpanders { get; set; }
        private Details details;
        private ISelectSeason SelectedSeason { get; set; }

        public void LoadSeasons(ObservableCollection<ISeasonExpander> seasonExpanders, ISelectSeason selectedSeason, Action<int> setMaxProgress, Action<bool> progressVisible)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();

            SeasonExpanders = seasonExpanders;
            SetMaxProgress = setMaxProgress;

            new Task(async () =>
            {
                var data = DataStructure.Load();

                dynamic user;
                try
                {
                    var auth = await Authentication.Authenticate(data.Username, new AES().Decrypt(data.Password));
                    user = await User.GetUserAsync(data.Username);
                    UserId = (int)user.data[0].id;
                }
                catch (Exception e)
                {
                    ExecuteWithDispatcher(() =>
                    {
                        progressVisible(false);
                        MessageBoxSystem.OnShowMessageBox("Error", "Unable to authenticate. Please try again.");
                    });
                    return;
                }
               
                SelectedSeason = selectedSeason;
                LoadEntireSeason(selectedSeason.SeasonDisplay, selectedSeason.Year, cts.Token);
            }, cts.Token).Start();
        }

        private async void LoadEntireSeason(Season season, int year, CancellationToken token)
        {
            try
            {
                ClearList();

                var result = await Anime.GetSeason(season, year);

                await GetSeasonData(result, token);
                ExecuteWithDispatcher(() => SetMaxProgress((int) result.meta.count));

                await LoopSeasons((string) result.links.next, token);

                if (token.IsCancellationRequested)
                {
                    ClearList();
                }
            }
            catch
            {
                // ignored
            }
        }
   
        private async Task<List<SeasonalAnime>> LoopSeasons(string next, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return new List<SeasonalAnime>();
            }

            var result = await Anime.GetSeasonNextPage(next);

            List<SeasonalAnime> season = await GetSeasonData(result, token);

            try
            {
                await LoopSeasons((string)result.links.next, token);
            }
            catch
            {
                // ignored
            }

            return season;
        }

        private async Task<List<SeasonalAnime>> GetSeasonData(dynamic result, CancellationToken token)
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
                var anime = await Library.GetAnime(UserId, id);

                var animeDetails = await Anime.GetAnime(id);

                seasonalAnime = anime.data.Count > 0 
                    ? new SeasonalAnime(id, name, (string)anime.data[0].attributes.status, item.attributes, SelectedSeason.ToString(), true) 
                    : new SeasonalAnime(id, name, (string)item.attributes.status, item.attributes, SelectedSeason.ToString(), false);

                string posterImage = PlaceHolderImage;
                
                JObject jObject = animeDetails.data.attributes.posterImage;
                if (jObject != null)
                {
                    posterImage = (string)animeDetails.data.attributes.posterImage.small;
                }
              
                ExecuteWithDispatcher(() => AddSeasonalAnimeToList(seasonalAnime, posterImage, jObject != null));
            }

            return season;
        }

        private async void ExecuteWithDispatcher(Action action)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
        }

        private void AddSeasonalAnimeToList(SeasonalAnime anime, string smallImage, bool imageFound)
        {
            var imageLocation = LoadImage(smallImage, anime.Id, imageFound);

            int index = GetListIndexToModify(anime);

            var seasonEntry = new SeasonEntry(anime,
                imageLocation,
                index == 0 ? 150 : 0,
                () => AddAnimeToList(anime.Id),
                () => ShowAnimeDetails(anime, imageLocation));

            SeasonExpanders[index].SeasonEntries.Add(seasonEntry);
        }

        private void OpenOrCreateNewDetails(IDetailViewModel detailViewModel)
        {
            if (details == null)
            {
                details = new Details(detailViewModel);
            }
            else
            {
                details.Refresh(detailViewModel);
            }

            details.Show();
            details.Activate();
        }

        private void ShowAnimeDetails(SeasonalAnime anime, string imageLocation)
        {
            var detailViewModel = new DetailViewModel(anime, imageLocation,
                () => Process.Start($"https://kitsu.io/anime/{anime.Id}"),
                () => 
                {
                    if (!anime.IsInList)
                    {
                        AddAnimeToList(anime.Id);
                        details.Hide();
                    }

                    details.ShowMessage(anime.Name, "This is already on your list and can't be added again.");
                });

            OpenOrCreateNewDetails(detailViewModel);
        }

        private async void AddAnimeToList(int animeId)
        {
            var result = await Library.AddAnime(UserId, animeId, Status.planned);

            if (!string.IsNullOrEmpty(result) && result.Contains("errors"))
            {
                //todo logging
            }
            else
            {
                var expander = SeasonExpanders[0].SeasonEntries.FirstOrDefault(x => x.AnimeId == animeId);
                expander.AddButtonSize = 0;
                SeasonExpanders.First().SeasonEntries.Remove(expander);
                SeasonExpanders.Last().SeasonEntries.Add(expander);
            }
        }

        private void ClearList()
        {
            ExecuteWithDispatcher(() => 
            {
                for (var i = 0; i < SeasonExpanders.Count(); i++)
                {
                    SeasonExpanders[i].SeasonEntries.Clear();
                }
            });
        }

        private int GetListIndexToModify(SeasonalAnime anime)
        {
            var index = 0;
            if (!anime.IsInList) return index;
            
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

            return index;
        }

        private string LoadImage(string url, int animeId, bool imageFound)
        {
            if (!imageFound)
            {
                return Path.Combine(DataStructure.ImageFolder, PlaceHolderImage);
            }
            
            var location = Path.Combine(DataStructure.ImageFolder, $"{animeId}.jpg");
            if (File.Exists(location))
            {
                return location;
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(url), location);
                }
            }
            catch (Exception e)
            {
                return Path.Combine(DataStructure.ImageFolder, PlaceHolderImage);
            }

            return location;
        }
    }
}
