using System;
using System.Globalization;
using System.Net.Mail;

namespace KitsuSeasons.Logic
{
    public static class Validator
    {
        public static bool EmailAddressIsValid(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return true;
            }

            try
            {
                MailAddress mail = new MailAddress(emailAddress);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
