using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //This is where the Drivers License will be stored in the DB
        public byte[] DriverLicense { get; set; }
        //This is where any other information that you deem nessicary will be held in the DB
        public byte[] Other { get; set; }
        [Required]
        public string Message { get; set; }
        public string FileName_Resume { get; set; }
        public string FileName_CoverLetter { get; set; }
        public string FileName_DriverLicense { get; set; }
        public string FileName_Other { get; set; }
        public string Resume_ContentType { get; set; }
        public string CoverLetter_ContentType { get; set; }
        public string DriverLicense_ContentType { get; set; }
        public string Other_ContentType { get; set; }
        HttpPostedFileBase upload { get; set; }
    }
}