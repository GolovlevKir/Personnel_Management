using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class ErrorController : Controller
    {
        //Страница не найдена
        public ActionResult NotFound()
        {
            return View();
        }

        //Отсутствуют права доступа
        public ActionResult NotRight()
        {
            return View();
        }

        //Пользователь не авторизован
        public ActionResult NotAuth()
        {
            return View();
        }

        //Неверный запрос
        public ActionResult NotZapr()
        {
            return View();
        }

        //Ошибка сервера
        public ActionResult NotSer()
        {
            return View();
        }
    }
}
