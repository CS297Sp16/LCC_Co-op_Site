

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Major
    {
        public int MajorID { get; set; }

        public Department Department { get; set; }
        [Display(Name ="Major Name")]
        public string MajorName { get; set; }

        //Will re-implement later on. Will be a feature to add towards the end after everything else is working the 
        //way we want it to.
        //public virtual ICollection<Course> Courses { get; set; }
    }
}