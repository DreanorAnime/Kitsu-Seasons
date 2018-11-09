using KitsuSeasons.Interfaces;
using System.Collections.ObjectModel;

namespace KitsuSeasons.Models
{
    public class SeasonExpanderModel : ISeasonExpander
    {
        public SeasonExpanderModel(ObservableCollection<ISeasonEntry> seasonList, string header)
        {
            SeasonEntries = seasonList;
            Header = header;
        }

        public ObservableCollection<ISeasonEntry> SeasonEntries { get; private set; }

        public string Header { get; private set; }
    }
}
