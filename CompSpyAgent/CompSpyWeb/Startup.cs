using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CompSpyWeb.Startup))]
namespace CompSpyWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
