using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class User
    {
        //Placeholder ID until we start using Identity
        public int UserID { get; set; }
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}