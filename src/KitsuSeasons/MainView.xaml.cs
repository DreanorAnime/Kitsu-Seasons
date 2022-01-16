using KitsuSeasons.Interfaces;
using KitsuSeasons.Logic;
using KitsuSeasons.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace KitsuSeasons
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : IMainView
    {
        private const string DummyPassword = "******";

        public MainView(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Passwordbox.LostFocus += Passwordbox_LostFocus;
            Loaded += MainView_Loaded;
            DataStructure.SetupFolders();
            
            MessageBoxSystem.ShowMessageBox += async (title, message) => await this.ShowMessageAsync(title, message);
        }

        private void MainView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DataStructure.Load().Password))
            {
                Passwordbox.Password = DummyPassword;
            }
        }

        private void Passwordbox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            //Has to be this way, since we don't want to bind the password thus exposing it
            if (Passwordbox.Password != DummyPassword)
            {
                DataStructure.Save(new SaveData(string.Empty, new AES().Encrypt(Passwordbox.Password)));
            }
        }
    }
}
