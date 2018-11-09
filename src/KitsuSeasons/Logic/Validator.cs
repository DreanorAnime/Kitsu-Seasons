using System;
using System.Net.Mail;

namespace Design.Logic
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
