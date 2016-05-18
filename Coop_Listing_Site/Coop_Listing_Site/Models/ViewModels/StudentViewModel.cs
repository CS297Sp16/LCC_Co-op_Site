using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class StudentViewModel
    {
        [Key]
        [Display(Name = "L-Number")]
        public string LNumber { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Major { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double GPA { get; set; }

        [Display(Name = "Status")]
        public bool Enabled { get; set; }

        public StudentViewModel(StudentInfo sInfo)
        {
            LNumber = sInfo.LNumber;
            FirstName = sInfo.User.FirstName;
            LastName = sInfo.User.LastName;
            Email = sInfo.User.Email;
            Major = sInfo.Major.MajorName;
            GPA = sInfo.GPA;
            Enabled = sInfo.User.Enabled;
        }
    }
}