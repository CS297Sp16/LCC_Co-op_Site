﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class StudentInfo
    {
        [Key]
        public string LNumber { get; set; }
        public string UserId { get; set; }
        public int MajorID { get; set; }
        public double GPA { get; set; }
        // public virtual ICollection<Course> CompletedCourses { get; set; }
        // public virtual ICollection<File> Files { get; set; }
    }
}