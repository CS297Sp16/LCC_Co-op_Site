using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class Opportunity
    {
        public int OpportunityID { get; set; }
        public int PDF { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        [Display(Name = "Organization Name")]
        public string CompanyName { get; set; }  //Company name
        [Display(Name ="Contact Name")]
        public string ContactName { get; set; }  //Company contact name
        [Display(Name ="Contact Number")]
        public string ContactNumber { get; set; }  //Company contact phone number
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }  //Company contact email
        [Display(Name = "Worksite")]
        public string Location { get; set; }  //Location of coop
        [Display(Name = "Organization Website")]
        public string CompanyWebsite { get; set; }  //Company website
        [Display(Name = "Mission Statement")]
        public string AboutCompany { get; set; }  //Description of the company
        [Display(Name = "Department Description")]
        public string AboutDepartment { get; set; }  //Description of coop department
        [Display(Name = "Position Title")]
        public string CoopPositionTitle { get; set; }  //Title of coop
        [Display(Name = "Position Duties")]
        public string CoopPositionDuties { get; set; }  //Main duties involved with coop
        [Display(Name = "Requirements and Qualifications")]
        public string Qualifications { get; set; }  //Required experience/qualifications
        [Display(Name = "Minimum Required G.P.A.")]
        public double GPA { get; set; }  //Required minimum grade point average
        public bool Paid { get; set; }  //Paid, stippened, or unpaid coop
        [Display(Name = "Enter actual rate or salary")]
        public string Wage { get; set; } // to enter a wage if paid is selected
        [Display(Name = "Total Stipend Amount")]
        public string Amount { get; set; }// to enter amount if stippened is selected
        [Display(Name = "Position Duration")]
        public string Duration { get; set; }  //Duration of co-op
        [Display(Name = "Number of Openings")]
        public int OpeningsAvailable { get; set; } //Represents the quantity of available opportunities offered by the company
        [Display(Name = "Terms Available")]
        public string TermAvailable { get; set; }  //Datatype could also be DateTime and/or create an additional Term Class Entity        
        public int DepartmentID { get; set; } // the department whose students this opportunity is intended for
    }
}
