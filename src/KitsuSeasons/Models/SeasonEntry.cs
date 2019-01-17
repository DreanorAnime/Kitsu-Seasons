using KitsuSeasons.Interfaces;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System;
using System.Globalization;

namespace KitsuSeasons.Models
{
    public class SeasonEntry : ViewModelBase<ISeasonEntry>, ISeasonEntry
    {
        public SeasonEntry(string name, string episodes, string imagePath, string type, string status, string score, string startDate, string endDate, string rating, int buttonSize, int animeId, Action addAnimeToList, Action showDetails)
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
            AnimeId = animeId;
            AddAnimeToListCmd = new ActionCommand(addAnimeToList);
            DoubleClickCmd = new ActionCommand(showDetails);
        }

        public ActionCommand AddAnimeToListCmd { get; }

        public ActionCommand DoubleClickCmd { get; }

        public string ImagePath
        {
            get { return Get(x => x.ImagePath); }
            private set { Set(x => x.ImagePath, value); }
        }

        public int AnimeId
        {
            get { return Get(x => x.AnimeId); }
            private set { Set(x => x.AnimeId, value); }
        }

        public string Name
        {
            get { return Get(x => x.Name); }
            private set { Set(x => x.Name, value); }
        }

        public string EpisodeText
        {
            get { return Get(x => x.EpisodeText); }
            private set { Set(x => x.EpisodeText, value); }
        }

        public string Type
        {
            get { return Get(x => x.Type); }
            private set { Set(x => x.Type, value); }
        }

        public string Status
        {
            get { return Get(x => x.Status); }
            private set { Set(x => x.Status, value); }
        }

        public string ScoreText
        {
            get { return Get(x => x.ScoreText); }
            private set { Set(x => x.ScoreText, value); }
        }

        public string AiredText
        {
            get { return Get(x => x.AiredText); }
            private set { Set(x => x.AiredText, value); }
        }

        public string Rating
        {
            get { return Get(x => x.Rating); }
            private set { Set(x => x.Rating, value); }
        }
        public int AddButtonSize
        {
            get { return Get(x => x.AddButtonSize); }
            set { Set(x => x.AddButtonSize, value); }
        }

        public bool IsHidden
        {
            get { return Get(x => x.IsHidden); }
            set { Set(x => x.IsHidden, value); }
        }

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
