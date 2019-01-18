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

        string Type { get; }

        string Episodes { get; }

        string Status { get; }

        string Aired { get; }

        string Season { get; }

        string Rating { get; }

        string Studios { get; }

        string Length { get; }

        string Categories { get; }

        ActionCommand OpenInKitsuCmd { get; }

        ActionCommand AddToListCmd { get; }
    }
}
