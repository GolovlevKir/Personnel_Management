﻿using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Personal_Management
{
    public class Program
    {
        public static int colIsp = 0;
        public static int id;
        public static bool step1, step2, step3, step4, step5, step6;
        public static int admin;
        public static SqlConnection SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PersonalContext"].ToString());
        public static int Admin = 0, Kadri = 0, Otdeli = 0, Buh = 0;
        public static void update()
        {
            try
            {
                //Поиск испытательных сроков в ожидании
                SqlCommand command = new SqlCommand("select count(*) from Isp_Sroki join dbo.Posit_Responsibilities on ID_Pos_Res = Pos_Res_ID join dbo.Sotrs on Sotr_ID = ID_Sotr where Status_ID = 3 and fired = 'false'", SqlConnection);
                SqlConnection.Open();
                colIsp = Convert.ToInt32(command.ExecuteScalar());
                //Изменение тех что в процессе на ожидании
                command.CommandText = "update Isp_Sroki set Status_ID = 3 where DATEDIFF(day, Convert(datetime, Date_Finish,104), GETDATE()) >= 0 and Status_ID = 2";
                command.ExecuteNonQuery();
                SqlConnection.Close();
            }
            catch
            {
                Program.SqlConnection.Close();
            }
        }

    }
}