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
        public string CompanyName { get; set; }  //Company name
        public string ContactName { get; set; }  //Company contact name
        public string ContactNumber { get; set; }  //Company contact phone number
        public string ContactEmail { get; set; }  //Company contact email
        public string Location { get; set; }  //Location of coop
        public string CompanyWebsite { get; set; }  //Company website
        public string AboutCompany { get; set; }  //Description of the company
        public string AboutDepartment { get; set; }  //Description of coop department
        public string CoopPositionTitle { get; set; }  //Title of coop
        public string CoopPositionDuties { get; set; }  //Main duties involved with coop
        public string Qualifications { get; set; }  //Required experience/qualifications
        public double GPA { get; set; }  //Required minimum grade point average
        public bool Paid { get; set; }  //Paid, stippened, or unpaid coop
        public string Wage { get; set; } // to enter a wage if paid is selected
        public string Amount { get; set; }// to enter amount if stippened is selected
        public string Duration { get; set; }  //Duration of co-op
        public int OpeningsAvailable { get; set; } //Represents the quantity of available opportunities offered by the company
        public string TermAvailable { get; set; }  //Datatype could also be DateTime and/or create an additional Term Class Entity        
        public int DepartmentID { get; set; } // the department whose students this opportunity is intended for
    }
}
