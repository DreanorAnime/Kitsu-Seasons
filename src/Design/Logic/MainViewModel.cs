using Design.Interfaces;
using ModelViewViewModel.Base;
using ModelViewViewModel.commands;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Design.Logic
{
    public class MainViewModel : ViewModelBase<IMainViewModel>, IMainViewModel
    {
        public ActionCommand OpenOptionsCmd => new ActionCommand(() => OptionsAreVisible = true);

        public MainViewModel()
        {
            PropertyChanged += OnPropertyChanged;

            SeasonExpanders = new ObservableCollection<ISeasonExpander>
            {
                new SeasonExpander(new ObservableCollection<ISeason>{new Season(), new Season() }, "test"),
                new SeasonExpander(new ObservableCollection<ISeason> { new Season(), new Season() }, "test")
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
