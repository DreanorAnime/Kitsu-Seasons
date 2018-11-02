using Kitsu.Anime;
using Kitsu.User;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Kitsu.Authentication;

namespace KitsuSeasons
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var auth = await Authentication.Authenticate("user", "password");

            var user = await User.GetUserAsync(FilterType.Name, "user");

            var summerSeason = await LoadEntireSeason(Anime.Season.summer, 2018);
            var userLibrary = await LoadEntireLibrary(int.Parse(user.Data[0].Id));

            foreach (var item in summerSeason)
            {
                if (!userLibrary.Contains(item.Id))
                {
                    Debug.WriteLine(item.Id);
                }
            }

        }

        private async Task<List<int>> LoadEntireLibrary(int userId)
        {
            List<int> ids = new List<int>();
            var userLibrary = await User.GetUserLibrary(userId);

            foreach (var item in userLibrary.data)
            {
                var libraryId = (int)item.id;
                var anime = await Anime.GetAnimeFromLibrary(libraryId);
                ids.Add((int)anime.data.id);
            }

            ids.AddRange(await GetLibraryIds((string)userLibrary.links.next));

            return ids;
        }

        public class Season
        {
            public Season(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; private set; }

            public string Name { get; private set; }
        }

        private async Task<List<int>> GetLibraryIds(string next)
        {
            List<int> ids = new List<int>();

            var userLibrary = await User.GetUserLibraryNext(next);

            foreach (var item in userLibrary.data)
            {
                var libraryId = (int)item.id;
                var anime = await Anime.GetAnimeFromLibrary(libraryId);
                ids.Add((int)anime.data.id);
            }

            try
            {
                ids.AddRange(await GetLibraryIds((string)userLibrary.links.next));
            }
            catch (Exception ex)
            {
            }

            return ids;
        }

        private async Task<List<Season>> LoadEntireSeason(Anime.Season season, int year)
        {
            List<Season> ids = new List<Season>();

            var result = await Anime.GetSeason(season, year);

            foreach (var item in result.data)
            {
                int id = (int)item.id;
                string name = (string)item.attributes.titles.en_jp;
                ids.Add(new Season(id, name));
            }

            ids.AddRange(await GetSeasonIds((string)result.links.next));

            return ids;
        }

        private async Task<List<Season>> GetSeasonIds(string next)
        {
            List<Season> ids = new List<Season>();

            var result = await Anime.GetSeasonNext(next);

            foreach (var item in result.data)
            {
                int id = (int)item.id;
                string name = (string)item.canonicalTitle;
                ids.Add(new Season(id, name));
            }

            try
            {
                ids.AddRange(await GetSeasonIds((string)result.links.next));
            }
            catch (Exception ex)
            {
            }

            return ids;
        }
    }
}
