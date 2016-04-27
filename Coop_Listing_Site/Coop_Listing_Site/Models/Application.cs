using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public int OpportunityId { get; set; }
        //This is where the resumes will be stored in the DB     
        public byte[] Resume { get; set; }
        //This is where the cover letters will be stored in the DB
        public byte[] CoverLetter { get; set; }

        public string Message { get; set; }
        public string FileName_Resume { get; set; }
        public string FileName_CoverLetter { get; set; }
        HttpPostedFileBase upload { get; set; }
    }
}