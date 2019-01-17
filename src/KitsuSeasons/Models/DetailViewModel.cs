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

        public DetailViewModel(string synopsis, string title, string alternativeTitles, string imagePath, Action openInKitsu, Action addAnimeToList)
        {
            Synopsis = synopsis;
            Title = title;
            AlternativeTitles = alternativeTitles;
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
    }
}
