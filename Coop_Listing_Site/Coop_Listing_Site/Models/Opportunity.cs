using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class Opportunity
    {
        public int OpportunityID { get; set; }
        public int PDF { get; set; }
        //[ForeignKey("User")]
        public int UserID { get; set; }
        //[ForeignKey("Company")]
        public int CompanyID { get; set; }
        public int OpeningsAvailable { get; set; } //Represents the quantity of available opportunities offered by the company
        public string TermAvailable { get; set; }  //Datatype could also be DateTime and/or create an additional Term Class Entity

        List<Opportunity> opportunities = new List<Opportunity>();
        public List<Opportunity> Opportunities
        {
            get
            {
                return opportunities;
            }
        }
    }
}