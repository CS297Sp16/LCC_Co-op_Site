using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models.ViewModels
{
    public class CoordUpdateModel
    {
        public CoordUpdateModel()
        {
            Majors = new List<Major>();
        }

        public CoordUpdateModel(CoordinatorInfo coord)
        {
            CoordID = coord.CoordInfoID;
            Email = coord.User.Email;
            Majors = coord.Majors;
        }

        public int CoordID { get; set; }

        [Required]
        public string Email { get; set; }

        public ICollection<Major> Majors { get; set; }
    }
}