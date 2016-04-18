﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class CoordinatorInfo
    {
        public CoordinatorInfo()
        {
            Departments = new List<Department>();
        }

        [Key]
        public string UserId { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}