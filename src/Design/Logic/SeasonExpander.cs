using Design.Interfaces;
using System.Collections.ObjectModel;

namespace Design.Logic
{
    public class SeasonExpander : ISeasonExpander
    {
        public SeasonExpander(ObservableCollection<ISeasonEntry> seasonList, string header)
        {
            SeasonEntries = seasonList;
            Header = header;
        }

        public ObservableCollection<ISeasonEntry> SeasonEntries { get; private set; }

        public string Header { get; private set; }
    }
}
