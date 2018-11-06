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

            SeasonNotInList = new ObservableCollection<ISeason> { new Season(), new Season() };
        }

        public bool OptionsAreVisible
        {
            get { return Get(x => x.OptionsAreVisible); }
            set { Set(x => x.OptionsAreVisible, value); }
        }

        public ObservableCollection<ISeason> SeasonNotInList
        {
            get { return Get(x => x.SeasonNotInList); }
            private set { Set(x => x.SeasonNotInList, value); }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
