using System.Collections.Generic;

namespace Coop_Listing_Site.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public virtual ICollection<Major> Majors { get; set; }
    }
}