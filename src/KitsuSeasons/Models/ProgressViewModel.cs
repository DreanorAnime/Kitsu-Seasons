using KitsuSeasons.Interfaces;
using ModelViewViewModel.Base;

namespace KitsuSeasons.Models
{
    public class ProgressViewModel : ViewModelBase<IProgressViewModel>, IProgressViewModel
    {
        public ProgressViewModel()
        {
            ProgressMaximum = 100;
        }

        public int ProgressValue
        {
            get { return Get(x => x.ProgressValue); }
            set { Set(x => x.ProgressValue, value); }
        }

        public int ProgressMaximum
        {
            get { return Get(x => x.ProgressMaximum); }
            set { Set(x => x.ProgressMaximum, value); }
        }

        public string ProgressText
        {
            get { return Get(x => x.ProgressText); }
            set { Set(x => x.ProgressText, value); }
        }

        public bool ProgressIsVisible
        {
            get { return Get(x => x.ProgressIsVisible); }
            set { Set(x => x.ProgressIsVisible, value); }
        }

        public bool ProgressIsIndeterminate
        {
            get { return Get(x => x.ProgressIsIndeterminate); }
            set { Set(x => x.ProgressIsIndeterminate, value); }
        }

        public void ResetValues()
        {
            ProgressIsIndeterminate = true;
            ProgressIsVisible = true;
            ProgressMaximum = 100;
            ProgressValue = 0;
        }
    }
}
