namespace Coop_Listing_Site.Models
{
    public class Coordinator : User
    {
        public int CoordinatorID { get; set; }

        public string LNumber { get; set; }

        public int DepartmentID { get; set; }
    }
}