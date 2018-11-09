using System.Collections.ObjectModel;

namespace KitsuSeasons.Interfaces
{
    public interface ISeasonExpander
    {
        ObservableCollection<ISeasonEntry> SeasonEntries { get; }

        string Header { get; }
    }
}