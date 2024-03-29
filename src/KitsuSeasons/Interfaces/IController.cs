﻿using KitsuSeasons.Models;
using System;
using System.Collections.ObjectModel;

namespace KitsuSeasons.Interfaces
{
    public interface IController
    {
        ObservableCollection<ISelectSeason> PopulateSeasonSelection();
        ISelectSeason GetPreviousSeason(ISelectSeason selectedSeason, ObservableCollection<ISelectSeason> seasonList);
        ISelectSeason GetNextSeason(ISelectSeason selectedSeason, ObservableCollection<ISelectSeason> seasonList);
        void SaveUsername(string emailAddress);
        SaveData LoadSaveData();
        void LoadSeasons(ObservableCollection<ISeasonExpander> seasonExpanders, ISelectSeason selectedSeason, Action<int> setMaxProgress, Action<bool> progressVisible);
        void FilterResults(ObservableCollection<ISeasonExpander> seasonExpanders, string filterText, bool includeNsfw);
        bool DoesFilterApply(ISeasonEntry entry, string filter, bool includeNsfw);
        int GetCurrentSeasonIndex();
        void SortResults(ObservableCollection<ISeasonExpander> seasonExpanders, int activeSort);
    }
}
