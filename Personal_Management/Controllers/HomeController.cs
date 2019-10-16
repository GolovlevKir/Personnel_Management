using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Response.Write("<script>alert('Добро пожаловать!'); </script>");
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.log = "Ваш логин: " + User.Identity.Name;
            }
            else
            {
                RedirectToAction("Login", "Account");
            }
            return View();
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