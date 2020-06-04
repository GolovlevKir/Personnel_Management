using Personal_Management.Hubs;
using Personal_Management.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shifr;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections.Generic;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class Pass_DannieController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Pass_Dannie
        [Authorize]
        public ActionResult Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //получение списка паспортных данных сотрудников, которые не уволены и не заблокированы
                var pass_Dannie = db.Pass_Dannie.Include(p => p.Sotrs).Where(i => i.Sotrs.fired == false && i.Sotrs.Accounts.Block == false);
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                //Поиск сотрудников, которым не назначены должностные обязанности
                command.CommandText = "SELECT count(*) FROM Sotrs LEFT JOIN Pass_Dannie ON Sotrs.ID_Sotr=Pass_Dannie.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Pass_Dannie.Sotr_ID IS NULL";
                Program.SqlConnection.Open();
                //Выполенение запроса
                int? es = (int)command.ExecuteScalar();
                Program.SqlConnection.Close();
                if (es == null || es == 0)
                {
                    ViewBag.i = false;
                }
                else
                {
                    ViewBag.i = true;
                }
                return View(pass_Dannie.ToList());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        //Частичное представление
        public ActionResult GetEmployeeData()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //получение списка паспортных данных сотрудников, которые не уволены и не заблокированы
                var pass_Dannie = db.Pass_Dannie.Include(p => p.Sotrs).Where(i => i.Sotrs.fired == false && i.Sotrs.Accounts.Block == false);
                return PartialView("_EmployeeData", pass_Dannie.ToList());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public ActionResult Create()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Список сотрудников
                string constr = ConfigurationManager.ConnectionStrings["PersonalContext"].ToString();
                SqlConnection _con = new SqlConnection(constr);
                //Поиск сотрудников, которым не назначены должностные инструкции
                SqlDataAdapter _da = new SqlDataAdapter("SELECT ID_Sotr, Surname_Sot + ' ' + Name_Sot + ' ' + Petronumic_Sot + ' (' + Login_Acc + ')' as FIO FROM Sotrs LEFT JOIN Pass_Dannie ON Sotrs.ID_Sotr=Pass_Dannie.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Pass_Dannie.Sotr_ID IS NULL", constr);
                DataTable _dt = new DataTable();
                _da.Fill(_dt);
                //Список сотрудников
                ViewBag.Sotr_ID = ToSelectList(_dt, "ID_Sotr", "FIO"); return View();
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID_Pass,S_Pas,N_Pas,Sotr_ID")] Pass_Dannie pass_Dannie)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    //Добавление данных
                    db.Pass_Dannie.Add(new Pass_Dannie {
                    Sotr_ID = pass_Dannie.Sotr_ID,
                    N_Pas = Shifrovanie.Encryption(pass_Dannie.N_Pas),
                    S_Pas = Shifrovanie.Encryption(pass_Dannie.S_Pas)
                    });
                    //Сохранение 
                    await db.SaveChangesAsync();
                    //Обновление списка пасортных данных у всех пользователей
                    EmployeesHub.BroadcastData();
                    return RedirectToAction("Index");
                }
                //Список сотрудников
                string constr = ConfigurationManager.ConnectionStrings["PersonalContext"].ToString();
                SqlConnection _con = new SqlConnection(constr);
                //Поиск сотрудников, которым не назначены должностные инструкции
                SqlDataAdapter _da = new SqlDataAdapter("SELECT ID_Sotr, Surname_Sot + ' ' + Name_Sot + ' ' + Petronumic_Sot + ' (' + Login_Acc + ')' as FIO FROM Sotrs LEFT JOIN Pass_Dannie ON Sotrs.ID_Sotr=Pass_Dannie.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Pass_Dannie.Sotr_ID IS NULL", constr);
                DataTable _dt = new DataTable();
                _da.Fill(_dt);
                //Список сотрудников
                ViewBag.Sotr_ID = ToSelectList(_dt, "ID_Sotr", "FIO", pass_Dannie.Sotr_ID);
                return View(pass_Dannie);
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
                //Добавление в виртуальный список
                list.Add(new SelectListItem()
                {
                    //Отображаемый текст
                    Text = row[textField].ToString(),
                    //Значение
                    Value = row[valueField].ToString(),
                });
            }
            //Возврат виртуального списка
            return new SelectList(list, "Value", "Text", id);
        }

        // GET: Pass_Dannie1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Pass_Dannie pass_Dannie = await db.Pass_Dannie.FindAsync(id);
                pass_Dannie.S_Pas = Shifrovanie.Decryption(pass_Dannie.S_Pas);
                pass_Dannie.N_Pas = Shifrovanie.Decryption(pass_Dannie.N_Pas);
                var fio = db.Sotrs.Where(f => f.ID_Sotr == pass_Dannie.Sotr_ID).FirstOrDefault();
                ViewBag.fio = fio.Full;
                if (pass_Dannie == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs.Where(i => i.fired == false && i.Accounts.Block == false), "ID_Sotr", "Full", pass_Dannie.Sotr_ID);
                return View(pass_Dannie);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID_Pass,S_Pas,N_Pas,Sotr_ID")] Pass_Dannie pass_Dannie)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    pass_Dannie.N_Pas = Shifrovanie.Encryption(pass_Dannie.N_Pas);
                    pass_Dannie.S_Pas = Shifrovanie.Encryption(pass_Dannie.S_Pas);
                    //Сохранение измений
                    db.Entry(pass_Dannie).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //Обновление у всех пользователей данных
                    EmployeesHub.BroadcastData();
                    return RedirectToAction("Index");
                }
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs.Where(i => i.fired == false && i.Accounts.Block == false), "ID_Sotr", "Full", pass_Dannie.Sotr_ID);
                var fio = db.Sotrs.Where(f => f.ID_Sotr == pass_Dannie.Sotr_ID).FirstOrDefault();
                ViewBag.fio = fio.Full;
                return View(pass_Dannie);
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
                //Поиск по ключу
                Pass_Dannie pass_Dannie = await db.Pass_Dannie.FindAsync(id);
                if (pass_Dannie == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(pass_Dannie);
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
                Pass_Dannie pass_Dannie = await db.Pass_Dannie.FindAsync(id);
                //Удаление записи
                db.Pass_Dannie.Remove(pass_Dannie);
                //Сохранение 
                await db.SaveChangesAsync();
                //Обновление у всех пользователей данных
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
    }
}
