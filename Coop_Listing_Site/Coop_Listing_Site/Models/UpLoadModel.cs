using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class UpLoadModel
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}