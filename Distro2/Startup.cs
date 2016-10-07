using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Distro2.Startup))]
namespace Distro2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
