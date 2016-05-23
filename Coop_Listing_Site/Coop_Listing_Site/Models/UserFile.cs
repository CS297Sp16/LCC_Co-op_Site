using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class UserFile
    {
        public int UserFileID { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public byte[] FileData { get; set; }
    }
}