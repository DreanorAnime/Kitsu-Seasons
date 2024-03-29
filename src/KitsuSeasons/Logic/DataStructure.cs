﻿using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using KitsuSeasons.Models;
using KitsuSeasons.Properties;
using Newtonsoft.Json;

namespace KitsuSeasons.Logic
{
    public static class DataStructure
    {
        private const string HomeFolderName = "KitsuSeasons";
        private const string ImageFolderName = "Images";
        private const string FileName = "SaveData.json";

        private static string SaveFilePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), HomeFolderName,
                FileName);

        public static void SetupFolders()
        {
            if (!Directory.Exists(HomeFolder))
            {
                Directory.CreateDirectory(HomeFolder);
            }

            if (!Directory.Exists(ImageFolder))
            {
                Directory.CreateDirectory(ImageFolder);
            }

            var placeholder = Path.Combine(ImageFolder, "placeholder.jpg");
            if (!File.Exists(placeholder))
            {
                CreatePlaceholderImage(placeholder);
            }
        }

        private static void CreatePlaceholderImage(string path)
        {
            var resource = Application.GetResourceStream(new Uri("/img/placeholder.jpg", UriKind.RelativeOrAbsolute));
            Image image = new Image();
            if (resource != null)
            {
                image.Source = BitmapFrame.Create(resource.Stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource) image.Source));
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(stream);
                }
            }
        }

        private static string HomeFolder =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), HomeFolderName);

        public static string ImageFolder => Path.Combine(HomeFolder, ImageFolderName);

        public static void Save(SaveData saveData)
        {
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

            var serializedSaveData = JsonConvert.SerializeObject(saveData, Formatting.Indented);

            if (!string.IsNullOrWhiteSpace(serializedSaveData))
            {
                File.WriteAllText(SaveFilePath, serializedSaveData);
            }
        }

        public static SaveData Load()
        {
            if (!File.Exists(SaveFilePath)) return new SaveData();

            var json = File.ReadAllText(SaveFilePath);
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
    }
}