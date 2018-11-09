using Design.Enums;
using Design.Interfaces;
using Design.Logic;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Design.Models
{
    public class MainViewModel : ViewModelBase<IMainViewModel>, IMainViewModel
    {
        private IController controller;

        public ActionCommand OpenOptionsCmd => new ActionCommand(() => OptionsAreVisible = true);
        public ActionCommand CreateAccountCmd => new ActionCommand(() => Process.Start("https://kitsu.io/"));
        public ActionCommand PreviousSeasonCmd => new ActionCommand(() => SelectedSeason = controller.GetPreviousSeason(SelectedSeason, SeasonList));
        public ActionCommand NextSeasonCmd => new ActionCommand(() => SelectedSeason = controller.GetNextSeason(SelectedSeason, SeasonList));
        public ActionCommand RefreshCmd => new ActionCommand(() => controller.LoadSeasons(SeasonExpanders));

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

            //AddValidationRule(x => x.Username, new ValidationRule(() => Validator.EmailAddressIsValid(Username), "This is not a valid Email"));
            PropertyChanged += OnPropertyChanged;
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


        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GetPropertyName(x => x.Username) && string.IsNullOrWhiteSpace(this[GetPropertyName(x => x.Username)]))
            {
                controller.SaveUsername(Username);
            }
        }
    }
}
