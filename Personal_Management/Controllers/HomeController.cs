using Personal_Management.Models;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Program.update();
            //Response.Write("<script>alert('Добро пожаловать!'); </script>");
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.log = "Ваш логин: " + User.Identity.Name;
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                Program.SqlConnection.Open();
                command.CommandText = "SELECT dbo.Sotrs.Surname_Sot + ' ' + dbo.Sotrs.Name_Sot + ' ' + dbo.Sotrs.Petronumic_Sot FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                ViewBag.FIO = command.ExecuteScalar().ToString();
                command.CommandText = "SELECT Num_Phone FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                ViewBag.Phone = command.ExecuteScalar().ToString();
                command.CommandText = "SELECT Email FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                ViewBag.Em = command.ExecuteScalar().ToString();
                command.CommandText = "SELECT dbo.Positions.Naim_Posit FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Positions ON dbo.Sotrs.Positions_ID = dbo.Positions.ID_Positions where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                ViewBag.Pos = command.ExecuteScalar().ToString();
                command.CommandText = "SELECT Opisanie FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                ViewBag.Opis = command.ExecuteScalar().ToString();
                command.CommandText = "SELECT Photo FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                ViewBag.photo = command.ExecuteScalar().ToString();
                command.CommandText = "SELECT dbo.Roles.Manip_Roles FROM dbo.Accounts INNER JOIN dbo.Roles ON dbo.Accounts.Role_ID = dbo.Roles.ID_Role where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.admin = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "SELECT dbo.Roles.Manip_Sotrs FROM dbo.Accounts INNER JOIN dbo.Roles ON dbo.Accounts.Role_ID = dbo.Roles.ID_Role where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.Kadri = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "SELECT dbo.Roles.Manip_Department FROM dbo.Accounts INNER JOIN dbo.Roles ON dbo.Accounts.Role_ID = dbo.Roles.ID_Role where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.Otdeli = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "SELECT dbo.Roles.Buh_Ych FROM dbo.Accounts INNER JOIN dbo.Roles ON dbo.Accounts.Role_ID = dbo.Roles.ID_Role where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.Buh = Convert.ToInt32(command.ExecuteScalar());
                
                command.CommandText = "SELECT dbo.Steps.AddSotrInIS FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.step1 = Convert.ToBoolean(command.ExecuteScalar());
                command.CommandText = "SELECT dbo.Steps.AddRezume FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.step2 = Convert.ToBoolean(command.ExecuteScalar());
                command.CommandText = "SELECT dbo.Steps.AddSobesedovanie FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.step3 = Convert.ToBoolean(command.ExecuteScalar());
                command.CommandText = "SELECT dbo.Steps.AddIspSrok FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.step4 = Convert.ToBoolean(command.ExecuteScalar());
                command.CommandText = "SELECT dbo.Steps.RezimOzidaniya FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.step5 = Convert.ToBoolean(command.ExecuteScalar());
                command.CommandText = "SELECT dbo.Steps.Reshenie FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                Program.step6 = Convert.ToBoolean(command.ExecuteScalar());

                Program.SqlConnection.Close();
            }
            else
            {
                RedirectToAction("Login", "Account");
            }
            return View();
        }

        
        [HttpPost]
        public ActionResult Index(string password, string password1, string password2)
        {
            SqlCommand command = new SqlCommand("", Program.SqlConnection);
            if (password != null || password1 != null || password2 != null)
            {
                command.CommandText = "SELECT count(*) FROM dbo.Accounts where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "' and [dbo].[Accounts].[Password] = '" + password + "'";
                Program.SqlConnection.Open();
                int co = Convert.ToInt32(command.ExecuteScalar());
                Program.SqlConnection.Close();
                if (co > 0)
                {
                    if (password1 == password2)
                    {
                        ViewBag.New = "";
                        ViewBag.NewPass = "";
                        ViewBag.OldPass = "";
                        command.CommandText = "update Accounts " +
                        "set " +
                        "Password = '" + password1 + "' " +
                        "where Login = '" + User.Identity.Name + "'";
                        Program.SqlConnection.Open();
                        command.ExecuteScalar();
                        Program.SqlConnection.Close();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Response.Write("<script>alert('Введенные новые пароли не совпадают!'); </script>");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    Response.Write("<script>alert('Старый пароль введен неверно!'); </script>");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Response.Write("<script>alert('Заполните поля паролей!'); </script>");
                return RedirectToAction("Index");
            }
        }

        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //[Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}