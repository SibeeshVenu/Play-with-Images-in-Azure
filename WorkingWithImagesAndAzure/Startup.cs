using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorkingWithImagesAndAzure.Startup))]
namespace WorkingWithImagesAndAzure
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
