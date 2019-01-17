using ModelViewViewModel.commands;
using System.ComponentModel;

namespace KitsuSeasons.Interfaces
{
    public interface IDetailViewModel : INotifyPropertyChanged
    {
        string Synopsis { get; }

        string Title { get; }

        string AlternativeTitles { get; }

        string ImagePath { get; }

        ActionCommand OpenInKitsuCmd { get; }

        ActionCommand AddToListCmd { get; }
    }
}
