using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models.Enums
{
    public enum WorkCompensationProvider
    {       
        College,
        [Display(Name = "Work Site")]
        WorkSite,
        [Display(Name = "Work Study")]
        WorkStudy,
        [Display(Name = "No Coverage")]
        NoCoverage
    }
}