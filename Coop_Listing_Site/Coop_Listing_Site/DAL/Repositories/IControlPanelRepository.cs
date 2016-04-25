using System.Web.Mvc;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;

namespace Coop_Listing_Site.DAL.Repositories
{
    public interface IControlPanelRepository
    {
        ActionResult Index();
        ActionResult Invite();
        ActionResult Invite([Bind(Include = "Email,UserType")] RegisterInvite invitation);
        ActionResult SMTP();
        ActionResult SMTP(string SMTPAddress, string SMTPUser, string SMTPPassword, string InviteEmail, string Domain);
        ActionResult UpdateStudent();
        ActionResult UpdateStudent([Bind(Include = "UserId,GPA,MajorID,Password,ConfirmPassword")] StudentUpdateModel studentUpdateModel);
    }
}