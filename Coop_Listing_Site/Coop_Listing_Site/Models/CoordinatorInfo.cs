using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class CoordinatorInfo
    {
        //public string LNumber { get; set; } // Ignoring for now
        [Key]
        public string UserId { get; set; }
        public int DepartmentID { get; set; }
    }
}