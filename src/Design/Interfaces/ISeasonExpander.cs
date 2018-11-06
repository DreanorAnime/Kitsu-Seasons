using System.Collections.ObjectModel;

namespace Design.Interfaces
{
    public interface ISeasonExpander
    {
        ObservableCollection<ISeason> SeasonList { get; }

        string Header { get; }
    }
}