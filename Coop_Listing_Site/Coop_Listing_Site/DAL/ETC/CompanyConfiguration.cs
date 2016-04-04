using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.DAL.ETC
{
    public class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            ToTable("Company");
            Property(x => x.CompanyName)
               .IsRequired();
            Property(x => x.City)
                .IsRequired();
            Property(x => x.State)
                .IsRequired();


        }
    }
}