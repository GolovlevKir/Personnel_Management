using Email;
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
    public class ZayavkaNaSobesController : Controller
    {
        private PersonalContext db = new PersonalContext();


        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //Получение списка заявок
                var zayavkaNaSobes = db.ZayavkaNaSobes.Where(z => z.itog == false).Include(z => z.Questions).Include(z => z.Sotrs);
                return View(await zayavkaNaSobes.ToListAsync());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public ActionResult Create()
        {
            //Если пользователь авторизован
            if (User.Identity.IsAuthenticated)
            {
                //Получение данных аккаунта
                var ids = db.Sotrs.Where(s => s.Login_Acc == User.Identity.Name).FirstOrDefault();
                //ФИО сотрудника
                ViewBag.FIO = ids.FullName;
                //Почта
                ViewBag.Im = ids.Email;
                if (ids.Petronumic_Sot == "" || ids.Petronumic_Sot == " " || ids.Petronumic_Sot == "-" || ids.Petronumic_Sot == null)
                {
                    //Номер заявки
                    Session["NumZ"] = "CMS_" + ids.Surname_Sot[0].ToString() + ids.Name_Sot[0].ToString() + "О" + DateTime.Now.ToString("hhmmssddMMyyyy");
                }
                else
                {
                    //Номер заявки
                    Session["NumZ"] = "CMS_" + ids.Surname_Sot[0].ToString() + ids.Name_Sot[0].ToString() + ids.Petronumic_Sot[0].ToString() + DateTime.Now.ToString("hhmmssddMMyyyy");
                }
                ViewBag.item = db.Questions;
                //Дата заявки
                Session["dataz"] = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ZayavkaNaSobes zayavkaNaSobes, int[] idqu, string[] Doc)
        {
            //Получение данных аккаунта
            var ids = db.Sotrs.Where(s => s.Login_Acc == User.Identity.Name).FirstOrDefault();
            if (idqu != null && idqu.Length > 0)
            {
                //Цикл по всем вопросам
                for (int i = 0; i < idqu.Length; i++)
                {
                    //Добавление данных
                    db.ZayavkaNaSobes.Add(new ZayavkaNaSobes
                    {
                        Sotr_ID = ids.ID_Sotr,
                        Qwe_ID = idqu[i],
                        otvet = Doc[i],
                        datazayavki = Session["dataz"].ToString(),
                        nomerzay = Session["NumZ"].ToString(),
                        itog = false

                    });
                    //Сохранение
                    db.SaveChanges();
                    ViewBag.Suсc = true;
                    ViewBag.SucсMes = "Ваша заявка успешно отправлена, ожидайте решение компании";

                }
                EmailTo.MySendMail("" +
                        "Приветствуем, " + ids.Full + "! " +
                        "Спасибо за оформленную заявку для прохождения собеседования в ООО \"Си эМ эС\"! Ожидайте, в ближайшее время мы примем решение.<br><br>" +
                        "Номер заявки: " + Session["NumZ"].ToString() + "* <br><br>" +
                        "* - С помощью данного номера, Вы можете просматривать результат поданной заявки!!!",
                         ids.Email, "Компания CMS", "Собеседование");
                //Вывод отчета по заявке
                return Redirect("/Home/ZayObr/" + Session["NumZ"].ToString());
            }
            return RedirectToAction("Zayavki", "Home");
        }

        // GET: ZayavkaNaSobes/Edit/5
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
                ZayavkaNaSobes zayavkaNaSobes = await db.ZayavkaNaSobes.FindAsync(id);
                if (zayavkaNaSobes == null)
                {
                    //400 ошибка
                    return HttpNotFound();
                }
                //Список вопросов
                ViewBag.Qwe_ID = new SelectList(db.Questions, "ID_Quest", "Quest_Naim", zayavkaNaSobes.Qwe_ID);
                //Список соискателей
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", zayavkaNaSobes.Sotr_ID);
                return View(zayavkaNaSobes);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        // POST: ZayavkaNaSobes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID_Zay,Sotr_ID,Qwe_ID,otvet")] ZayavkaNaSobes zayavkaNaSobes)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //Если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(zayavkaNaSobes).State = EntityState.Modified;
                    //Сохранение 
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                //Список вопросов
                ViewBag.Qwe_ID = new SelectList(db.Questions, "ID_Quest", "Quest_Naim", zayavkaNaSobes.Qwe_ID);
                //Список соискателей
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", zayavkaNaSobes.Sotr_ID);
                return View(zayavkaNaSobes);
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
                ZayavkaNaSobes zayavkaNaSobes = await db.ZayavkaNaSobes.FindAsync(id);
                if (zayavkaNaSobes == null)
                {
                    //400 ошибка
                    return HttpNotFound();
                }
                return View(zayavkaNaSobes);
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
                //Поиск по ключу
                ZayavkaNaSobes zayavkaNaSobes = await db.ZayavkaNaSobes.FindAsync(id);
                //Удаление записи
                db.ZayavkaNaSobes.Remove(zayavkaNaSobes);
                //Сохранение
                await db.SaveChangesAsync();
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


        public ActionResult CreateSob(string num)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //получение номера заявки
                var ids = db.ZayavkaNaSobes.Where(z => z.nomerzay == num).FirstOrDefault();
                int? id;
                if (ids == null)
                {
                    var sot = db.Sotrs.Where(z => z.Login_Acc == num).FirstOrDefault();
                    id = sot.ID_Sotr;
                }
                else
                {
                    //Ключ соискателя
                    id = ids.Sotr_ID;
                }

                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Obrabotka obrabotka = new Obrabotka();
                //Сегодняшняя дата
                obrabotka.Data_Sob = DateTime.Now.ToShortDateString();
                //Время онлан
                obrabotka.Vremya_Sob = DateTime.Now.ToShortTimeString();
                //номер заявки
                if (ids == null)
                {
                    obrabotka.nomerzay = "Отсутствует";
                }
                else
                {
                    obrabotka.nomerzay = num;
                }
                obrabotka.Sotr_ID = (int)id;
                //Список соискателей
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
        public ActionResult CreateSob(Obrabotka obrabotka)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //Данные заявки
                var ids = db.ZayavkaNaSobes.Where(z => z.nomerzay == obrabotka.nomerzay).FirstOrDefault();
                int? id;
                if (ids == null)
                {
                    var sot = db.Sotrs.Where(z => z.ID_Sotr == obrabotka.Sotr_ID).FirstOrDefault();
                    id = sot.ID_Sotr;
                }
                else
                {
                    id = ids.Sotr_ID;
                }

                //Если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Добавление данных
                    db.Obrabotka.Add(new Obrabotka
                    {
                        Data_Sob = obrabotka.Data_Sob,
                        Vremya_Sob = obrabotka.Vremya_Sob,
                        Kommnt = obrabotka.Kommnt,
                        reshenie = obrabotka.reshenie,
                        Sotr_ID = (int)id,
                        nomerzay = obrabotka.nomerzay
                    });
                    //Сохранение 
                    db.SaveChanges();
                    EmailTo.MySendMail("" +
                        "Приветствуем, " + ids.Sotrs.Full + "! " +
                        "Для Вас назначено собеседование.<br><br>" +
                        "Номер заявки: " + ids.nomerzay + "<br>" +
                        "Дата собеседования: " + obrabotka.Data_Sob + ";<br>" +
                        "Время собеседования: " + obrabotka.Vremya_Sob + ".<br><br>" +
                        "Большая просьба, не опаздывать!!!",
                         ids.Sotrs.Email, "Компания CMS", "Собеседование");
                    if (Session["perehod"] == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return Redirect(Session["perehod"].ToString());
                    }

                }
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", obrabotka.Sotr_ID);
                return View(obrabotka);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

    }
}
