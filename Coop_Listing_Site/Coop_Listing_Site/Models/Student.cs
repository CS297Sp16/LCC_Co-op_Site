namespace Coop_Listing_Site.Models
{
    public class Student : User
    {
        public string LNumber { get; set; }
		
        public int MajorID { get; set; }
    }
}