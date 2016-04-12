using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class OpportunityModel
    {
        public int OpportunityID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        [Required, Display(Name ="Company Name")]
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }         
        public string ContactEmail { get; set; }
        public string Location { get; set; }
        public string CompanyWebsite { get; set; }
        public string AboutCompany { get; set; }
        public string AboutDepartment { get; set; }
        public string CoopPositionTitle { get; set; }
        public string CoopPositionDuties { get; set; }
        public string Qualifications { get; set; }
        public double GPA { get; set; }
        public bool Paid { get; set; }
        public string Duration { get; set; }
        public int OpeningsAvailable { get; set; }
        public string TermAvailable { get; set; }
    }
}