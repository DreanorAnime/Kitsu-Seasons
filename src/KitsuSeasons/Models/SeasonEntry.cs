using KitsuSeasons.Interfaces;
using ModelViewViewModel.commands;
using System;
using System.Globalization;

namespace KitsuSeasons.Models
{
    public class SeasonEntry : ISeasonEntry
    {
        public SeasonEntry(string name, string episodes, string imagePath, string type, string status, string score, string startDate, string endDate, string rating, int buttonSize, Action addAnimeToList)
        {
            string formattedStartDate = string.IsNullOrWhiteSpace(startDate) ? "-" 
                : DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");

            string formattedEndDate = string.IsNullOrWhiteSpace(endDate) ? "-" 
                : DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");

            Name = name;
            ImagePath = imagePath;
            EpisodeText = $"Episodes: {GetValueOrDefault(episodes)}";
            Type = UppercaseFirst(GetValueOrDefault(type.ToString()));
            Status = UppercaseFirst(GetValueOrDefault(status.ToString()));
            ScoreText = $"Score: {GetValueOrDefault(score)}%";
            AiredText = $"Aired: {formattedStartDate} to {formattedEndDate}";
            Rating = UppercaseFirst(GetValueOrDefault(rating));
            AddButtonSize = buttonSize;
            AddAnimeToListCmd = new ActionCommand(addAnimeToList);
        }

        public ActionCommand AddAnimeToListCmd { get; }

        public string ImagePath { get; }

        public string Name { get; }

        public string EpisodeText { get; }

        public string Type { get; }

        public string Status { get; }

        public string ScoreText { get; }

        public string AiredText { get; }

        public string Rating { get; }

        public int AddButtonSize { get; }

        private static string UppercaseFirst(string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        private string GetValueOrDefault(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }
    }
}
