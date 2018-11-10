using KitsuSeasons.Interfaces;
using ModelViewViewModel.Base;
using System.Collections.ObjectModel;

namespace KitsuSeasons.Models
{
    public class SeasonExpanderModel : ViewModelBase<ISeasonExpander>, ISeasonExpander
    {
        public SeasonExpanderModel(ObservableCollection<ISeasonEntry> seasonList, string header)
        {
            SeasonEntries = seasonList;
            Header = header;
        }

        public ObservableCollection<ISeasonEntry> SeasonEntries
        {
            get { return Get(x => x.SeasonEntries); }
            private set { Set(x => x.SeasonEntries, value); }
        }

        public string Header
        {
            get { return Get(x => x.Header); }
            private set { Set(x => x.Header, value); }
        }

    }
}
