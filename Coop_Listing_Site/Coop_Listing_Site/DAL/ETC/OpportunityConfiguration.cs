using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.DAL.ETC
{
    public class OpportunityConfiguration : EntityTypeConfiguration<Opportunity>
    {
        public OpportunityConfiguration()
        {
            ToTable("Opportunity");
        }
    }
}