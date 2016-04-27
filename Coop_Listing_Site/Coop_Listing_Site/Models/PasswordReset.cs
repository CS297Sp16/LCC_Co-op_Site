using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class PasswordReset
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }


        public Dictionary<bool, string> SendResetEmail(EmailInfo emailInfo)
        {
            var retVal = new Dictionary<bool, string>();
            string fromEmail = string.Format("noreply@{0}", emailInfo.Domain);

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
                        string message = "You have received this e-mail in an attempt to reset your password. If you did initiate this action, please ignore this e-mail.{0}{0}" +
                                            "To reset your password, please click the link below. You will be taken to the password reset page. There you must enter a new password.{0}" +
                                            "https://{2}/Auth/ResetPassword/{1} {0}{0}" +
                                            "This is an automatic message. Any replies sent to this e-mail will not be viewed.";

                        mail.Subject = "Password Reset";
                        mail.Body = string.Format(message, "\r\n", Id, emailInfo.Domain);

                        client.Send(mail);
                    }
                }

                retVal[true] = "An email has been sent with instructions to reset your password. Please wait up to 5 minutes for it to arrive.";
            }
            catch
            {
                retVal[false] = "Failed to sent an e-mail. Please try again later. If the problem persists, get in contact with the site administrator.";
            }

            return retVal;
        }
    }
}