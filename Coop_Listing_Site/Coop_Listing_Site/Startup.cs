using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;
using System;
using Microsoft.AspNet.Identity;

namespace Coop_Listing_Site
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cookie for now. Switch to Session auth only. We don't want user to be logged in automatically when visiting the site by using a Cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                SlidingExpiration = true, // Recreate the cookie if a new request is made, and half of the previous cookie's expiration time has passed
                ExpireTimeSpan = TimeSpan.FromHours(2), // Have the cookie expire after two hours
                LoginPath = new PathString("/Login") // Tentative, depends on how authorization is set up
            });
        }
    }
}