using System;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                //Проверка на испытательные сроки
                Program.update();
                //Response.Write("<script>alert('Добро пожаловать!'); </script>");
                if (User.Identity.IsAuthenticated)
                {
                    //Получение значений личного кабинета
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
                    Session["admin"] = Convert.ToInt32(command.ExecuteScalar());
                    command.CommandText = "SELECT dbo.Roles.Manip_Sotrs FROM dbo.Accounts INNER JOIN dbo.Roles ON dbo.Accounts.Role_ID = dbo.Roles.ID_Role where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["Kadri"] = Convert.ToInt32(command.ExecuteScalar());
                    command.CommandText = "SELECT dbo.Roles.Manip_Department FROM dbo.Accounts INNER JOIN dbo.Roles ON dbo.Accounts.Role_ID = dbo.Roles.ID_Role where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["Otdeli"] = Convert.ToInt32(command.ExecuteScalar());
                    command.CommandText = "SELECT dbo.Roles.Buh_Ych FROM dbo.Accounts INNER JOIN dbo.Roles ON dbo.Accounts.Role_ID = dbo.Roles.ID_Role where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["Buh"] = Convert.ToInt32(command.ExecuteScalar());

                    command.CommandText = "SELECT dbo.Steps.AddSotrInIS FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["step1"] = Convert.ToBoolean(command.ExecuteScalar());
                    command.CommandText = "SELECT dbo.Steps.AddRezume FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["step2"] = Convert.ToBoolean(command.ExecuteScalar());
                    command.CommandText = "SELECT dbo.Steps.AddSobesedovanie FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["step3"] = Convert.ToBoolean(command.ExecuteScalar());
                    command.CommandText = "SELECT dbo.Steps.AddIspSrok FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["step4"] = Convert.ToBoolean(command.ExecuteScalar());
                    command.CommandText = "SELECT dbo.Steps.RezimOzidaniya FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["step5"] = Convert.ToBoolean(command.ExecuteScalar());
                    command.CommandText = "SELECT dbo.Steps.Reshenie FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Steps ON dbo.Sotrs.ID_Sotr = dbo.Steps.Sotr_ID where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                    Session["step6"] = Convert.ToBoolean(command.ExecuteScalar());

                    Program.SqlConnection.Close();
                }
                else
                {
                    RedirectToAction("Login", "Account");
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(string password, string password1, string password2)
        {
            SqlCommand command = new SqlCommand("", Program.SqlConnection);
            if (password != null || password1 != null || password2 != null)
            {
                //Изменение пароля
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
                        ModelState.AddModelError("", "Введенные новые пароли не совпадают!");
                        return View("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Старый пароль введен неверно!");
                    return View("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Заполните поля паролей!");
                return View("Index");
            }
        }

    }
}