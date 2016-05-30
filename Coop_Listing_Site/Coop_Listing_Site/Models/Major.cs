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

        // The department the major belongs to
        public virtual Department Department { get; set; }

        // The name of the major
        public string MajorName { get; set; }

        // The opportunities that are listed under this major. Required for Many to Many relationship
        public virtual ICollection<Opportunity> Opportunities { get; set; }

        // The Coordinator that oversees the major
        public virtual CoordinatorInfo Coordinator { get; set; }
    }
}