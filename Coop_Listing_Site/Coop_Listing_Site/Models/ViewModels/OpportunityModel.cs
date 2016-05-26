using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class OpportunityModel
    {
        public OpportunityModel()
        {
            Students = new List<StudentInfo>();
            Majors = new List<Major>();
        }

        public OpportunityModel(Opportunity opportunity)
        {
            OpportunityID = opportunity.OpportunityID;
            AboutCompany = opportunity.AboutCompany;
            AboutDepartment = opportunity.AboutDepartment;
            Amount = opportunity.Amount;
            Approved = opportunity.Approved;
            CompanyName = opportunity.CompanyName;
            CompanyWebsite = opportunity.CompanyWebsite;
            ContactEmail = opportunity.ContactEmail;
            ContactName = opportunity.ContactName;
            ContactNumber = opportunity.ContactNumber;
            CoopPositionDuties = opportunity.CoopPositionDuties;
            CoopPositionTitle = opportunity.CoopPositionTitle;
            Department = opportunity.Department;
            Duration = opportunity.Duration;
            GPA = opportunity.GPA;
            Location = opportunity.Location;
            Majors = opportunity.Majors;
            OpeningsAvailable = opportunity.OpeningsAvailable;
            Paid = opportunity.Paid;
            Qualifications = opportunity.Qualifications;
            Students = opportunity.Students;
            TermAvailable = opportunity.TermAvailable;
            Wage = opportunity.Wage;
        }

        public int OpportunityID { get; set; }

        [Required(ErrorMessage ="Please enter an organization name"), Display(Name ="Organization Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Please enter the name of a contact"), Display(Name ="Contact Name")]
        public string ContactName { get; set; }

        [Phone, Required(ErrorMessage = "Please enter the contact phone number"), Display(Name ="Contact Phone Number")]
        public string ContactNumber { get; set; }

        [EmailAddress, Required(ErrorMessage = "Please enter the contact e-mail address"), Display(Name ="Contact Email Address")]         
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Please enter the cooperative opportunity's place of employment"), Display(Name ="Worksite")]
        public string Location { get; set; }

        [Display(Name = "Organization Website")]
        public string CompanyWebsite { get; set; }

        [Display(Name = "Mission Statement")]
        public string AboutCompany { get; set; }

        [Display(Name ="Department Description")]
        public string AboutDepartment { get; set; }

        [Required(ErrorMessage = "Please enter a title for the cooperative opportunity"), Display(Name ="Position Title")]
        public string CoopPositionTitle { get; set; }

        [Required(ErrorMessage = "Please enter a description of the cooperative duties"), Display(Name ="Position Duties")]
        public string CoopPositionDuties { get; set; }

        [Required(ErrorMessage = "Please enter any required qualificatins, classes, and/or experience"), Display(Name ="Requirements and Qualifications")]
        public string Qualifications { get; set; }

        [Display(Name = "Minimum Required G.P.A.")]
        public double? GPA { get; set; }

        [Display(Name = "Paid")]
        public bool Paid { get; set; }

        [Display(Name = "Enter actual rate or salary")]
        public string Wage { get; set; }

        [Display(Name = "Total stipend amount")]
        public string Amount { get; set; }

        [Required(ErrorMessage = "Please enter the duration of the cooperative opportunity"), Display(Name ="Position Duration")]
        public string Duration { get; set; }

        [Display(Name ="Number of Openings")]
        public int OpeningsAvailable { get; set; }

        [Display(Name ="Terms Available")]
        public string TermAvailable { get; set; }

        public bool Approved { get; set; }

        public Department Department { get; set; } 

        public ICollection<StudentInfo> Students { get; set; }

        public ICollection<Major> Majors { get; set; }

        // Like ToString, but returns an Opportunity object instead
        public Opportunity ToOpportunity()
        {
            Opportunity opp = new Opportunity();
            opp.OpportunityID = OpportunityID;
            opp.AboutCompany = AboutCompany;
            opp.AboutDepartment = AboutDepartment;
            opp.Amount = Amount;
            opp.Approved = Approved;
            opp.CompanyName = CompanyName;
            opp.CompanyWebsite = CompanyWebsite;
            opp.ContactEmail = ContactEmail;
            opp.ContactName = ContactName;
            opp.ContactNumber = ContactNumber;
            opp.CoopPositionDuties = CoopPositionDuties;
            opp.CoopPositionTitle = CoopPositionTitle;
            opp.Department = Department;
            opp.Duration = Duration;
            opp.GPA = GPA;
            opp.Location = Location;
            opp.Majors = Majors;
            opp.OpeningsAvailable = OpeningsAvailable;
            opp.Paid = Paid;
            opp.Qualifications = Qualifications;
            opp.Students = Students;
            opp.TermAvailable = TermAvailable;
            opp.Wage = Wage;

            return opp;
        }
    }
}