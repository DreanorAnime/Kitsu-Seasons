using Design.Models;
using System.Collections.ObjectModel;

namespace Design.Interfaces
{
    public interface IController
    {
        ObservableCollection<ISelectSeason> PopulateSeasonSelection();
        ISelectSeason GetPreviousSeason(ISelectSeason selectedSeason, ObservableCollection<ISelectSeason> seasonList);
        ISelectSeason GetNextSeason(ISelectSeason selectedSeason, ObservableCollection<ISelectSeason> seasonList);
        void SaveUsername(string emailAddress);
        SaveData LoadSaveData();
        void LoadSeasons(ObservableCollection<ISeasonExpander> seasonExpanders);
    }
}
