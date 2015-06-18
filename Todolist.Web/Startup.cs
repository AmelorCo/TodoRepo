using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Todolist.Web.Startup))]
namespace Todolist.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
