using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Models
{
    public class EmailInfo
    {
        public int EmailInfoID { get; set; }

        public string SMTPAccountName { get; set; }

        public string SMTPPassword { get; set; }

        public string SMTPAddress { get; set; }

        public string Domain { get; set; }

        public bool ProperlySet
        {
            get { return CheckIfInfoIsSet(); }
        }

        private bool CheckIfInfoIsSet()
        {
            return (!string.IsNullOrWhiteSpace(SMTPAccountName)
                    && !string.IsNullOrWhiteSpace(SMTPPassword)
                    && !string.IsNullOrWhiteSpace(SMTPAddress)
                    && !string.IsNullOrWhiteSpace(Domain));
        }
    }
}