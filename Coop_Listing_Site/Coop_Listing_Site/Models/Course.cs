using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public virtual ICollection<Course> Prerequsites { get; set; }
    }
}