using KitsuSeasons.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace KitsuSeasons.Interfaces
{
    public interface IController
    {
        ObservableCollection<ISelectSeason> PopulateSeasonSelection();
        ISelectSeason GetPreviousSeason(ISelectSeason selectedSeason, ObservableCollection<ISelectSeason> seasonList);
        ISelectSeason GetNextSeason(ISelectSeason selectedSeason, ObservableCollection<ISelectSeason> seasonList);
        void SaveUsername(string emailAddress);
        SaveData LoadSaveData();
        void LoadSeasons(ObservableCollection<ISeasonExpander> seasonExpanders, ISelectSeason selectedSeason, Action<int> setMaxProgress);
    }
}
