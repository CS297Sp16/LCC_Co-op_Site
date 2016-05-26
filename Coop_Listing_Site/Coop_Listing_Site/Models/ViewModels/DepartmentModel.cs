using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class DepartmentModel
    {
        public DepartmentModel()
        {
            // make sure Majors isn't null when we make a new Department
            Majors = new List<Major>();
        }
        [Display(Name = "Department ID")]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Please enter a department name"), Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        public virtual ICollection<Major> Majors { get; set; }
    }
}