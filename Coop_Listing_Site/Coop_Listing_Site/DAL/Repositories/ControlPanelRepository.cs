using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;

namespace Coop_Listing_Site.DAL.Repositories
{
    public class ControlPanelRepository : IDisposable, IControlPanelRepository
    {
       //TODO: write the tests -LONNIE
        public ActionResult Index()
        {
            throw new NotImplementedException();
        }

        public ActionResult Invite()
        {
            throw new NotImplementedException();
        }

        public ActionResult Invite([Bind(Include = "Email,UserType")] RegisterInvite invitation)
        {
            throw new NotImplementedException();
        }

        public ActionResult SMTP()
        {
            throw new NotImplementedException();
        }

        public ActionResult SMTP(string SMTPAddress, string SMTPUser, string SMTPPassword, string InviteEmail, string Domain)
        {
            throw new NotImplementedException();
        }

        public ActionResult UpdateStudent()
        {
            throw new NotImplementedException();
        }

        public ActionResult UpdateStudent([Bind(Include = "UserId,GPA,MajorID,Password,ConfirmPassword")] StudentUpdateModel studentUpdateModel)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}