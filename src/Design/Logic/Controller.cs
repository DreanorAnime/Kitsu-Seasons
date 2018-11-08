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
        private const string FolderName = "KitsuSeasons";
        private const string FileName = "SaveData.json";

        public Controller()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var mergedFolder = Path.Combine(appData, FolderName);
            if (!Directory.Exists(mergedFolder))
            {
                Directory.CreateDirectory(mergedFolder);
            }

            AppFolder = mergedFolder;
            SaveFilePath = Path.Combine(AppFolder, FileName);
        }

        private string AppFolder { get; }

        private string SaveFilePath { get; }

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
            string serializedSaveData = string.Empty;

            if (File.Exists(SaveFilePath))
            {
                var json = File.ReadAllText(SaveFilePath);

                var saveData = JsonConvert.DeserializeObject<SaveData>(json);

                if (saveData.EmailAddress != emailAddress)
                {
                    saveData.EmailAddress = emailAddress;
                    serializedSaveData = JsonConvert.SerializeObject(saveData, Formatting.Indented);
                }
            }
            else
            {
                serializedSaveData = JsonConvert.SerializeObject(new SaveData(emailAddress, string.Empty), Formatting.Indented);
            }

            if (!string.IsNullOrWhiteSpace(serializedSaveData))
            {
                File.WriteAllText(SaveFilePath, serializedSaveData);
            }
        }

        public SaveData LoadSaveData()
        {
            if (File.Exists(SaveFilePath))
            {
                var json = File.ReadAllText(SaveFilePath);
                return JsonConvert.DeserializeObject<SaveData>(json);
            }

            return new SaveData();
        }
    }
}
