namespace KitsuSeasons.Logic
{
    public static class MessageBoxSystem
    {
        public delegate void MessageBoxTextEventHandler(string title, string message);

        public static event MessageBoxTextEventHandler ShowMessageBox;

        public static void OnShowMessageBox(string title, string message)
        {
            ShowMessageBox?.Invoke(title, message);
        }
    }
}