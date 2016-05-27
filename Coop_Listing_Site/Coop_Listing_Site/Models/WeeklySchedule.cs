using Coop_Listing_Site.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class WeeklySchedule
    {
        WeeklySchedule()
        {
            WorkDays = new HashSet<WorkDay>();
        }
        public ICollection<WorkDay> WorkDays { get; set; }
    }
}