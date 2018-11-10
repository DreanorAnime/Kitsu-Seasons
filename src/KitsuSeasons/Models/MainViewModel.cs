using KitsuSeasons.Interfaces;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace KitsuSeasons.Models
{
    public class MainViewModel : ViewModelBase<IMainViewModel>, IMainViewModel
    {
        private IController controller;

        public ActionCommand OpenOptionsCmd => new ActionCommand(() => OptionsAreVisible = true);
        public ActionCommand CreateAccountCmd => new ActionCommand(() => Process.Start("https://kitsu.io/"));
        public ActionCommand PreviousSeasonCmd => new ActionCommand(() => SelectedSeason = controller.GetPreviousSeason(SelectedSeason, SeasonList));
        public ActionCommand NextSeasonCmd => new ActionCommand(() => SelectedSeason = controller.GetNextSeason(SelectedSeason, SeasonList));
        public ActionCommand RefreshCmd => new ActionCommand(() => 
        {
            ProgressIsIndeterminate = true;
            ProgressIsVisible = true;
            ProgressMaximum = 100;
            ProgressValue = 0;
            controller.LoadSeasons(SeasonExpanders, SelectedSeason, SetMaxProgress);
        });

        public MainViewModel(IController controller)
        {
            this.controller = controller;
            var saveData = controller.LoadSaveData();
            Username = saveData.Username;
            SeasonList = controller.PopulateSeasonSelection();

            SeasonExpanders = new ObservableCollection<ISeasonExpander>
            {
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Not in List"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Currently watching"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Completed"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "On hold"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Dropped"),
                new SeasonExpanderModel(new ObservableCollection<ISeasonEntry>(), "Plan to watch")
            };

            ProgressMaximum = 100;

            //AddValidationRule(x => x.Username, new ValidationRule(() => Validator.EmailAddressIsValid(Username), "This is not a valid Email"));
            PropertyChanged += OnPropertyChanged;

            foreach (var item in SeasonExpanders)
            {
                item.SeasonEntries.CollectionChanged += SeasonEntries_CollectionChanged;
            }
        }

        private void SeasonEntries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ProgressIsVisible = true;
                ProgressIsIndeterminate = false;

                ProgressValue++;
                double percent = ((double)ProgressValue / (double)ProgressMaximum) * 100;
                ProgressText = $"{ProgressValue}/{ProgressMaximum} ({Math.Round(percent, 2)}%)";

                if (ProgressValue == ProgressMaximum)
                {
                    ProgressIsVisible = false;
                }
            }
        }

        public bool OptionsAreVisible
        {
            get { return Get(x => x.OptionsAreVisible); }
            set { Set(x => x.OptionsAreVisible, value); }
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

        public int ProgressValue
        {
            get { return Get(x => x.ProgressValue); }
            set { Set(x => x.ProgressValue, value); }
        }

        public int ProgressMaximum
        {
            get { return Get(x => x.ProgressMaximum); }
            set { Set(x => x.ProgressMaximum, value); }
        }

        public string ProgressText
        {
            get { return Get(x => x.ProgressText); }
            set { Set(x => x.ProgressText, value); }
        }

        public bool ProgressIsVisible
        {
            get { return Get(x => x.ProgressIsVisible); }
            set { Set(x => x.ProgressIsVisible, value); }
        }

        public bool ProgressIsIndeterminate
        {
            get { return Get(x => x.ProgressIsIndeterminate); }
            set { Set(x => x.ProgressIsIndeterminate, value); }
        }

        private void SetMaxProgress(int max)
        {
            ProgressMaximum = max;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GetPropertyName(x => x.Username) && string.IsNullOrWhiteSpace(this[GetPropertyName(x => x.Username)]))
            {
                controller.SaveUsername(Username);
            }
        }
    }
}
