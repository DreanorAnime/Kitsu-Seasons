using Design.Interfaces;
using System.Collections.ObjectModel;

namespace Design.Models
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
