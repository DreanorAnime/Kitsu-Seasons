﻿using KitsuSeasons.Enums;
using KitsuSeasons.Interfaces;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace KitsuSeasons.Models
{
    public sealed class MainViewModel : ViewModelBase<IMainViewModel>, IMainViewModel
    {
        private readonly IController controller;
        public ActionCommand OpenOptionsCmd => new ActionCommand(() => OptionsAreVisible = true);
        public ActionCommand CreateAccountCmd => new ActionCommand(() => Process.Start("https://kitsu.io/"));
        public ActionCommand PreviousSeasonCmd => new ActionCommand(() => SelectedSeason = controller.GetPreviousSeason(SelectedSeason, SeasonList));
        public ActionCommand NextSeasonCmd => new ActionCommand(() => SelectedSeason = controller.GetNextSeason(SelectedSeason, SeasonList));
        public ActionCommand RefreshCmd => new ActionCommand(() => 
        {
            ProgressModel.ResetValues();
            controller.LoadSeasons(SeasonExpanders, SelectedSeason, x => ProgressModel.ProgressMaximum = x, y => ProgressModel.ProgressIsVisible = y);
        });

        public MainViewModel(IController controller)
        {
            ProgressModel = new ProgressViewModel();
            this.controller = controller;
            var saveData = controller.LoadSaveData();
            Username = saveData.Username;
            SeasonList = controller.PopulateSeasonSelection();
            SelectedSeason = SeasonList[controller.GetCurrentSeasonIndex()];

            SortActions = new List<string> { "No Sort", "by Start Date", "by End Date", "by Score"};

            FilterText = string.Empty;
            SeasonExpanders = new ObservableCollection<ISeasonExpander>
            {
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Not in List"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Currently watching"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Completed"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "On hold"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Dropped"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Plan to watch")
            };
        
            //AddValidationRule(x => x.Username, new ValidationRule(() => Validator.EmailAddressIsValid(Username), "This is not a valid Email"));
            PropertyChanged += OnPropertyChanged;

            foreach (var item in SeasonExpanders)
            {
                item.SeasonEntries.CollectionChanged += SeasonEntries_CollectionChanged;
            }
        }

        public bool OptionsAreVisible
        {
            get { return Get(x => x.OptionsAreVisible); }
            set { Set(x => x.OptionsAreVisible, value); }
        }

        public IProgressViewModel ProgressModel
        {
            get { return Get(x => x.ProgressModel); }
            private set { Set(x => x.ProgressModel, value); }
        }

        public ObservableCollection<ISeasonExpander> SeasonExpanders
        {
            get { return Get(x => x.SeasonExpanders); }
            private set { Set(x => x.SeasonExpanders, value); }
        }

        public ObservableCollection<ISelectSeason> SeasonList
        {
            get { return Get(x => x.SeasonList); }
            private set { Set(x => x.SeasonList, value); }
        }

        public ISelectSeason SelectedSeason
        {
            get { return Get(x => x.SelectedSeason); }
            set { Set(x => x.SelectedSeason, value); }
        }

        public string Username
        {
            get { return Get(x => x.Username); }
            set { Set(x => x.Username, value); }
        }

        public string FilterText
        {
            get { return Get(x => x.FilterText); }
            set { Set(x => x.FilterText, value); }
        }

        public bool IncludeNsfw
        {
            get { return Get(x => x.IncludeNsfw); }
            set { Set(x => x.IncludeNsfw, value); }
        }

        public List<string> SortActions
        {
            get { return Get(x => x.SortActions); }
            set { Set(x => x.SortActions, value); }
        }

        public int ActiveSort
        {
            get { return Get(x => x.ActiveSort); }
            set { Set(x => x.ActiveSort, value); }
        }
        
        private void SeasonEntries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add) return;
            
            if (ProgressModel.ProgressValue != ProgressModel.ProgressMaximum)
            {
                SetProgress();
            }

            var entry = (ISeasonEntry)e.NewItems[0];
            entry.IsHidden = controller.DoesFilterApply(entry, FilterText.ToLower(), IncludeNsfw);
        }

        private void SetProgress()
        {
            ProgressModel.ProgressIsVisible = true;
            ProgressModel.ProgressIsIndeterminate = false;

            ProgressModel.ProgressValue++;
            double percent = ((double)ProgressModel.ProgressValue / (double)ProgressModel.ProgressMaximum) * 100;
            ProgressModel.ProgressText = $"{ProgressModel.ProgressValue}/{ProgressModel.ProgressMaximum} ({Math.Round(percent, 2)}%)";

            if (ProgressModel.ProgressValue == ProgressModel.ProgressMaximum)
            {
                ProgressModel.ProgressIsVisible = false;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GetPropertyName(x => x.Username) && string.IsNullOrWhiteSpace(this[GetPropertyName(x => x.Username)]))
            {
                controller.SaveUsername(Username);
            }

            if (e.PropertyName == GetPropertyName(x => x.FilterText)
                || e.PropertyName == GetPropertyName(x => x.IncludeNsfw))
            {
                controller.FilterResults(SeasonExpanders, FilterText, IncludeNsfw);
            }

            if (e.PropertyName == GetPropertyName(x => x.ActiveSort))
            {
                controller.SortResults(SeasonExpanders, ActiveSort);
            }
        }
    }
}
