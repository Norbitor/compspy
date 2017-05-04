using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(CompSpyWeb.Startup))]

namespace CompSpyWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}