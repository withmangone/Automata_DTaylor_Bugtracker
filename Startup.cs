using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Automata_DTaylor_Bugtracker.Startup))]
namespace Automata_DTaylor_Bugtracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
