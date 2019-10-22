using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Personal_Management
{
    public class Program
    {
        public static int colIsp = 0;
        public static string style = "bootstrap2.min.css";
        public static int id;
        public static bool step1, step2, step3, step4, step5, step6;
        public static int admin;
        public static SqlConnection SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PersonalContext"].ToString());
        public static int Admin = 0, Kadri = 0, Otdeli = 0, Buh = 0;
        public static void update()
        {
            try
            {
                SqlCommand command = new SqlCommand("select count(*) from Isp_Sroki where Status_ID = 3", Program.SqlConnection);
                Program.SqlConnection.Open();
                Program.colIsp = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "update Isp_Sroki set Status_ID = 3 where DATEDIFF(day, Convert(datetime, Date_Finish,104), GETDATE()) = 0 and Status_ID = 2";
                command.ExecuteScalar();
                Program.SqlConnection.Close();
            }
            catch
            {
                Program.SqlConnection.Close();
            }
        }
    }
}