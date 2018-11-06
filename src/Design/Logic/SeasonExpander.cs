using Design.Interfaces;
using System.Collections.ObjectModel;

namespace Design.Logic
{
    public class SeasonExpander : ISeasonExpander
    {
        public ObservableCollection<ISeason> SeasonList { get; private set; }

        public string Header { get; private set; }

        public SeasonExpander(ObservableCollection<ISeason> seasonList, string header)
        {
            SeasonList = seasonList;
            Header = header;
        }
    }
}
