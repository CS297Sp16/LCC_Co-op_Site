using System.Collections.Generic;

namespace Coop_Listing_Site.Models
{
    public class Major
    {
        public int MajorID { get; set; }

        public int DepartmentID { get; set; }

        public string MajorName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}