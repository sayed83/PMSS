using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PMSMVC.Startup))]
namespace PMSMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
