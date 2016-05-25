using System.Collections.Generic;

namespace Coop_Listing_Site.Models
{
    public class Department
    {
        public Department()
        {
            // make sure Majors isn't null when we make a new Department
            Majors = new List<Major>();
        }

        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public virtual ICollection<Major> Majors { get; set; }
    }
}