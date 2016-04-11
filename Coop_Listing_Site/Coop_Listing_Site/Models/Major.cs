
using System.Collections.Generic;

namespace Coop_Listing_Site.Models
{
    public class Major
    {
        public int MajorID { get; set; }
        public int DepartmentID { get; set; }
        public string MajorName { get; set; }       //Required was set in the configuration class

        //Will re-implement later on. Will be a feature to add towards the end after everything else is working the 
        //way we want it to.
        //public virtual ICollection<Course> Courses { get; set; }

    }
}