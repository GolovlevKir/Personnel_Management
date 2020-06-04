using Personal_Management.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class Posit_ResponsibilitiesController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение списка штатного состава
                var posit_Responsibilities = db.Posit_Responsibilities.Include(p => p.Positions).Include(p => p.Rates).Include(p => p.Sotrs).Where(s => s.Sotrs.fired == false);
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                //Поиск сотрудников, которым не назначены должностные обязанности
                command.CommandText = "SELECT count(*) FROM Sotrs LEFT JOIN Posit_Responsibilities ON Sotrs.ID_Sotr=Posit_Responsibilities.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Posit_Responsibilities.Sotr_ID IS NULL";
                Program.SqlConnection.Open();
                //Выполенение запроса
                int? i = (int)command.ExecuteScalar();
                Program.SqlConnection.Close();
                if (i == null || i == 0)
                {
                    ViewBag.i = false;
                }
                else
                {
                    ViewBag.i = true;
                }
                return View(await posit_Responsibilities.ToListAsync());
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
                //Список должностей
                ViewBag.Positions_ID = new SelectList(db.Positions.Where(p => p.Vak_Mest > 0), "ID_Positions", "Naim_Posit");
                //Список ставок
                ViewBag.Rates_ID = new SelectList(db.Rates, "ID_Rate", "Rate");
                string constr = ConfigurationManager.ConnectionStrings["PersonalContext"].ToString();
                SqlConnection _con = new SqlConnection(constr);
                SqlDataAdapter _da;
                //Поиск сотрудников, которым не назначены должностные инструкции
                if (id == null || id == 0)
                {
                    _da = new SqlDataAdapter("SELECT ID_Sotr, Surname_Sot + ' ' + Name_Sot + ' ' + Petronumic_Sot + ' (' + Login_Acc + ')' as FIO FROM Sotrs LEFT JOIN Posit_Responsibilities ON Sotrs.ID_Sotr=Posit_Responsibilities.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Posit_Responsibilities.Sotr_ID IS NULL", constr);
                }
                else
                {
                    _da = new SqlDataAdapter("SELECT ID_Sotr, Surname_Sot + ' ' + Name_Sot + ' ' + Petronumic_Sot + ' (' + Login_Acc + ')' as FIO FROM Sotrs LEFT JOIN Posit_Responsibilities ON Sotrs.ID_Sotr=Posit_Responsibilities.Sotr_ID WHERE ID_Sotr = " + id.ToString(), constr);
                }
                DataTable _dt = new DataTable();
                //Заполнение виртуальной таблицы
                _da.Fill(_dt);
                ViewBag.Sotr_ID = ToSelectList(_dt, "ID_Sotr", "FIO");
                return View();
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID_Pos_Res,Sotr_ID,Positions_ID,Rates_ID")] Posit_Responsibilities posit_Responsibilities)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация успешно прошла
                if (ModelState.IsValid)
                {
                    //Добавление данных
                    db.Posit_Responsibilities.Add(posit_Responsibilities);
                    //Сохранение 
                    await db.SaveChangesAsync();
                    return Redirect(Session["perehod"].ToString());
                }
                //Список должностей
                ViewBag.Positions_ID = new SelectList(db.Positions.Where(p => p.Vak_Mest > 0), "ID_Positions", "Naim_Posit", posit_Responsibilities.Positions_ID);
                //Список ставок
                ViewBag.Rates_ID = new SelectList(db.Rates, "ID_Rate", "Rate", posit_Responsibilities.Rates_ID);
                string constr = ConfigurationManager.ConnectionStrings["PersonalContext"].ToString();
                SqlConnection _con = new SqlConnection(constr);
                //Поиск сотрудников, которым не назначены должностные инструкции
                SqlDataAdapter _da = new SqlDataAdapter("SELECT ID_Sotr, Surname_Sot + ' ' + Name_Sot + ' ' + Petronumic_Sot + ' (' + Login_Acc + ')' as FIO FROM Sotrs LEFT JOIN Posit_Responsibilities ON Sotrs.ID_Sotr=Posit_Responsibilities.Sotr_ID WHERE Guest = 'false' and [fired] = 'false' and Posit_Responsibilities.Sotr_ID IS NULL", constr);
                DataTable _dt = new DataTable();
                _da.Fill(_dt);
                //Список сотрудников
                ViewBag.Sotr_ID = ToSelectList(_dt, "ID_Sotr", "FIO", posit_Responsibilities.Sotr_ID);
                return View(posit_Responsibilities);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


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
                Posit_Responsibilities posit_Responsibilities = await db.Posit_Responsibilities.FindAsync(id);
                if (posit_Responsibilities == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                //Список должностей
                ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit", posit_Responsibilities.Positions_ID);
                //Список ставок
                ViewBag.Rates_ID = new SelectList(db.Rates, "ID_Rate", "Rate", posit_Responsibilities.Rates_ID);
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", posit_Responsibilities.Sotr_ID);
                return View(posit_Responsibilities);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID_Pos_Res,Sotr_ID,Positions_ID,Rates_ID")] Posit_Responsibilities posit_Responsibilities)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация успешно прошла
                if (ModelState.IsValid)
                {
                    //Сохранение данных
                    db.Entry(posit_Responsibilities).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                //Список должностей
                ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit", posit_Responsibilities.Positions_ID);
                //Список ставок
                ViewBag.Rates_ID = new SelectList(db.Rates, "ID_Rate", "Rate", posit_Responsibilities.Rates_ID);
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", posit_Responsibilities.Sotr_ID);
                return View(posit_Responsibilities);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        // GET: Posit_Responsibilities/Delete/5
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
                Posit_Responsibilities posit_Responsibilities = await db.Posit_Responsibilities.FindAsync(id);
                if (posit_Responsibilities == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(posit_Responsibilities);
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
                Posit_Responsibilities posit_Responsibilities = await db.Posit_Responsibilities.FindAsync(id);
                //Удаление записи
                db.Posit_Responsibilities.Remove(posit_Responsibilities);
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
    }
}
