using Design.Interfaces;
using Design.Logic;
using Design.Models;
using MahApps.Metro.Controls;
using System.IO;

namespace Design
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : MetroWindow, IMainView
    {
        public MainView(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Passwordbox.LostFocus += Passwordbox_LostFocus;
            Loaded += MainView_Loaded;
            DataStructure.SetupFolders();
        }

        private void MainView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Passwordbox.Password = new AES().Decrypt(DataStructure.Load().Password);
        }

        private void Passwordbox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            //Has to be this way, since we don't want to bind the password thus exposing it
            DataStructure.Save(new SaveData(string.Empty, new AES().Encrypt(Passwordbox.Password)));
        }
    }
}
