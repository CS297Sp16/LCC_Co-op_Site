using Coop_Listing_Site.Models;

namespace Coop_Listing_Site.Models
{
    public class Opportunity
    {
        public int OpportunityID { get; set; }

        public int PDF { get; set; }

        public int UserID { get; set; }

        public int CompanyID { get; set; }

        // the department whose students this opportunity is intended for
        public int DepartmentID { get; set; }

        //Represents the quantity of available opportunities offered by the company
        public int OpeningsAvailable { get; set; }

        //Datatype could also be DateTime and/or create an additional Term Class Entity
        public string TermAvailable { get; set; } 
    }
}