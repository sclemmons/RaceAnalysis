using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RaceAnalysis.Startup))]
namespace RaceAnalysis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
