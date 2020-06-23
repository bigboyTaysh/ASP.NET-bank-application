using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BankApplication.Startup))]
namespace BankApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
