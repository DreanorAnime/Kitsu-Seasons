using Kitsu;
using KitsuSeasons.Interfaces;
using KitsuSeasons.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KitsuSeasons.Logic
{
    public class Controller : IController
    {
        private AnimeJob animeJob;

        public Controller()
        {
            DataStructure.SetupFolders();
            animeJob = new AnimeJob();
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

        public int GetCurrentSeasonIndex()
        {
            switch (DateTime.Now.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 7;
                case 4:
                case 5:
                case 6:
                    return 6;
                case 7:
                case 8:
                case 9:
                    return 5;
                case 10:
                case 11:
                case 12:
                    return 4;
                default:
                    return 0;
            }
        }

        public void SaveUsername(string username)
        {
            DataStructure.Save(new SaveData(username, string.Empty));
        }

        public SaveData LoadSaveData()
        {
            return DataStructure.Load();
        }

        public void LoadSeasons(ObservableCollection<ISeasonExpander> seasonExpanders, ISelectSeason selectedSeason, Action<int> setMaxProgress)
        {
            animeJob.LoadSeasons(seasonExpanders, selectedSeason, setMaxProgress);
        }

        public void FilterResults(ObservableCollection<ISeasonExpander> seasonExpanders, string filterText, bool includeNsfw)
        {
            string filter = filterText.ToLower();
            foreach (var expander in seasonExpanders)
            {
                foreach (var entry in expander.SeasonEntries)
                {
                    entry.IsHidden = DoesFilterApply(entry, filter, includeNsfw);
                }
            }
        }

        public bool DoesFilterApply(ISeasonEntry entry, string filter, bool includeNsfw)
        {
            bool shouldBeHidden = !(EqualOrContains(entry.Name, filter)
                           || EqualOrContains(entry.Status, filter)
                           || EqualOrContains(entry.Type, filter)
                           || EqualOrContains(entry.Rating, filter));

            if (!shouldBeHidden && entry.Nsfw && !includeNsfw)
            {
                return true;
            }

            return shouldBeHidden;
        }

        private bool EqualOrContains(string a, string b)
        {
            string c = a.ToLower();
            return c.Equals(b) || c.Contains(b);
        }

        public void SortResults(ObservableCollection<ISeasonExpander> seasonExpanders, int activeSort)
        {
            foreach (var expander in seasonExpanders)
            {
                if (activeSort == 1)
                {
                    expander.SeasonEntries = new ObservableCollection<ISeasonEntry>(expander.SeasonEntries.OrderBy(x => CompareDate(x.Anime.StartDate)));

                }
                if (activeSort == 2)
                {
                    expander.SeasonEntries = new ObservableCollection<ISeasonEntry>(expander.SeasonEntries.OrderBy(x => CompareDate(x.Anime.EndDate)));
                }
            }
        }

        private static DateTime CompareDate(string startDate)
        {
            DateTime.TryParse(startDate.Replace("*", string.Empty), out DateTime dt);
            return dt;
        }
    }
}
