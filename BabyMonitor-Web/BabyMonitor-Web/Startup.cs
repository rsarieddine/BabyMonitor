using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BabyMonitor_Web.Startup))]
namespace BabyMonitor_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
