using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Coop_Listing_Site.Models
{
    public class RegisterInvite
    {
        // A GUID to be used for verifying the register link
        public string RegisterInviteID { get; set; }

        // The email the invite was sent to. Used to set the email field on the registration page.
        [Required, EmailAddress]
        public string Email { get; set; }

        // The account type, so someone with a Student invite can't register as a Coordinator, and vice versa
        [Required, Display(Name = "User Type")]
        public AccountType UserType { get; set; }

        // An enum of account types
        public enum AccountType
        {
            Student = 1,
            Coordinator = 2
        };

        public Dictionary<bool, string> SendInvite(EmailInfo emailInfo)
        {
            var retVal = new Dictionary<bool, string>();

            if (string.IsNullOrWhiteSpace(RegisterInviteID) || string.IsNullOrWhiteSpace(Email) || !UserTypeSet())
            {
                retVal[false] = "One or more fields for the invitation appear to be empty";
            }
            else
            {
                string fromEmail = string.Format("Invites@{0}", emailInfo.Domain);

                try
                {
                    using (var client = new SmtpClient(emailInfo.SMTPAddress, 587))
                    {
                        client.EnableSsl = true;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(emailInfo.SMTPAccountName, emailInfo.SMTPPassword);

                        using (var mail = new MailMessage(fromEmail, Email))
                        {
                            string message = "You have been invited to use the Lane Community College Co-op Listing website.{0}{0}" +
                                                "To register, please click the link below. You will be taken to the registration page, with your e-mail already filled out for you.{0}" +
                                                "https://{3}/Register/{1}/{2} {0}{0}" +
                                                "This is an automatic message. Any replies sent to this e-mail will not be viewed.";

                            mail.Subject = "Lane Community College Co-op Listing Invitiation";
                            mail.Body = string.Format(message, "\r\n", UserType.ToString(), RegisterInviteID, emailInfo.Domain);

                            client.Send(mail);
                        }
                    }

                    retVal[true] = "Invite successfully sent to " + Email;
                }
                catch
                {
                    retVal[false] = "Failed to send invite.";
                }
            }

            return retVal;
        }

        private bool UserTypeSet()
        {
            // Checks if the UserType is set to one of the available enum values in AccountType using bitwise operators
            return ((UserType & AccountType.Student) == AccountType.Student || (UserType & AccountType.Coordinator) == AccountType.Coordinator);
        }
    }
}