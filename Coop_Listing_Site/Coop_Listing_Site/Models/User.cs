using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    /// <summary>
    /// Base user class. To be inherited by Student, CoopAdvisor, etc.
    /// </summary>
    public class User : IdentityUser
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public override string Email { get; set; }

        [Required, Phone, Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }
    }
}