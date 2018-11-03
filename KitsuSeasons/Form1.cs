using Kitsu.Anime;
using Kitsu.Authentication;
using Kitsu.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KitsuSeasons
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Season
        {
            public enum Status
            {
                Invalid,
                current,
                finished,
                tba,
                unreleased,
                upcoming,
                completed,
                dropped,
                on_hold,
                planned
            }

            public Season(int id, string name, string status, bool isInList)
            {
                Id = id;
                Name = name;
                StatusInlist = (Status)Enum.Parse(typeof(Status), status);
                IsInList = isInList;
            }

            public int Id { get; private set; }

            public string Name { get; private set; }

            public bool IsInList { get; }

            public Status StatusInlist { get; private set; }
        }


        private async Task<List<Season>> LoadEntireSeason(Anime.Season season, int year, int userId)
        {
            var result = await Anime.GetSeason(season, year);
            List<Season> list = await GetSeasonData(userId, result);
            AddToView(list);
            list.AddRange(await LoopSeasons((string)result.links.next, userId));
            return list;
        }

        private async Task<List<Season>> LoopSeasons(string next, int userId)
        {
            var result = await Anime.GetSeasonNext(next);

            List<Season> season = await GetSeasonData(userId, result);

            AddToView(season);

            try
            {
                season.AddRange(await LoopSeasons((string)result.links.next, userId));
            }
            catch (Exception ex)
            {
            }

            return season;
        }

        private void AddToView(List<Season> season)
        {
            foreach (var item in season)
            {
                ListViewItem lv = new ListViewItem(item.Name, item.IsInList ? listView1.Groups[0] : listView1.Groups[1]);
                lv.SubItems.Add(item.StatusInlist.ToString());
                listView1.Items.Add(lv);
            }
        }

        private async Task<List<Season>> GetSeasonData(int userId, dynamic result)
        {
            List<Season> season = new List<Season>();

            foreach (var item in result.data)
            {
                int id = (int)item.id;
                string name = (string)item.attributes.canonicalTitle;
                var anime = await User.GetAnimeFromLibrary(userId, id);

                if (anime.data.Count > 0)
                {
                    var status = anime.data[0].attributes.status;
                    season.Add(new Season(id, name, (string)status, true));
                }
                else
                {
                    season.Add(new Season(id, name, (string)item.attributes.status, false));
                }
            }

            return season;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = "user";
            string password = "password"; 
            var auth = await Authentication.Authenticate(username, password);

            var user = await User.GetUserAsync(FilterType.Name, username);
            var summerSeason = await LoadEntireSeason(Anime.Season.summer, 2018, user.Data[0].Id);
        }
    }
}
