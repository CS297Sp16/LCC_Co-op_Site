namespace Coop_Listing_Site.Models
{
    public class Major
    {
        public int MajorID { get; set; }

        public virtual Department Department { get; set; }

        public string MajorName { get; set; }
    }
}