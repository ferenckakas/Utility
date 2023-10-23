using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SpotifyWebAPI.MVC.Startup))]
namespace SpotifyWebAPI.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
