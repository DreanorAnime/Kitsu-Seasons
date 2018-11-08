using Design.Enums;
using Design.Interfaces;
using Design.Logic;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Design.Models
{
    public class MainViewModel : ViewModelBase<IMainViewModel>, IMainViewModel
    {
        private IController controller;

        public ActionCommand OpenOptionsCmd => new ActionCommand(() => OptionsAreVisible = true);
        public ActionCommand CreateAccountCmd => new ActionCommand(() => Process.Start("https://kitsu.io/"));
        public ActionCommand PreviousSeasonCmd => new ActionCommand(() => SelectedSeason = controller.GetPreviousSeason(SelectedSeason, SeasonList));
        public ActionCommand NextSeasonCmd => new ActionCommand(() => SelectedSeason = controller.GetNextSeason(SelectedSeason, SeasonList));
        public ActionCommand RefreshCmd => new ActionCommand(() => { });

        public MainViewModel(IController controller)
        {
            this.controller = controller;
            var saveData = controller.LoadSaveData();
            EmailAddress = saveData.EmailAddress;

            SeasonList = controller.PopulateSeasonSelection();

            AddValidationRule(x => x.EmailAddress, new ValidationRule(() => Validator.EmailAddressIsValid(EmailAddress), "This is not a valid Email"));

            PropertyChanged += OnPropertyChanged;

            SeasonExpanders = new ObservableCollection<ISeasonExpander>
            {
                new SeasonExpander(new ObservableCollection<ISeasonEntry>{
                    new SeasonEntry("Meh", 0, "", SeasonType.TV, AiringStatus.unreleased, 24.6, "test2", AgeRating.R18),
                    new SeasonEntry("Moo", 0, "", SeasonType.OVA, AiringStatus.upcoming, 4.6, "test2", AgeRating.G)
                }, "Not in list"),
                new SeasonExpander(new ObservableCollection<ISeasonEntry>{
                    new SeasonEntry("Meh", 0, "", SeasonType.TV, AiringStatus.unreleased, 24.6, "test2", AgeRating.R18),
                    new SeasonEntry("Moo", 0, "", SeasonType.OVA, AiringStatus.upcoming, 4.6, "test2", AgeRating.G)
                }, "Currently watching"),
                new SeasonExpander(new ObservableCollection<ISeasonEntry>{
                    new SeasonEntry("Meh", 0, "", SeasonType.TV, AiringStatus.unreleased, 24.6, "test2", AgeRating.R18),
                    new SeasonEntry("Moo", 0, "", SeasonType.OVA, AiringStatus.upcoming, 4.6, "test2", AgeRating.G)
                }, "Completed"),
                new SeasonExpander(new ObservableCollection<ISeasonEntry>{
                    new SeasonEntry("Meh", 0, "", SeasonType.TV, AiringStatus.unreleased, 24.6, "test2", AgeRating.R18),
                    new SeasonEntry("Moo", 0, "", SeasonType.OVA, AiringStatus.upcoming, 4.6, "test2", AgeRating.G)
                }, "On hold"),
                new SeasonExpander(new ObservableCollection<ISeasonEntry>{
                    new SeasonEntry("Meh", 0, "", SeasonType.TV, AiringStatus.unreleased, 24.6, "test2", AgeRating.R18),
                    new SeasonEntry("Moo", 0, "", SeasonType.OVA, AiringStatus.upcoming, 4.6, "test2", AgeRating.G)
                }, "Plan to watch"),
            };
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

        public string EmailAddress
        {
            get { return Get(x => x.EmailAddress); }
            set { Set(x => x.EmailAddress, value); }
        }


        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GetPropertyName(x => x.EmailAddress) && string.IsNullOrWhiteSpace(this[GetPropertyName(x => x.EmailAddress)]))
            {
                controller.SaveEmail(EmailAddress);
            }
        }
    }
}
