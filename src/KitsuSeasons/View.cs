using Kitsu;
using Kitsu.Api;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KitsuSeasons
{
    public partial class View : Form
    {
        private Sync sync;

        public View()
        {
            InitializeComponent();
            sync = new Sync(AddToView);
        }

        private void AddToView(List<SeasonalAnime> season)
        {
            foreach (var item in season)
            {
                ListViewItem lv = new ListViewItem(item.Name, item.IsInList ? listView1.Groups[0] : listView1.Groups[1]);
                lv.SubItems.Add(item.StatusInlist.ToString());
                lv.SubItems.Add(item.StatusInlist.ToString());
                listView1.Items.Add(lv);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = "user";
            string password = "password"; 
            var auth = await Authentication.Authenticate(username, password);

            var user = await User.GetUserAsync(username);

            //string response = await Library.AddAnime((int)user.data[0].id, 40962, Status.on_hold);

            var summerSeason = await sync.LoadEntireSeason(Season.summer, 2018, (int)user.data[0].id);
        }
    }
}
