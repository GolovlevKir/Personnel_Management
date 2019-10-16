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

                Program.SqlConnection.Close();
            }
            else
            {
                RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Sotrs sotrs, HttpPostedFileBase imgfile)
        {
            string path = uploadimage(imgfile);
            Response.Write("<script>alert('" + path + "'); </script>");
            if (path.Equals("-1"))
            {

            }
            else
            {
                SqlCommand command = new SqlCommand("", Program.SqlConnection);

                Program.SqlConnection.Open();
                command.CommandText = "SELECT ID_Sotr FROM dbo.Accounts JOIN dbo.Sotrs ON dbo.Accounts.Sotr_ID = dbo.Sotrs.ID_Sotr where [dbo].[Accounts].[Login] = '" + User.Identity.Name + "'";
                int id = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "update Sotrs " +
                    "set " +
                    "Photo = '" + path + "' " +
                    "where ID_Sotr = " + id.ToString();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
                //var model = db.Sotrs.Find(sotrs.Positions_ID);
                //string path = uploadimage(imgfile);
                //sotrs.Photo = path;
                //db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(sotrs);
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
                    }
                }
                else
                {
                    Response.Write("<script>alert('Старый пароль введен неверно!'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Заполните поля паролей!'); </script>");
            }
            return View("Index");
        }

        public string uploadimage(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/Photo/Sotrs/"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }
            return path;
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