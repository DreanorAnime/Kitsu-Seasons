using Design.Interfaces;
using Design.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Design.Logic
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
                list.Add(new SelectSeason(Kitsu.Season.fall, year));
                list.Add(new SelectSeason(Kitsu.Season.summer, year));
                list.Add(new SelectSeason(Kitsu.Season.spring, year));
                list.Add(new SelectSeason(Kitsu.Season.winter, year));
            }

            return new ObservableCollection<ISelectSeason>(list);
        }

        public void SaveEmail(string emailAddress)
        {
            DataStructure.Save(new SaveData(emailAddress, string.Empty));
        }

        public SaveData LoadSaveData()
        {
            return DataStructure.Load();
        }
    }
}
