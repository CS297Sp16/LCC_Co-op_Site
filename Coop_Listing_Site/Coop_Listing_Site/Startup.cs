using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;

namespace Coop_Listing_Site
{
    public class Startup
    {
        //should this be a static method, I've seen it done both ways ??? -LONNIE  ???
        public void Configuration(IAppBuilder app)
        {
            // Cookie for now. Switch to Session auth only. We don't want user to be logged in automatically when visiting the site by using a Cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Login") // Tentative, depends on how authorization is set up
            });


        }
    }
}