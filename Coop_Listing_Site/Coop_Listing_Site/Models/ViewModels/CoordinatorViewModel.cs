using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class CoordinatorViewModel
    {
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

        public ICollection<Major> Majors { get; set; }

        public CoordinatorViewModel()
        {

        }
    }
}