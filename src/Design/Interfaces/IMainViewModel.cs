using ModelViewViewModel.commands;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Design.Interfaces
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        ObservableCollection<ISeasonExpander> SeasonExpanders { get; }

        ActionCommand OpenOptionsCmd { get; }

        bool OptionsAreVisible { get; set; }
    }
}
