using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

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
                SqlCommand command = new SqlCommand("select count(*) from Isp_Sroki where Status_ID = 3", Program.SqlConnection);
                Program.SqlConnection.Open();
                Program.colIsp = Convert.ToInt32(command.ExecuteScalar());
                //Изменение тех что в процессе на ожидании
                command.CommandText = "update Isp_Sroki set Status_ID = 3 where DATEDIFF(day, Convert(datetime, Date_Finish,104), GETDATE()) = 0 and Status_ID = 2";
                command.ExecuteScalar();
                Program.SqlConnection.Close();
            }
            catch
            {
                Program.SqlConnection.Close();
            }
        }

        public static string Hash(string input)
        {
            SHA1Managed sha1 = new SHA1Managed();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Создать новый Stringbuilder для сбора байтов
            // и создаем строку.
            StringBuilder sb = new StringBuilder(hash.Length * 2);
            // Перебираем каждый байт хэшированных данных
            // и форматируем каждый как шестнадцатеричную строку.
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            // Возвращаем шестнадцатеричную строку.
            return sb.ToString();
        }
    }
}