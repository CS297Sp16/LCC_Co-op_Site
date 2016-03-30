using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Major
    {
        public int MajorID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}