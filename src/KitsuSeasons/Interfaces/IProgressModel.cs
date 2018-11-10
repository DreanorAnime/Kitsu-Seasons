using System.ComponentModel;

namespace KitsuSeasons.Interfaces
{
    public interface IProgressModel : INotifyPropertyChanged
    {
        int ProgressValue { get; set; }

        int ProgressMaximum { get; set; }

        string ProgressText { get; set; }

        bool ProgressIsVisible { get; set; }

        bool ProgressIsIndeterminate { get; set; }
    }
}
