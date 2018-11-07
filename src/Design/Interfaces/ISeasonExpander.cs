using System.Collections.ObjectModel;

namespace Design.Interfaces
{
    public interface ISeasonExpander
    {
        ObservableCollection<ISeasonEntry> SeasonEntries { get; }

        string Header { get; }
    }
}