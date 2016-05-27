using Coop_Listing_Site.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class WorkDay
    {
        public int WorkDayID { get; set; }
        public WeekDay WeekDay { get; set; }
        public TimeSpan Arrival { get; set; }
        public TimeSpan Departure { get; set; }
    }
}