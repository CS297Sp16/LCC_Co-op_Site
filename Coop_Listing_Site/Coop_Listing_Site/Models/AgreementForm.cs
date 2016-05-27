using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop_Listing_Site.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class AgreementForm
    {
        public int AgreementFormID { get; set; }
        public Term Term { get; set; }
        public DateTime TodaysDate { get; set; }
        public string SubjectNumber { get; set; }
        public int CRN { get; set; }
        public string StudentName { get; set; }
        public string LNumber { get; set; }
        public Major Major { get; set; }
        public string StudentPhone { get; set; }
        public string StudentAddress { get; set; }
        public string StudentCity { get; set; }
        public string StudentState { get; set; }
        public int StudentZipCode { get; set; }
        public int? StudentZipCodeExtention { get; set; }
        public string StudentEmail { get; set; }
        public int Credits { get; set; }
        public int ClockHours { get; set; }
        public string CompanyName { get; set; }
        public string Supervisor { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public int CompanyZipCode { get; set; }
        public int? CompanyZipCodeExtention { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyFax { get; set; }
        public string WorkAssignmentDutyDescription { get; set; }
        public WeeklySchedule WeeklySchedule { get; set; }
        public decimal Wage { get; set; }  //how much money
        public string WageRate { get; set; } //per hour, day, lifetime, etc.
        public bool PaidPosition { get; set; } //paid or unpaid
        public WorkCompensationProvider WorkCompProv { get; set; }
        public CoordinatorInfo Coordinator { get; set; }
        public string SpecialNotes { get; set; }
        public string WorkSiteSupervisorSignature { get; set; }
        public string CoopCoordinatorSignature { get; set; }
        public string StudentSignature { get; set; }

    }
}