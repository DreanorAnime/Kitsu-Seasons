using ModelViewViewModel.commands;
using System.ComponentModel;

namespace KitsuSeasons.Interfaces
{
    public interface ISeasonEntry : INotifyPropertyChanged
    {
        string ImagePath { get; }

        string Name { get; }

        string EpisodeText { get; }

        string Type { get; } 

        string Status { get; }

        string ScoreText { get; }

        string AiredText { get; } 

        string Rating { get; }

        int AddButtonSize { get; set; }

        ActionCommand AddAnimeToListCmd { get; }

        ActionCommand DoubleClickCmd { get; }

        int AnimeId { get; }

        bool IsHidden { get; set; }
    }
}
