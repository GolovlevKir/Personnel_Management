using Personal_Management.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class ObrabotkasController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //Получение списка собеседований соискателей
                var obrabotka = db.Obrabotka.Include(o => o.Sotrs).Where(o => o.Sotrs.Guest == true);
                return View(await obrabotka.ToListAsync());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public async Task<ActionResult> Details(int? id)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Obrabotka obrabotka = await db.Obrabotka.FindAsync(id);
                if (obrabotka == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(obrabotka);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Obrabotka obrabotka = await db.Obrabotka.FindAsync(id);
                if (obrabotka == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                //Соискатели
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", obrabotka.Sotr_ID);
                return View(obrabotka);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Obr,Sotr_ID,Data_Sob,Vremya_Sob,Kommnt,reshenie,nomerzay")] Obrabotka obrabotka)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //Если валидация успешно прошла
                if (ModelState.IsValid)
                {
                    //Проверка на время
                    if (Convert.ToInt32(obrabotka.Vremya_Sob[0].ToString() + obrabotka.Vremya_Sob[1].ToString()) <= 23 && Convert.ToInt32(obrabotka.Vremya_Sob[3].ToString() + obrabotka.Vremya_Sob[4].ToString()) <= 59)
                    {
                        //Изменение данных
                        db.Entry(obrabotka).State = EntityState.Modified;
                        //Сохранение 
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", obrabotka.Sotr_ID);
                        //Вывод сообщения, что время указано некорректно
                        ModelState.AddModelError("Vremya_Sob", "Некорректное указано время");
                        return View(obrabotka);
                    }
                }
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", obrabotka.Sotr_ID);
                return View(obrabotka);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Obrabotka obrabotka = await db.Obrabotka.FindAsync(id);
                if (obrabotka == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(obrabotka);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                Obrabotka obrabotka = await db.Obrabotka.FindAsync(id);
                //Удаление
                db.Obrabotka.Remove(obrabotka);
                //Сохранение
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        //Служит для очистки мусора
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
