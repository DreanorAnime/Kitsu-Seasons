using KitsuSeasons.Interfaces;
using MahApps.Metro.Controls;
using System.ComponentModel;

namespace KitsuSeasons
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : MetroWindow
    {
        public Details(IDetailViewModel dataViewModel)
        {
            InitializeComponent();
            DataContext = dataViewModel;
        }

        public void Refresh(IDetailViewModel detailViewModel)
        {
            DataContext = detailViewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
