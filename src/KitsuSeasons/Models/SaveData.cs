namespace KitsuSeasons.Models
{
    public class SaveData
    {
        public SaveData()
        {
        }

        public SaveData(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }

        public string Password{ get; set; }
    }
}
