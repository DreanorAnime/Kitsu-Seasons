using KitsuSeasons.Interfaces;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System;

namespace KitsuSeasons.Models
{
    public class DetailViewModel : ViewModelBase<IDetailViewModel>, IDetailViewModel
    {
        public ActionCommand OpenInKitsuCmd { get; }
        public ActionCommand AddToListCmd { get; }

        public DetailViewModel(SeasonalAnime anime, string imagePath, Action openInKitsu, Action addAnimeToList)
        {
            Synopsis = anime.Synopsis;
            Title = anime.Name;
            AlternativeTitles = anime.AlternativeTitles;
            Type = anime.Type;
            Episodes = anime.Episodes;
            Type = anime.Type;
            Status = anime.StatusInlist.ToString();
            Aired = $"{anime.StartDate} to {anime.EndDate}";
            Rating = $"{anime.AverageRating}%";
            Length = anime.TotalLength;
            Season = anime.Season.Replace("- ", string.Empty);

            //todo
            Studios = "-";
            Categories = "-";

            ImagePath = imagePath;

            OpenInKitsuCmd = new ActionCommand(openInKitsu);
            AddToListCmd = new ActionCommand(addAnimeToList);
        }

        public string Synopsis
        {
            get { return Get(x => x.Synopsis); }
            private set { Set(x => x.Synopsis, value); }
        }

        public string Title
        {
            get { return Get(x => x.Title); }
            private set { Set(x => x.Title, value); }
        }

        public string AlternativeTitles
        {
            get { return Get(x => x.AlternativeTitles); }
            private set { Set(x => x.AlternativeTitles, value); }
        }

        public string ImagePath
        {
            get { return Get(x => x.ImagePath); }
            private set { Set(x => x.ImagePath, value); }
        }

        public string Type
        {
            get { return Get(x => x.Type); }
            private set { Set(x => x.Type, value); }
        }

        public string Episodes
        {
            get { return Get(x => x.Episodes); }
            private set { Set(x => x.Episodes, value); }
        }

        public string Status
        {
            get { return Get(x => x.Status); }
            private set { Set(x => x.Status, value); }
        }

        public string Aired
        {
            get { return Get(x => x.Aired); }
            private set { Set(x => x.Aired, value); }
        }

        public string Season
        {
            get { return Get(x => x.Season); }
            private set { Set(x => x.Season, value); }
        }

        public string Rating
        {
            get { return Get(x => x.Rating); }
            private set { Set(x => x.Rating, value); }
        }

        public string Studios
        {
            get { return Get(x => x.Studios); }
            private set { Set(x => x.Studios, value); }
        }

        public string Length
        {
            get { return Get(x => x.Length); }
            private set { Set(x => x.Length, value); }
        }

        public string Categories
        {
            get { return Get(x => x.Categories); }
            private set { Set(x => x.Categories, value); }
        }
    }
}
