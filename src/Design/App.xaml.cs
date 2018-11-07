using Design.Interfaces;
using Design.Logic;
using System.Windows;

namespace Design
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IController controller = new Controller();
            IMainViewModel viewModel = new MainViewModel(controller);
            IMainView view = new MainView(viewModel);
            view.Show();
        }
    }
}