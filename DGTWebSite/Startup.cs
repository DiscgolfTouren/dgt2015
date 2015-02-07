using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DGTWebSite.Startup))]
namespace DGTWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
