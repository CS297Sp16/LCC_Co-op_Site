using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Application
    {
        public Application()
        {
            Files = new List<UserFile>();
        }

        public int ApplicationId { get; set; }

        public virtual User User { get; set; }

        public virtual Opportunity Opportunity { get; set; }
        //This is where the resumes will be stored in the DB     
        public byte[] Resume { get; set; }
        //This is where the cover letters will be stored in the DB
        public byte[] CoverLetter { get; set; }
        //This is where the Drivers License will be stored in the DB
        public byte[] DriverLicense { get; set; }
        //This is where any other information that you deem nessicary will be held in the DB
        public byte[] Other { get; set; }

        public virtual StudentInfo Student { get; set; }

        public virtual Opportunity Opportunity { get; set; }

        [Required]
        public string Message { get; set; }

        public virtual ICollection<UserFile> Files { get; set; }
    }
}