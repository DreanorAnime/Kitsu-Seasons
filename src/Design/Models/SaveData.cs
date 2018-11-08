namespace Design.Models
{
    public class SaveData
    {
        public SaveData()
        {
        }

        public SaveData(string emailAddress, string password)
        {
            EmailAddress = emailAddress;
            Password = password;
        }

        public string EmailAddress { get; set; }

        public string Password{ get; set; }
    }
}
