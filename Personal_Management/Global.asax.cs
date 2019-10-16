using System;
using System.Collections.Generic;
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
            SqlDependency sql = new SqlDependency();
            SqlCommand command = new SqlCommand("select count(*) from Isp_Sroki where DATEDIFF(day, Convert(datetime, Date_Finish,104), GETDATE())=0", Program.SqlConnection);
            Program.SqlConnection.Open();
            Program.colIsp = Convert.ToInt32(command.ExecuteScalar());
            command.CommandText = "update Isp_Sroki set Status_ID = 3 where DATEDIFF(day, Convert(datetime, Date_Finish,104), GETDATE()) > 0";
            command.ExecuteScalar();
            Program.SqlConnection.Close();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            FormsAuthentication.SignOut();
        }
    }
}
