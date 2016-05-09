using System;
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
            Majors = new List<Major>();
        }

        [Key]
        public int CoordInfoID { get; set; }
        
        // The user this information is linked to
        public User User { get; set; }

        // The majors the coordinator oversees
        public virtual ICollection<Major> Majors { get; set; }
    }
}