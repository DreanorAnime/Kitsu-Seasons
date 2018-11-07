using Design.Enums;
using Design.Interfaces;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace Design.Logic
{
    public class MainViewModel : ViewModelBase<IMainViewModel>, IMainViewModel
    {
        public ActionCommand OpenOptionsCmd => new ActionCommand(() => OptionsAreVisible = true);
        public ActionCommand CreateAccountCmd => new ActionCommand(() => Process.Start("https://kitsu.io/"));

        public MainViewModel()
        {
            PropertyChanged += OnPropertyChanged;
            SeasonExpanders = new ObservableCollection<ISeasonExpander>
            {
                new SeasonExpander(new ObservableCollection<ISeason>{
                    new Season("Meh", 0, "", SeasonType.TV, AiringStatus.unreleased, 24.6, "test2", AgeRating.R18),
                    new Season("Moo", 0, "", SeasonType.OVA, AiringStatus.upcoming, 4.6, "test2", AgeRating.G)
                }, "Headertext"),
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

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
