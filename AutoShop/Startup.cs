using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoShop.Startup))]
namespace AutoShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
