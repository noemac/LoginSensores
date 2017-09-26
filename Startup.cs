using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sensoresapp.Startup))]
namespace sensoresapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
