using System.ComponentModel.DataAnnotations;

namespace Coop_Listing_Site.Models
{
    public class Company : User
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }
    }
}
