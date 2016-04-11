using Coop_Listing_Site.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class DbInitializer : DropCreateDatabaseAlways<CoopContext>
    {
        protected override void Seed(CoopContext context)
        {
            base.Seed(context);
        }
    }
}