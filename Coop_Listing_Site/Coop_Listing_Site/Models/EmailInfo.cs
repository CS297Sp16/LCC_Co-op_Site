using Coop_Listing_Site.DAL;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Data.Entity;

namespace Coop_Listing_Site.Models
{
    public class EmailInfo
    {
        public int EmailInfoID { get; set; }

        public string SMTPAccountName { get; set; }

        public string SMTPPassword { get; set; }

        public string SMTPAddress { get; set; }

        public string Domain { get; set; }

        public bool ProperlySet
        {
            get { return CheckIfInfoIsSet(); }
        }

        private bool CheckIfInfoIsSet()
        {
            return (!string.IsNullOrWhiteSpace(SMTPAccountName)
                    && !string.IsNullOrWhiteSpace(SMTPPassword)
                    && !string.IsNullOrWhiteSpace(SMTPAddress)
                    && !string.IsNullOrWhiteSpace(Domain));
        }

        public Dictionary<bool, string> SendInviteEmail(RegisterInvite invitation)
        {
            var retVal = new Dictionary<bool, string>();

            if (string.IsNullOrWhiteSpace(invitation.RegisterInviteID) || string.IsNullOrWhiteSpace(invitation.Email) || !invitation.UserTypeSet())
            {
                retVal[false] = "One or more fields for the invitation appear to be empty";
            }
            else if (!CheckIfInfoIsSet())
            {
                retVal[false] = "The email information appears to not be set correctly. Normally you would not be able to view this message. Contact the site administrator immediately.";
            }
            else
            {
                string fromEmail = string.Format("Invites@{0}", Domain);

                try
                {
                    using (var client = new SmtpClient(SMTPAddress, 587))
                    {
                        client.EnableSsl = true;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(SMTPAccountName, SMTPPassword);

                        using (var mail = new MailMessage(fromEmail, invitation.Email))
                        {
                            string message = "You have been invited to use the Lane Community College Co-op Listing website as a {1}.{0}" +
                                                "To register, please click the link below. You will be taken to the registration page, with your e-mail already filled out for you.{0}" +
                                                "https://{3}/Register/{1}/{2} {0}{0}" +
                                                "This is an automatic message. Any replies sent to this e-mail will not be viewed.";

                            mail.Subject = "Lane Community College Co-op Listing Invitiation";
                            mail.Body = string.Format(message, "\r\n", invitation.UserType.ToString(), invitation.RegisterInviteID, Domain);

                            client.Send(mail);
                        }
                    }

                    retVal[true] = "Invite successfully sent to " + invitation.Email;
                }
                catch
                {
                    retVal[false] = "Failed to send invite.";
                }
            }

            return retVal;
        }

        public Dictionary<bool, string> SendResetEmail(PasswordReset resetInfo)
        {
            var retVal = new Dictionary<bool, string>();

            if (!CheckIfInfoIsSet())
            {
                retVal[false] = "The email information appears to not be set correctly. Normally you would not be able to view this message. Contact the site administrator immediately.";
            }
            else
            {
                try
                {
                    string fromEmail = string.Format("noreply@{0}", Domain);
                    using (var client = new SmtpClient(SMTPAddress, 587))
                    {
                        client.EnableSsl = true;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(SMTPAccountName, SMTPPassword);

                        using (var mail = new MailMessage(fromEmail, resetInfo.Email))
                        {
                            string message = "You have received this e-mail in an attempt to reset your password. If you did not initiate this action, please ignore this e-mail.{0}{0}" +
                                                "To reset your password, please click the link below. You will be taken to the password reset page. There, you must enter a new password.{0}" +
                                                "https://{2}/Auth/ResetPassword/{1} {0}{0}" +
                                                "This is an automatic message. Any replies sent to this e-mail will not be viewed.";

                            mail.Subject = "Password Reset";
                            mail.Body = string.Format(message, "\r\n", resetInfo.Id, Domain);

                            client.Send(mail);
                        }
                    }

                    retVal[true] = "An email has been sent with instructions to reset your password. Please wait up to 5 minutes for it to arrive.";
                }
                catch
                {
                    retVal[false] = "Failed to sent an e-mail. Please try again later. If the problem persists, get in contact with the site administrator.";
                }
            }

            return retVal;
        }

        public void SendApplicationNotification(Application app)
        {
            if (CheckIfInfoIsSet())
            {
                StudentInfo student;
                CoordinatorInfo coord = null;

                using (var db = new CoopContext())
                {
                    db.Majors.Load();
                    db.Users.Load();

                    student = db.Students.FirstOrDefault(s => s.User.Id == app.User.Id);
                    foreach (var cInfo in db.Coordinators.ToList())
                    {
                        if(cInfo.Majors.Contains(student.Major))
                        {
                            coord = cInfo;
                            break;
                        }
                    }
                }

                try
                {
                    string fromEmail = string.Format("noreply@{0}", Domain);

                    using (var client = new SmtpClient(SMTPAddress, 587))
                    {
                        client.EnableSsl = true;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(SMTPAccountName, SMTPPassword);

                        using (var mail = new MailMessage(fromEmail, coord.User.Email))
                        {
                            string message = "{1} {2} has applied for the co-op opportunity {3} at {4}.{0}" +
                                                "Visit your control panel at the co-op listing site to review this application.{0}{0}" +
                                                "This is an automatic message. Any replies sent to this e-mail will not be viewed.";

                            mail.Subject = "New Co-op Application";
                            mail.Body = string.Format(message, "\r\n", app.User.FirstName, app.User.LastName, app.Opportunity.CoopPositionTitle, app.Opportunity.CompanyName);

                            client.Send(mail);
                        }
                    }
                }
                catch { }
            }
        }
    }
}