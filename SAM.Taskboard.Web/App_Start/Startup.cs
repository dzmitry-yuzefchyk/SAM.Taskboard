using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Unity;
using Unity.Injection;

[assembly: OwinStartup(typeof(SAM.Taskboard.Web.App_Start.Startup))]

namespace SAM.Taskboard.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new UnityContainer();
            container.RegisterType<IDataProtectionProvider>(new InjectionFactory(c => app.GetDataProtectionProvider()));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}
