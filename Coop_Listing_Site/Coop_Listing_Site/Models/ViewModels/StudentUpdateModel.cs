using Coop_Listing_Site.DAL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class StudentUpdateModel
    {
        public StudentUpdateModel() { }

        public StudentUpdateModel(StudentInfo studInfo)
        {
            LNumber = studInfo.LNumber;
            MajorID = studInfo.Major.MajorID;
            Email = studInfo.User.Email;

            if (studInfo.GPA == 0)
                GPA = null;
            else
                GPA = studInfo.GPA;
        }

        public string LNumber { get; set; }

        [Display(Name ="Current Major")]
        public int MajorID { get; set; }

        [Display(Name ="Current GPA")]
        public double? GPA { get; set; }

        public string Email { get; set; }          
    }
}