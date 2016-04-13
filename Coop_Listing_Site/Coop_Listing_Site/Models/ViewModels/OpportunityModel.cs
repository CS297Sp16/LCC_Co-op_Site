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
        [Required, Display(Name ="Business Name")]
        public string CompanyName { get; set; }
        [Required, Display(Name ="Contact Name")]
        public string ContactName { get; set; }
        [Phone, Required, Display(Name ="Contact Phone Number")]
        public string ContactNumber { get; set; }
        [EmailAddress, Required, Display(Name ="Contact Email Address")]         
        public string ContactEmail { get; set; }
        [Required, Display(Name ="Worksite")]
        public string Location { get; set; }
        [Required, Display(Name ="Business Website Address")]
        public string CompanyWebsite { get; set; }
        [Required, Display(Name ="Business Mission Statement")]
        public string AboutCompany { get; set; }
        [Required, Display(Name ="Department Description")]
        public string AboutDepartment { get; set; }
        [Required, Display(Name ="Position Title")]
        public string CoopPositionTitle { get; set; }
        [Required, Display(Name ="Position Duties")]
        public string CoopPositionDuties { get; set; }
        [Required, Display(Name ="Necessary Requirements and Qualifications")]
        public string Qualifications { get; set; }
        [Display(Name ="Minimum Required G.P.A.")]
        public double GPA { get; set; }
        [Required]
        public bool Paid { get; set; }
        [Required, Display(Name ="Position Duration")]
        public string Duration { get; set; }
        [Required, Display(Name ="Number of Fillable Positions")]
        public int OpeningsAvailable { get; set; }
        [Required, Display(Name ="Terms offered")]
        public string TermAvailable { get; set; }
    }
}