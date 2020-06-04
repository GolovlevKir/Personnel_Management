using Personal_Management.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class Work_ScheduleController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public async Task<ActionResult> Index()
        {
                if ((bool)Session["Manip_Sotrs"] == true || Session["Manip_Sotrs"] != null)
                {
                    //Список графиков работы
                    var work_Schedule = db.Work_Schedule.Include(w => w.DaysOfWeek).Include(w => w.Sotrs);
                    SqlCommand command = new SqlCommand("", Program.SqlConnection);
                    //Поиск сотрудников, которым не назначен график работы
                    command.CommandText = "SELECT count(*) FROM Sotrs LEFT JOIN Work_Schedule ON Sotrs.ID_Sotr=Work_Schedule.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Work_Schedule.Sotr_ID IS NULL";
                    Program.SqlConnection.Open();
                    int? i = (int)command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    if (i == null || i == 0)
                    {
                        //Запретить добавление графиков работы
                        ViewBag.i = false;
                    }
                    else
                    {
                        //Разрешить добавление
                        ViewBag.i = true;
                    }
                    return View(await work_Schedule.ToListAsync());
                }
                else
                {
                    return Redirect("/Error/NotRight");
                }

        }

        public ActionResult Create(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Дни недели
                ViewBag.item = db.DaysOfWeek;
                //Список дней недели
                ViewBag.Day_ID = new SelectList(db.DaysOfWeek, "ID_Day", "Naim_Day");
                string constr = ConfigurationManager.ConnectionStrings["PersonalContext"].ToString();
                SqlConnection _con = new SqlConnection(constr);
                SqlDataAdapter _da;
                //Поиск сотрудников, которым не назначен график работы
                if (id == null || id == 0)
                {
                    _da = new SqlDataAdapter("SELECT ID_Sotr, Surname_Sot + ' ' + Name_Sot + ' ' + Petronumic_Sot + ' (' + Login_Acc + ')' as FIO FROM Sotrs LEFT JOIN Work_Schedule ON Sotrs.ID_Sotr=Work_Schedule.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Work_Schedule.Sotr_ID IS NULL", constr);
                }
                else
                {
                    _da = new SqlDataAdapter("SELECT ID_Sotr, Surname_Sot + ' ' + Name_Sot + ' ' + Petronumic_Sot + ' (' + Login_Acc + ')' as FIO FROM Sotrs LEFT JOIN Work_Schedule ON Sotrs.ID_Sotr=Work_Schedule.Sotr_ID WHERE id_Sotr = " + id.ToString(), constr);
                }
                DataTable _dt = new DataTable();
                _da.Fill(_dt);
                //Список сотрудников
                ViewBag.Sotr_ID = ToSelectList(_dt, "ID_Sotr", "FIO");
                return View();
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField, int id = 1)
        {
            //Виртуальный список
            List<SelectListItem> list = new List<SelectListItem>();
            //Перебор строк виртуальной таблицы
            foreach (DataRow row in table.Rows)
            {
                //Добавление в список
                list.Add(new SelectListItem()
                {
                    //Отображаемое значение
                    Text = row[textField].ToString(),
                    //Значение
                    Value = row[valueField].ToString(),
                });
            }
            //Возрат списка сотрудников
            return new SelectList(list, "Value", "Text", id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID_Schedule,Day_ID,Vremya_Start,Vremya_End,Break_time,Break_Start,Break_End,Sotr_ID, Vih")] Work_Schedule work_Schedule, int[] deleteInputs, int[] deleteInputs2)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Добавлене рабочих дней
                    if (deleteInputs != null && deleteInputs.Length > 0)
                    {
                        for (int i = 0; i < deleteInputs.Length; i++)
                        {
                            work_Schedule.Day_ID = deleteInputs[i];
                            //Добавление
                            db.Work_Schedule.Add(work_Schedule);
                            //Сохранение в БД
                            await db.SaveChangesAsync();
                        }
                        if (deleteInputs2 != null && deleteInputs2.Length > 0)
                        {
                            //Добавление выходных дней
                            for (int i = 0; i < deleteInputs2.Length; i++)
                            {
                                work_Schedule.Day_ID = deleteInputs2[i];
                                work_Schedule.Break_End = "";
                                work_Schedule.Break_Start = "";
                                work_Schedule.Break_time = "";
                                work_Schedule.Vih = true;
                                work_Schedule.Vremya_End = "";
                                work_Schedule.Vremya_Start = "";
                                //Добавление данных
                                db.Work_Schedule.Add(work_Schedule);
                                //Сохранение 
                                await db.SaveChangesAsync();
                            }
                        }
                        return Redirect(Session["perehod"].ToString());
                    }

                }
                //Список дней недели
                ViewBag.Day_ID = new SelectList(db.DaysOfWeek, "ID_Day", "Naim_Day", work_Schedule.Day_ID);
                //Поиск сотрудников, которым не назначен график работы
                string constr = ConfigurationManager.ConnectionStrings["PersonalContext"].ToString();
                SqlConnection _con = new SqlConnection(constr);
                SqlDataAdapter _da = new SqlDataAdapter("SELECT ID_Sotr, Surname_Sot + ' ' + Name_Sot + ' ' + Petronumic_Sot + ' (' + Login_Acc + ')' as FIO FROM Sotrs LEFT JOIN Work_Schedule ON Sotrs.ID_Sotr=Work_Schedule.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Work_Schedule.Sotr_ID IS NULL", constr);
                DataTable _dt = new DataTable();
                _da.Fill(_dt);
                //Список сотрудников
                ViewBag.Sotr_ID = ToSelectList(_dt, "ID_Sotr", "FIO", work_Schedule.Sotr_ID);
                ViewBag.item = db.DaysOfWeek;
                return View(work_Schedule);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        // GET: Work_Schedule/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по коду
                Work_Schedule work_Schedule = await db.Work_Schedule.FindAsync(id);
                if (work_Schedule == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                //Список дней
                ViewBag.Day_ID = new SelectList(db.DaysOfWeek, "ID_Day", "Naim_Day", work_Schedule.Day_ID);
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", work_Schedule.Sotr_ID);
                return View(work_Schedule);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID_Schedule,Day_ID,Vremya_Start,Vremya_End,Break_time,Break_Start,Break_End,Sotr_ID,Vih")] Work_Schedule work_Schedule)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(work_Schedule).State = EntityState.Modified;
                    //Сохранение
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                //Список дней
                ViewBag.Day_ID = new SelectList(db.DaysOfWeek, "ID_Day", "Naim_Day", work_Schedule.Day_ID);
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", work_Schedule.Sotr_ID);
                return View(work_Schedule);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
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
                //Поиск по коду
                Work_Schedule work_Schedule = await db.Work_Schedule.FindAsync(id);
                if (work_Schedule == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(work_Schedule);
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
                Work_Schedule work_Schedule = await db.Work_Schedule.FindAsync(id);
                //Удаление
                db.Work_Schedule.Remove(work_Schedule);
                //Сохранение
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        //Отчет графиков работы
        public async Task<ActionResult> Otch()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Список графиков работы
                var work_Schedule = db.Work_Schedule.Include(w => w.DaysOfWeek).Include(w => w.Sotrs);
                return View(await work_Schedule.ToListAsync());
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
