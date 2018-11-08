using Design.Interfaces;
using MahApps.Metro.Controls;

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
        }

        private void Passwordbox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            //Has to be this way, since we don't want to bind the password thus exposing it
            var a = Passwordbox;
            //encrypt password with AES and save to file
        }
    }
}
