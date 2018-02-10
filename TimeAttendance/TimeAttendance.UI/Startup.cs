using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TimeAttendance.UI.Startup))]
namespace TimeAttendance.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
