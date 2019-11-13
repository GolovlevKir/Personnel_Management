using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Personal_Management
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SqlDependency.Start(ConfigurationManager.ConnectionStrings["PersonalContext"].ToString());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        protected void Application_End()
        {
            SqlDependency.Stop(ConfigurationManager.ConnectionStrings["PersonalContext"].ToString());
            FormsAuthentication.SignOut();
        }
    }
}
