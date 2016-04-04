using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Company : User
    {
        // Inherits from User, doesn't need its own ID
        // public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }

    }
}