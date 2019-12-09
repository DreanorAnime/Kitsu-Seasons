using Kitsu;
using KitsuSeasons.Interfaces;
using KitsuSeasons.Logic;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System;

namespace KitsuSeasons.Models
{
    public class SeasonEntry : ViewModelBase<ISeasonEntry>, ISeasonEntry
    {
        public SeasonEntry(SeasonalAnime seasonalAnime, string imagePath, int buttonSize, Action addAnimeToList, Action showDetails)
        {
            Anime = seasonalAnime;
            Name = seasonalAnime.Name;
            ImagePath = imagePath;
            EpisodeText = $"Episodes: {seasonalAnime.Episodes}";
            Type = seasonalAnime.Type;
            Status = seasonalAnime.StatusInlist.ToString();
            ScoreText = $"Score: {seasonalAnime.AverageRating}%";
            AiredText = $"Aired: {seasonalAnime.StartDate} to {seasonalAnime.EndDate}";
            Rating = seasonalAnime.AgeRating;
            AddButtonSize = buttonSize;
            AnimeId = seasonalAnime.Id;
            Nsfw = seasonalAnime.Nsfw;
            AddAnimeToListCmd = new ActionCommand(addAnimeToList);
            DoubleClickCmd = new ActionCommand(showDetails);
        }

        public SeasonalAnime Anime { get; }

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

        public bool Nsfw
        {
            get { return Get(x => x.Nsfw); }
            private set { Set(x => x.Nsfw, value); }
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
    }
}
