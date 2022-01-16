using KitsuSeasons.Enums;
using ModelViewViewModel.commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using KitsuSeasons.Models;

namespace KitsuSeasons.Interfaces
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        ObservableCollection<ISeasonExpander> SeasonExpanders { get; }

        ObservableCollection<ISelectSeason> SeasonList { get; }

        ISelectSeason SelectedSeason { get; set; }

        ActionCommand OpenOptionsCmd { get; }

        ActionCommand CreateAccountCmd { get; }

        ActionCommand PreviousSeasonCmd { get; }

        ActionCommand NextSeasonCmd { get; }

        ActionCommand RefreshCmd{ get; }

        bool OptionsAreVisible { get; set; }

        string Username { get; set; }

        string FilterText { get; set; }

        bool IncludeNsfw { get; set; }

        IProgressViewModel ProgressModel { get; }

        List<string> SortActions { get; }

        int ActiveSort { get; }
    }
}
