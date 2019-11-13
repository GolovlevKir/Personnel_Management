using System;
using System.Data;
using System.Data.SqlClient;

namespace Personal_Management
{
    public class DataBaseTables
    {
        public SqlCommand command = new SqlCommand("", Program.SqlConnection);
        public event Action<DataTable> dtFillFull;
        public SqlDependency dependency = new SqlDependency();

        public DataTable dtDepartments = new DataTable("Departments");
        public DataTable dtPositions = new DataTable("Positions");

        public string qrDepartments = "Select Naim_Depart from Departments";
        public string qrPositions = "Select Naim_Posit, Salary from Positions join Departments on ID_Depart = Depart_ID";

        private void dtFill(DataTable table, string query)
        {
            try
            {
                command.Notification = null;
                //в команду записываем запрос
                command.CommandText = query;
                //Открытие подключения
                Program.SqlConnection.Open();
                //Выполнение команды
                //Прослушивание
                //SqlDependency.Start(Program.SqlConnection.ConnectionString);

                table.Load(command.ExecuteReader());
            }
            catch 
            {
                
            }
            finally
            {
                //Закрытие подключения
                Program.SqlConnection.Close();
            }
        }

        public void dtPositFill()
        {
            //Вызов метода с указаниев таблицы и запроса
            dtFill(dtPositions, qrPositions);
        }

        public void dtDepFill()
        {
            //Вызов метода с указаниев таблицы и запроса
            dtFill(dtDepartments, qrDepartments);
        }


    }
}