using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Personal_Management.Startup))]

namespace Personal_Management
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.UseSqlServer(ConfigurationManager.ConnectionStrings["PersonalContext"].ToString());
            app.MapSignalR();
        }
    }
}
