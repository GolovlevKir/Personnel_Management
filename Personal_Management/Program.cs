﻿using System;
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
        public static int id;
        public static int admin;
        public static SqlConnection SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PersonalContext"].ToString());
        public static void update()
        {
            SqlCommand command = new SqlCommand("select count(*) from Isp_Sroki where DATEDIFF(day, Convert(datetime, Date_Finish,104), GETDATE())=0", Program.SqlConnection);
            Program.SqlConnection.Open();
            Program.colIsp = Convert.ToInt32(command.ExecuteScalar());
            command.CommandText = "update Isp_Sroki set Status_ID = 3 where DATEDIFF(day, Convert(datetime, Date_Finish,104), GETDATE()) > 0";
            command.ExecuteScalar();
            Program.SqlConnection.Close();
        }
    }
}