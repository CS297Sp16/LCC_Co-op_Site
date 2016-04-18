using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site
{
    public static class EmailInfo
    {
        public static string SMTPAccountName { get; set; }

        public static string SMTPPassword { get; set; }

        public static string SMTPAddress { get; set; }

        public static string InviteEmail { get; set; }

        public static string Domain { get; set; }

        public static bool ProperlySet
        {
            get { return CheckInfo(); }
        }

        private static bool CheckInfo()
        {
            return (!string.IsNullOrWhiteSpace(SMTPAccountName)
                    && !string.IsNullOrWhiteSpace(SMTPPassword)
                    && !string.IsNullOrWhiteSpace(SMTPAddress)
                    && !string.IsNullOrWhiteSpace(InviteEmail)
                    && !string.IsNullOrWhiteSpace(Domain));
        }
    }
}