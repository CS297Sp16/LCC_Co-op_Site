
using System.Collections.Generic;

namespace Coop_Listing_Site.Models
{
    public class Major
    {
        public int MajorID { get; set; }
        public string Name { get; set; }       //Required was set in the configuration class
        public virtual ICollection<Course> Courses { get; set; }

    }
}