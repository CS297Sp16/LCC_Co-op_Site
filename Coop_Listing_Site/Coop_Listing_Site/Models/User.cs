using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    /// <summary>
    /// Base user class. Referenced by info classes and used for login, register, etc.
    /// </summary>
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string Email { get; set; }
        public bool Enabled { get; set; }

        /*
         * Ignoring for now
        [Required, Phone, Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }
        */
    }
}