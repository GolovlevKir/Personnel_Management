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
using Email;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class Isp_SrokiController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Isp_Sroki
        public async Task<ActionResult> Index(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //получение испытательных сроков
                var isp_Sroki = db.Isp_Sroki.Include(i => i.Posit_Responsibilities).Include(i => i.status_isp_sroka);
                //исключение уволенных и заблокированных сотрудников
                isp_Sroki = isp_Sroki.Where(i => i.Posit_Responsibilities.Sotrs.fired == false && i.Posit_Responsibilities.Sotrs.Accounts.Block == false);
                if (id != null && id != 0)
                {
                    //Если присутсвует ключ, то вывести по запрошенному ключу сотрудника и его испытательный срок 
                    isp_Sroki = isp_Sroki.Where(i => i.Posit_Responsibilities.Sotr_ID == id);
                }
                return View(await isp_Sroki.ToListAsync());
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
                //Получение строки подключения
                string constr = ConfigurationManager.ConnectionStrings["PersonalContext"].ToString();
                SqlConnection _con = new SqlConnection(constr);
                //Запрос на получение ФИО и должности
                SqlDataAdapter _da = new SqlDataAdapter("SELECT ID_Pos_Res, dbo.Sotrs.Surname_Sot + ' ' + dbo.Sotrs.Name_Sot + ' ' + dbo.Sotrs.Petronumic_Sot + ' (' + Naim_Posit + ')' as 'FIO' FROM dbo.Posit_Responsibilities JOIN dbo.Sotrs ON dbo.Posit_Responsibilities.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Positions ON dbo.Posit_Responsibilities.Positions_ID = dbo.Positions.ID_Positions where fired = 'false'", constr);
                DataTable _dt = new DataTable();
                _da.Fill(_dt);
                //Список штатного состава
                ViewBag.Pos_Res_ID = ToSelectList(_dt, "ID_Pos_Res", "FIO");
                //Спсиок статусов
                ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St");
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
            //Перебор строк таблицы из запроса
            foreach (DataRow row in table.Rows)
            {
                //Составление запроса
                list.Add(new SelectListItem()
                {
                    //Текст выводимый
                    Text = row[textField].ToString(),
                    //Значение
                    Value = row[valueField].ToString(),
                });
            }
            //Вывод списка
            return new SelectList(list, "Value", "Text", id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID_Isp,Pos_Res_ID,Date_Start,Date_Finish,Status_ID,itog")] Isp_Sroki isp_Sroki)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    //Добавление испытательного срока
                    db.Isp_Sroki.Add(isp_Sroki);
                    //Сохранение
                    await db.SaveChangesAsync();
                    SqlCommand command = new SqlCommand("", Program.SqlConnection);
                    //Ведутся ли этапы принятия 
                    command.CommandText = "select count(*) from Steps join Sotrs on Steps.Sotr_ID = ID_Sotr join Posit_Responsibilities on Posit_Responsibilities.Sotr_ID = ID_Sotr where ID_Pos_Res = " + isp_Sroki.Pos_Res_ID;
                    Program.SqlConnection.Open();
                    //выполнение запроса
                    int? step_count = (int)command.ExecuteScalar();
                    //Получение ключа сотрудника
                    command.CommandText = "select ID_Sotr from  Sotrs join Posit_Responsibilities on Posit_Responsibilities.Sotr_ID = ID_Sotr where ID_Pos_Res =  " + isp_Sroki.Pos_Res_ID;
                    //выполнение команды
                    int? idst = (int)command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    if (isp_Sroki.Status_ID != 1)
                    {
                        if (step_count != null && step_count != 0)
                        {
                            //Изменение этапов принятия
                            command.CommandText = "update [dbo].[Steps] set [Isp_Srok] = 'false' where Sotr_ID = " + idst;
                            Program.SqlConnection.Open();
                            //Выполнение запроса
                            command.ExecuteNonQuery();
                            Program.SqlConnection.Close();
                        }
                        else
                        {
                            //Добавление этапов принятия
                            command.CommandText = "Insert into dbo.Steps ([Sotr_ID],[Sobesedovanie],[Dolznost],[Grafik_Raboti],[Sbor_Documentov],[Isp_Srok],[Logical_Delete]) " +
                                "values (" + idst + ", 0,0,0,0,0,0)";
                            Program.SqlConnection.Open();
                            //выполнения запроса
                            command.ExecuteNonQuery();
                            Program.SqlConnection.Close();
                        }
                    }
                    else
                    {
                        if (step_count != null && step_count != 0)
                        {
                            //Изменение этапа принятия как пройденный испытательный срок
                            command.CommandText = "update [dbo].[Steps] set [Isp_Srok] = 'true' where Sotr_ID = " + idst;
                            Program.SqlConnection.Open();
                            //выполенение запроса
                            command.ExecuteNonQuery();
                            Program.SqlConnection.Close();
                        }
                        else
                        {
                            //Добавление испытательных сроков
                            command.CommandText = "Insert into dbo.Steps ([Sotr_ID],[Sobesedovanie],[Dolznost],[Grafik_Raboti],[Sbor_Documentov],[Isp_Srok],[Logical_Delete]) " +
                                "values (" + idst + ", 0,0,0,0,1,0)";
                            Program.SqlConnection.Open();
                            //выполнение запроса
                            command.ExecuteNonQuery();
                            Program.SqlConnection.Close();
                        }
                    }
                    return Redirect(Session["perehod"].ToString());
                }
                else
                {
                    //Строка подключения
                    string constr = ConfigurationManager.ConnectionStrings["PersonalContext"].ToString();
                    SqlConnection _con = new SqlConnection(constr);
                    //Добавление запроса
                    SqlDataAdapter _da = new SqlDataAdapter("SELECT ID_Pos_Res, dbo.Sotrs.Surname_Sot + ' ' + dbo.Sotrs.Name_Sot + ' ' + dbo.Sotrs.Petronumic_Sot + ' (' + Naim_Posit + ')' as 'FIO' FROM dbo.Posit_Responsibilities JOIN dbo.Sotrs ON dbo.Posit_Responsibilities.Sotr_ID = dbo.Sotrs.ID_Sotr JOIN dbo.Positions ON dbo.Posit_Responsibilities.Positions_ID = dbo.Positions.ID_Positions  where fired = 'false'", constr);
                    DataTable _dt = new DataTable();
                    _da.Fill(_dt);
                    //Список штатного состава
                    ViewBag.Pos_Res_ID = ToSelectList(_dt, "ID_Pos_Res", "FIO", isp_Sroki.Pos_Res_ID);
                    //Список статусов
                    ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St", isp_Sroki.Status_ID);

                    return View(isp_Sroki);
                }
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
                //поиск по ключу
                Isp_Sroki isp_Sroki = await db.Isp_Sroki.FindAsync(id);
                if (isp_Sroki == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                //Список штатного состава
                ViewBag.Pos_Res_ID = new SelectList(db.Posit_Responsibilities, "ID_Pos_Res", "ID_Pos_Res", isp_Sroki.Pos_Res_ID);
                //список статусов
                ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St", isp_Sroki.Status_ID);
                return View(isp_Sroki);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID_Isp,Pos_Res_ID,Date_Start,Date_Finish,Status_ID,itog")] Isp_Sroki isp_Sroki)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных и их фиксирование
                    db.Entry(isp_Sroki).State = EntityState.Modified;
                    SqlCommand command = new SqlCommand("", Program.SqlConnection);
                    //Манипулирование этапами принятия
                    command.CommandText = "select count(*) from Steps join Sotrs on Steps.Sotr_ID = ID_Sotr join Posit_Responsibilities on Posit_Responsibilities.Sotr_ID = ID_Sotr where ID_Pos_Res = " + isp_Sroki.Pos_Res_ID;
                    Program.SqlConnection.Open();
                    //Изменение тех что в процессе на ожидании
                    int? step_count = (int)command.ExecuteScalar();
                    command.CommandText = "select ID_Sotr from  Sotrs join Posit_Responsibilities on Posit_Responsibilities.Sotr_ID = ID_Sotr where ID_Pos_Res =  " + isp_Sroki.Pos_Res_ID;
                    int? idst = (int)command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    if (isp_Sroki.Status_ID != 1)
                    {
                        if (step_count != null && step_count != 0)
                        {
                            command.CommandText = "update [dbo].[Steps] set [Isp_Srok] = 'false' where Sotr_ID = " + idst;
                            Program.SqlConnection.Open();
                            //Изменение тех что в процессе на ожидании
                            command.ExecuteNonQuery();
                            Program.SqlConnection.Close();
                        }
                        else
                        {
                            command.CommandText = "Insert into dbo.Steps ([Sotr_ID],[Sobesedovanie],[Dolznost],[Grafik_Raboti],[Sbor_Documentov],[Isp_Srok],[Logical_Delete]) " +
                                "values (" + idst + ", 0,0,0,0,0,0)";
                            Program.SqlConnection.Open();
                            //Изменение тех что в процессе на ожидании
                            command.ExecuteNonQuery();
                            Program.SqlConnection.Close();
                        }
                        var sta = db.status_isp_sroka.Where(s => s.ID_St == isp_Sroki.Status_ID).FirstOrDefault();
                        var pos = db.Posit_Responsibilities.Where(s => s.ID_Pos_Res== isp_Sroki.Pos_Res_ID).FirstOrDefault();
                        EmailTo.MySendMail("" +
                        "Приветствуем, " + pos.Sotrs.Full + "! <br>Вы, " +
                        "проходите испытательный срок в нашей компании ООО \"Си эМ эС\", на должность " + pos.Positions.Naim_Posit + "." +
                        "<br />Ваш статус прохождения изменен на " + sta.Name_St,
                         pos.Sotrs.Email, "Компания CMS", "Прохождение испытательного срока");
                    }
                    else
                    {
                        if (step_count != null && step_count != 0)
                        {
                            command.CommandText = "update [dbo].[Steps] set [Isp_Srok] = 'true' where Sotr_ID = " + idst;
                            Program.SqlConnection.Open();
                            //Изменение тех что в процессе на ожидании
                            command.ExecuteNonQuery();
                            Program.SqlConnection.Close();
                        }
                        else
                        {
                            command.CommandText = "Insert into dbo.Steps ([Sotr_ID],[Sobesedovanie],[Dolznost],[Grafik_Raboti],[Sbor_Documentov],[Isp_Srok],[Logical_Delete]) " +
                                "values (" + idst + ", 0,0,0,0,1,0)";
                            Program.SqlConnection.Open();
                            //Изменение тех что в процессе на ожидании
                            command.ExecuteNonQuery();
                            Program.SqlConnection.Close();
                        }
                        var pos = db.Posit_Responsibilities.Where(s => s.ID_Pos_Res == isp_Sroki.Pos_Res_ID).FirstOrDefault();
                        EmailTo.MySendMail("" +
                        "Приветствуем, " + pos.Sotrs.Full + "! <br>Поздравляем, " +
                        "Вы прошли испытательный срок в нашей компании ООО \"Си эМ эС\", на должность " + pos.Positions.Naim_Posit + ".",
                         pos.Sotrs.Email, "Компания CMS", "Прохождение испытательного срока");
                    }
                    //сохранение изменений
                    await db.SaveChangesAsync();

                    return Redirect(Session["perehod"].ToString());
                }
                //Список штатного состава
                ViewBag.Pos_Res_ID = new SelectList(db.Posit_Responsibilities, "ID_Pos_Res", "ID_Pos_Res", isp_Sroki.Pos_Res_ID);
                //Список статусов
                ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St", isp_Sroki.Status_ID);
                return View(isp_Sroki);
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
                //поиск по ключу
                Isp_Sroki isp_Sroki = await db.Isp_Sroki.FindAsync(id);
                if (isp_Sroki == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(isp_Sroki);
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
                Isp_Sroki isp_Sroki = await db.Isp_Sroki.FindAsync(id);
                //удаление данных
                db.Isp_Sroki.Remove(isp_Sroki);
                //Сохранение
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

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
