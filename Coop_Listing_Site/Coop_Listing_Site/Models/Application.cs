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
        
        public virtual StudentInfo Student { get; set; }

        public virtual Opportunity Opportunity { get; set; }

        [Required]
        public string Message { get; set; }

        public virtual ICollection<UserFile> Files { get; set; }
    }
}