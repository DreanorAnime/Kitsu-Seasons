using System;
using System.IO;
using KitsuSeasons.Models;
using Newtonsoft.Json;

namespace KitsuSeasons.Logic
{
    public static class DataStructure
    {
        private const string HomeFolderName = "KitsuSeasons";
        private const string ImageFolderName = "Images";
        private const string FileName = "SaveData.json";
        private static string SaveFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), HomeFolderName, FileName);

        public static void SetupFolders()
        {
            if (!Directory.Exists(HomeFolder))
            {
                Directory.CreateDirectory(HomeFolder);
            }

            if (!Directory.Exists(HomeFolder))
            {
                Directory.CreateDirectory(HomeFolder);
            }
        }

        public static string HomeFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), HomeFolderName); } }

        public static string ImageFolder { get { return Path.Combine(HomeFolder, ImageFolderName); } }

        public static void Save(SaveData saveData)
        {
            string serializedSaveData = string.Empty;

            if (File.Exists(SaveFilePath))
            {
                var json = File.ReadAllText(SaveFilePath);

                var deserializedData = JsonConvert.DeserializeObject<SaveData>(json);

                if (string.IsNullOrWhiteSpace(saveData.Username))
                {
                    saveData.Username = deserializedData.Username;
                }

                if (string.IsNullOrWhiteSpace(saveData.Password))
                {
                    saveData.Password = deserializedData.Password;
                }
            }
    
            serializedSaveData = JsonConvert.SerializeObject(saveData, Formatting.Indented);

            if (!string.IsNullOrWhiteSpace(serializedSaveData))
            {
                File.WriteAllText(SaveFilePath, serializedSaveData);
            }
        }

        public static SaveData Load()
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
