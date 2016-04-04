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
        //Still researching if Inheritance is going to conflict with this strategy
        public UserConfiguration()
        {
            ToTable("User");
        }
    }
}