

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Major
    {
        public Major()
        {
            Opportunities = new List<Opportunity>();
        }

        public int MajorID { get; set; }

        public virtual Department Department { get; set; }

        public string MajorName { get; set; }

        public virtual ICollection<Opportunity> Opportunities { get; set; }
    }
}