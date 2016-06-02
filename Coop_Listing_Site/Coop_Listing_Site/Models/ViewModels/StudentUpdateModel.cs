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

        public string UserId { get; set; }
        [Display(Name ="Current Major")]
        public int MajorID { get; set; }
        [Display(Name ="Current GPA")]
        public double? GPA { get; set; }

        public StudentUpdateModel(StudentInfo studInfo)
        {
            Dictionary<double?, string> gpaList = new Dictionary<double?, string>();
            double gpaMin = 2.00d;
            double increment = 0.01d;
            double key;
            string value;
            double? gpaSelectedValue = null;

            this.UserId = studInfo.User.Id;
            this.MajorID = studInfo.Major.MajorID;
            this.GPA = gpaSelectedValue ?? studInfo.GPA; //Allows GET method to use studInfo.GPA and POST to use value returned by gpaSelectedValue

            gpaSelectedValue = this.GPA;

            gpaList.Add(0d, "N/A");  //first SelectListItem is the option "N/A"

            while (gpaMin <= 4.5)
            {
                value = gpaMin.ToString("N2");  //used to format the displayed value
                key = Convert.ToDouble(value);  //produces the values => key

                gpaList.Add(key, value);
                gpaMin += increment;
            }

            this.GPASelectList = new SelectList(gpaList, "key", "value", gpaSelectedValue);

        }

        /**************************************** VIEW SPECIFIC HELPER PROPERTIES *********************************************************/

        public SelectList GPASelectList { get; set; }
            
    }
}