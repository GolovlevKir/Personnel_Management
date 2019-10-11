using Personal_Management.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;

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
                user = db.Accounts.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с данным логином и паролем не существует");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            SelectList sot = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            ViewBag.Sotrs = sot;
            SelectList rol = new SelectList(db.Roles, "ID_Role", "Role_Naim");
            ViewBag.Roles = rol;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Accounts user = null;
                user = db.Accounts.FirstOrDefault(u => u.Login == model.Login);
                if (user == null)
                {
                    db.Accounts.Add(new Accounts { Login = model.Login, Password = model.Password, Role_ID = model.Role_ID, Sotr_ID = model.Sotr_ID });
                    db.SaveChanges();
                    user = db.Accounts.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с данным логином и паролем не существует");
                }
            }
            
            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}