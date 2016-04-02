using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;

namespace Coop_Listing_Site
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Login") // Might be changed later
            });
        }
    }
}