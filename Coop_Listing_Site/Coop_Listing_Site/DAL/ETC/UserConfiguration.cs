using Coop_Listing_Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.DAL.ETC
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        //used in conjunction with the fluidAPI and the modelBuilder in the dbContext class
        //this allows us to keep our POCO's clean and remove the "most" of the data annotations 
    }
}