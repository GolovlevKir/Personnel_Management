using Personal_Management.Hubs;
using Personal_Management.Models;
using Shifr;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class SotrsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Sotrs
        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение всех пользователей
                var sotrs = db.Sotrs.Include(s => s.Accounts);
                return View(await sotrs.ToListAsync());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public ActionResult Polz()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение исключительно клиентов
                var sotrs = db.Sotrs.Include(s => s.Accounts).Where(s => s.Guest.Equals(true) && s.Logical_Delete.Equals(false));
                return View(sotrs.ToList());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public ActionResult Sotr()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение сотрудников
                var sotrs = db.Sotrs.Include(s => s.Accounts).Where(s => s.Guest.Equals(false) && s.Logical_Delete.Equals(false));
                return View(sotrs.ToList());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public ActionResult Vse()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение всех пользователей
                var sotrs = db.Sotrs.Include(s => s.Accounts).Where(s => s.Logical_Delete.Equals(false));
                return View(sotrs.ToList());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        //Отчет "ШАБЛОН ЛИЧНОГО ДЕЛА"
        public async Task<ActionResult> Details(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //поиск по ключу
                Sotrs sotrs = await db.Sotrs.FindAsync(id);
                if (sotrs == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(sotrs);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        // Отчет "ПОЛНЫЙ ОТЧЕТ ПО СОТРУДНИКУ"
        public async Task<ActionResult> Details2(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //поиск по ключу
                Sotrs sotrs = await db.Sotrs.FindAsync(id);
                if (sotrs == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                //Получение данных штатного сотрудника
                var pos = db.Posit_Responsibilities.Where(p => p.Sotr_ID == id).FirstOrDefault();
                if (pos != null)
                {
                    //Отдел
                    ViewBag.dep = pos.Positions.Departments.Naim_Depart;
                    //Должность
                    ViewBag.posit = pos.Positions.Naim_Posit;
                    //Ставка
                    ViewBag.rate = pos.Rates.Rate;
                }
                else
                {
                    //Дефолтные значения
                    ViewBag.dep = "Не назначено";
                    ViewBag.posit = "Не назначено";
                    ViewBag.rate = "Не назначено";
                }
                if (pos != null)
                {
                    //Данные испытательного срока
                    var isp = db.Isp_Sroki.Where(p => p.Pos_Res_ID == pos.ID_Pos_Res).ToList();
                    if (isp != null)
                    {
                        ViewBag.isp = isp.ToList();
                    }
                    else
                    {
                        ViewBag.isp = null;
                    }
                }
                else
                {
                    ViewBag.isp = null;
                }
                //Данные реестра документов сотрудника
                var doc = db.Sbor_Docum.Where(p => p.Sotr_ID == id).ToList();
                ViewBag.doc = doc;
                var step = db.Steps.Where(p => p.Sotr_ID == id).ToList();
                ViewBag.step = step;
                return View(sotrs);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        // Отчет "ГРАФИК РАБОТЫ"
        public async Task<ActionResult> Details3(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //получение данных сотрудника
            Sotrs sotrs = await db.Sotrs.FindAsync(id);
            if (sotrs == null)
            {
                return HttpNotFound();
            }
            //Получение графика работы сотрудника
            var wk = db.Work_Schedule.Where(p => p.Sotr_ID == id).OrderBy(p => p.DaysOfWeek.ID_Day).ToList();
            //Получение данных штатного сотрудника
            var pos = db.Posit_Responsibilities.Where(p => p.Sotr_ID == id).FirstOrDefault();
            if (pos != null)
            {
                //Должность
                ViewBag.posit = pos.Positions.Naim_Posit;
            }
            else
            {
                //Дефолтные значения
                ViewBag.posit = "Не назначено";
            }
            //Добавление в виртуальную таблицу
            ViewBag.wk = wk;
            return View(sotrs);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Sotrs sotrs = await db.Sotrs.FindAsync(id);
                if (sotrs == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(sotrs);
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
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                Sotrs sotrs = await db.Sotrs.FindAsync(id);
                //Удаление 
                db.Sotrs.Remove(sotrs);
                //Сохранение 
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public async Task<ActionResult> DeleteLogic(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Sotrs sotrs = await db.Sotrs.FindAsync(id);
                if (sotrs == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(sotrs);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost, ActionName("DeleteLogic")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedLog(int id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                Sotrs sotrs = await db.Sotrs.FindAsync(id);
                //Логическое удаление
                sotrs.Logical_Delete = true;
                db.Entry(sotrs).State = EntityState.Modified;
                //Сохранение 
                await db.SaveChangesAsync();
                EmployeesHub.BroadcastData();
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


        public ActionResult AddOrEdit(int id = 0)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                Sotrs sotrs = new Sotrs();
                //Если ключ = 0
                if (id == 0)
                {
                    //Список аккаунтов
                    ViewBag.Login_Acc = new SelectList(db.Accounts.Where(a => a.Block == false), "Login", "Login");
                    return View(sotrs);
                }
                else
                {
                    //Поиск по ключу
                    sotrs = db.Sotrs.Find(id);
                    if (sotrs == null)
                    {
                        //404 ошибка
                        return HttpNotFound();
                    }
                    sotrs.Address = Shifrovanie.Decryption(sotrs.Address);
                    //Список аккаунтов
                    ViewBag.Login_Acc = new SelectList(db.Accounts.Where(a => a.Block == false), "Login", "Login", sotrs.Login_Acc);
                    if (sotrs == null)
                    {
                        //404 ошибка
                        return HttpNotFound();
                    }
                    return View(sotrs);
                }
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public ActionResult LichKab(int? id = 0)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение данных о сотруднике
                var sotrs = db.Sotrs.Where(s => s.ID_Sotr == id).ToList();
                ViewBag.dannie = sotrs;
                //Получение ключа сотрудника
                var sotid = db.Sotrs.Where(s => s.ID_Sotr == id).FirstOrDefault();
                ViewBag.id = sotid.ID_Sotr;
                //Получение данных штатного сотрудника
                var pos = db.Posit_Responsibilities.Where(p => p.Sotr_ID == id).FirstOrDefault();
                if (pos != null)
                {
                    //Наименование отдела
                    ViewBag.dep = pos.Positions.Departments.Naim_Depart;
                    //Наименование должности
                    ViewBag.posit = pos.Positions.Naim_Posit;
                    //Наименование трудовой ставки
                    ViewBag.rate = pos.Rates.Rate;
                }
                else
                {
                    //Дефолтные значения
                    ViewBag.dep = "Не назначено";
                    ViewBag.posit = "Не назначено";
                    ViewBag.rate = "Не назначено";
                }
                if (pos != null)
                {
                    //Данные по испытательному сроку
                    var isp = db.Isp_Sroki.Where(p => p.Pos_Res_ID == pos.ID_Pos_Res).ToList();
                    if (isp != null)
                    {
                        ViewBag.isp = isp.ToList();
                    }
                    else
                    {
                        ViewBag.isp = null;
                    }
                }
                else
                {
                    ViewBag.isp = null;
                }
                //Данные реестра документов
                var doc = db.Sbor_Docum.Where(p => p.Sotr_ID == id).ToList();
                ViewBag.doc = doc;
                //Данные этапов принятия
                var step = db.Steps.Where(p => p.Sotr_ID == id).ToList();
                ViewBag.step = step;
                return View();
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Sotrs sotrs)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (sotrs.Guest == false && sotrs.Date_of_adoption == null)
                {
                    //Если значение гость = false, необходимо указать дату принятия
                    ViewBag.Login_Acc = new SelectList(db.Accounts, "Login", "Login", sotrs.Login_Acc);
                    ModelState.AddModelError("Date_of_adoption", "Необходимо указать дату приема данного сотрудника");
                    return View(sotrs);
                }
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                //Запрос на получение возраста
                command.CommandText = "select Datediff(year, '" + sotrs.Day_Of_Birth + "', getdate())";
                int? voz = 0;
                Program.SqlConnection.Open();
                //Получение позраста
                voz = (int)command.ExecuteScalar();
                Program.SqlConnection.Close();
                if (voz < 16)
                {
                    //Сообщение о возрастных ограничениях
                    ViewBag.Login_Acc = new SelectList(db.Accounts.Where(a => a.Block == false), "Login", "Login", sotrs.Login_Acc);
                    ModelState.AddModelError("Day_Of_Birth", "Возрастные ограничения 18+");
                    return View(sotrs);
                }
                if (sotrs.ImageUpload != null)
                {
                    //Загрузка фото пользователя
                    string filename = Path.GetFileNameWithoutExtension(sotrs.ImageUpload.FileName);
                    string extension = Path.GetExtension(sotrs.ImageUpload.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                    sotrs.Photo = "/Content/Photo/st/" + filename;
                    sotrs.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Photo/st/"), filename));
                }
                if (sotrs.ID_Sotr.Equals(0))
                {
                    try
                    {
                        //Добавление пользователя
                        sotrs.Address = Shifrovanie.Encryption(sotrs.Address);
                        db.Sotrs.Add(sotrs);
                        //Сохранение 
                        db.SaveChanges();
                        EmployeesHub.BroadcastData();
                        return Redirect(Session["perehod"].ToString());

                    }
                    catch
                    {
                        //Список аккаунтов
                        ViewBag.Login_Acc = new SelectList(db.Accounts.Where(a => a.Block == false), "Login", "Login", sotrs.Login_Acc);
                        return View(sotrs);
                    }
                }
                else
                {
                    try
                    {
                        //Изменение данных
                        sotrs.Address = Shifrovanie.Encryption(sotrs.Address);
                        db.Entry(sotrs).State = EntityState.Modified;
                        //Сохранение данных
                        db.SaveChanges();
                        //Список аккаунтов
                        ViewBag.Login_Acc = new SelectList(db.Accounts.Where(a => a.Block == false), "Login", "Login", sotrs.Login_Acc);
                        EmployeesHub.BroadcastData();
                        return Redirect(Session["perehod"].ToString());
                    }
                    catch
                    {
                        //Список аккаунтов
                        ViewBag.Login_Acc = new SelectList(db.Accounts.Where(a => a.Block == false), "Login", "Login", sotrs.Login_Acc);
                        return View(sotrs);
                    }
                }
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }
    }
}
