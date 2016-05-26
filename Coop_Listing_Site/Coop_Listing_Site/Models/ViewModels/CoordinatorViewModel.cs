using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class CoordinatorViewModel
    {
        public int CoordInfoID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Status")]
        public bool Enabled { get; set; }

        public ICollection<Major> Majors { get; set; }

        public CoordinatorViewModel(CoordinatorInfo cInfo)
        {
            CoordInfoID = cInfo.CoordInfoID;
            FirstName = cInfo.User.FirstName;
            LastName = cInfo.User.LastName;
            Email = cInfo.User.Email;
            Enabled = cInfo.User.Enabled;
            Majors = cInfo.Majors;
        }
    }
}