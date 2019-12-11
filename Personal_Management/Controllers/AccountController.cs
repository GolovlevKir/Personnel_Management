using Personal_Management.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Personal_Management.Controllers
{
    public class AccountController : Controller
    {
        PersonalContext db = new PersonalContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Accounts user = null;
                string pa = Program.Hash(model.Password);
                //Поиск аккаунта по логину и паролю
                user = db.Accounts.FirstOrDefault(u => u.Login == model.Login && u.Password == pa);
                //Если найден, то открыть домашнюю страницу
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    Program.update();
                    return RedirectToAction("Index", "Home");
                }
                //Иначе вывести надпись, что пользователь отсутствует
                else
                {
                    ModelState.AddModelError("", "Пользователя с данным логином и паролем не существует");
                }
            }
            return View(model);
        }
        public ActionResult Register()
        {
            //Список сотрудников
            SelectList sot = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            ViewBag.Sotrs = sot;
            //Список ролей
            SelectList rol = new SelectList(db.Roles, "ID_Role", "Role_Naim");
            ViewBag.Roles = rol;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Accounts user = null;
                //Поиск пльзователей с таким логином
                user = db.Accounts.FirstOrDefault(u => u.Login == model.Login);
                //Если не найдено, то добавляем нового
                if (user == null)
                {
                    db.Accounts.Add(new Accounts { Login = model.Login, Password = model.Password, Role_ID = model.Role_ID, Sotr_ID = model.Sotr_ID });
                    db.SaveChanges();
                    user = db.Accounts.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    //Открываем окно аккаунта
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);

                        return RedirectToAction("Index", "Home");
                    }
                }
                //Иначе выводим надпись, что пользователь с таким логином уже существует
                else
                {
                    ModelState.AddModelError("", "Пользователя с данным логином и паролем не существует");
                }
            }

            return View(model);
        }

        public ActionResult LogOut()
        {
            //Выход из аккаунта
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session["admin"] = 0;
            Session["Kadri"] = 0;
            Session["Otdeli"] = 0;
            Session["Buh"] = 0;
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Them()
        {
            //Смена стилей
            if (Session["style"] == null)
            {
                Session["style"] = "bootstrap.min.css";
            }
            else
            {
                if (Session["style"].ToString() == "bootstrap.min.css")
                {
                    Session["style"] = "bootstrap2.min.css";
                }
                else
                {
                    Session["style"] = "bootstrap.min.css";
                }

            }
            return RedirectToAction("Index", "Home");
        }

    }
}