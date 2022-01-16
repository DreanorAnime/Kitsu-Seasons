using System;
using System.Threading.Tasks;
using KitsuSeasons.Interfaces;
using KitsuSeasons.Logic;
using KitsuSeasons.Models;
using System.Windows;
using System.Windows.Threading;

namespace KitsuSeasons
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            HandleSafeFail();
            
            IController controller = new Controller();
            IMainViewModel viewModel = new MainViewModel(controller);
            IMainView view = new MainView(viewModel);
            view.Show();
        }

        private void HandleSafeFail()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) => { MessageBox.Show(e.ExceptionObject.ToString());};

            DispatcherUnhandledException += (s, e) => { e.Handled = true; };

            TaskScheduler.UnobservedTaskException += (s, e) => { e.SetObserved(); };
        }
    }
}