using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class MajorViewModel
    {
        public MajorViewModel()
        {
            Opportunities = new List<Opportunity>();
        }

        public MajorViewModel(Major major)
        {
            MajorID = major.MajorID;
            MajorName = major.MajorName;
            Department = major.Department;
            Opportunities = major.Opportunities;
        }

        public int MajorID { get; set; }

        public Department Department { get; set; }

        [Required, Display(Name = "Major Name")]
        public string MajorName { get; set; }

        public ICollection<Opportunity> Opportunities { get; set; }

        public Major ToMajor()
        {
            var major = new Major();

            major.MajorID = MajorID;
            major.MajorName = MajorName;
            major.Department = Department;
            major.Opportunities = Opportunities;

            return major;
        }
    }
}