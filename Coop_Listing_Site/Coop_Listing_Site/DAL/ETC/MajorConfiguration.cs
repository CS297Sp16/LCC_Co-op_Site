using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.DAL.ETC
{

    //used in conjunction with the fluidAPI and the modelBuilder in the dbContext class
    //this allows us to keep our POCO's clean and remove the "most" of the data annotations 

    public class MajorConfiguration : EntityTypeConfiguration<Major>
    {
        public MajorConfiguration()
        {
            ToTable("Major");       //Explicitly Tell SQL Server the Table Name

            Property(x => x.MajorName);//this was causing an error on my machine when it was x.Name--name isn't in the major class

            Property(x => x.MajorName).IsRequired();
        }
    }
}