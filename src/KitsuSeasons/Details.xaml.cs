using MahApps.Metro.Controls;
using System.Threading;
using System.Windows;

namespace KitsuSeasons
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : MetroWindow
    {
        private Mutex mutex = null;

        public Details()
        {
            InitializeComponent();
        }
    }
}
