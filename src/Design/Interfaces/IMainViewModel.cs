using ModelViewViewModel.commands;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Design.Interfaces
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
    }
}
