using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Department
    {
        public Department()
        {
            // make sure Majors isn't null when we make a new Department
            Majors = new List<Major>();
        }
        [Display(Name ="Department ID")]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Please enter a department name"), Display(Name = "Department name")]
        public string DepartmentName { get; set; }

        public virtual ICollection<Major> Majors { get; set; }
    }
}