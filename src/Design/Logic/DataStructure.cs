using System;
using System.IO;
using Design.Models;
using Newtonsoft.Json;

namespace Design.Logic
{
    public static class DataStructure
    {
        private const string FolderName = "KitsuSeasons";
        private const string FileName = "SaveData.json";
        private static string SaveFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FolderName, FileName);

        public static void SetupFolders()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var mergedFolder = Path.Combine(appData, FolderName);
            if (!Directory.Exists(mergedFolder))
            {
                Directory.CreateDirectory(mergedFolder);
            }
        }

        public static void Save(SaveData saveData)
        {
            string serializedSaveData = string.Empty;

            if (File.Exists(SaveFilePath))
            {
                var json = File.ReadAllText(SaveFilePath);

                var deserializedData = JsonConvert.DeserializeObject<SaveData>(json);

                if (string.IsNullOrWhiteSpace(saveData.EmailAddress))
                {
                    saveData.EmailAddress = deserializedData.EmailAddress;
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
