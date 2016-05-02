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
        [Required(ErrorMessage ="Please enter a company name"), Display(Name ="Business Name")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Please enter a contact person's name"), Display(Name ="Contact Name")]
        public string ContactName { get; set; }
        [Phone, Required(ErrorMessage = "Please enter the contact person's phone number"), Display(Name ="Contact Phone Number")]
        public string ContactNumber { get; set; }
        [EmailAddress, Required(ErrorMessage = "Please enter an email address for the contact person"), Display(Name ="Contact Email Address")]         
        public string ContactEmail { get; set; }
        [Required(ErrorMessage = "Please enter the cooperative opportunity's place of employment"), Display(Name ="Worksite")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Please enter the web address of the company"), Display(Name ="Business Website Address")]
        public string CompanyWebsite { get; set; }
        [Required(ErrorMessage = "Please enter a brief company mission statement"), Display(Name ="Business Mission Statement")]
        public string AboutCompany { get; set; }
        [Display(Name ="Department Description")]
        public string AboutDepartment { get; set; }
        [Required(ErrorMessage = "Please enter a title for the cooperative opportunity"), Display(Name ="Position Title")]
        public string CoopPositionTitle { get; set; }
        [Required(ErrorMessage = "Please enter a description of the cooperative duties"), Display(Name ="Position Duties")]
        public string CoopPositionDuties { get; set; }
        [Required(ErrorMessage = "Please enter any required qualificatins, classes, and/or experience"), Display(Name ="Necessary Requirements and Qualifications")]
        public string Qualifications { get; set; }
        [Display(Name ="Minimum Required G.P.A.")]
        public double GPA { get; set; }
        [Required(ErrorMessage = "Please check box if the cooperative opportunity is going to be a paid position")]
        public bool Paid { get; set; }
        [Required(ErrorMessage = "Please enter the duration of the cooperative opportunity"), Display(Name ="Position Duration")]
        public string Duration { get; set; }
        [Required(ErrorMessage = "Please enter the number of openings available for this cooperative opportunity"), Display(Name ="Number of Fillable Positions")]
        public int OpeningsAvailable { get; set; }
        [Required(ErrorMessage = "Please enter the terms this cooperative opportunity will be available"), Display(Name ="Terms offered")]
        public string TermAvailable { get; set; }
        [Required(ErrorMessage = "Please enter the id of the major/department you would like this cooperative opportunity to be attached too")]
        public int DepartmentID { get; set; }
    }
}