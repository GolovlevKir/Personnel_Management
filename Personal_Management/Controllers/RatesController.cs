using Personal_Management.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class RatesController : Controller
    {
        private PersonalContext db = new PersonalContext();

        [Authorize]
        public ActionResult Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Проверка на испытательные сроки
                Program.update();
                return View(db.Rates.ToList());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Rate,Rate")] Rates rates)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //если валидайия пройдена успешно
                if (ModelState.IsValid)
                {
                    //Добавление данных
                    db.Rates.Add(rates);
                    //Сохранение
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(rates);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //поиск по ключу
                Rates rates = db.Rates.Find(id);
                if (rates == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(rates);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Rate,Rate")] Rates rates)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(rates).State = EntityState.Modified;
                    //Сохранение
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(rates);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Rates rates = db.Rates.Find(id);
                if (rates == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(rates);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Удаление данных
                Rates rates = db.Rates.Find(id);
                db.Rates.Remove(rates);
                //Сохранение
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        //Очистка мусора
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
