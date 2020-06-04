using Personal_Management.Models;
using System;
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
    public class PositionsController : Controller
    {
        private PersonalContext db = new PersonalContext();
        public static int idpos = 0;

        [Authorize]
        public ActionResult Index(int id = 0)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                var dep = db.Positions;
                var shtat = db.Posit_Responsibilities;
                foreach (Positions item in dep)
                {
                    command.CommandText = "Select Count(*) from [dbo].[Posit_Responsibilities] where [Positions_ID] = " + item.ID_Positions;
                    Program.SqlConnection.Open();
                    int? i = (int)command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    if (i != 0)
                    {
                        command.CommandText = "Update [dbo].[Positions] set [Vak_Mest] = [Kol_Vo_Pers] - " + i.ToString() + " where [ID_Positions] = " + item.ID_Positions.ToString();
                        Program.SqlConnection.Open();
                        command.ExecuteNonQuery();
                        Program.SqlConnection.Close();
                    }
                }
                //Обновление испытательных сроков
                Program.update();
                //Исли ключ не равен 0
                if (id != 0)
                {
                    idpos = id;
                    Program.id = id;
                    //группировка должностей по ключу отдела
                    var model = db.Positions.Include(x => x.Departments).ToList();
                    model = model.Where(p => p.Depart_ID == id).ToList();
                    //Получение наименования отдела
                    command.CommandText = "SELECT Naim_Depart FROM Departments where ID_Depart = " + idpos.ToString();
                    Program.SqlConnection.Open();
                    ViewBag.d = command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    return View(model);
                }
                else
                {
                    idpos = 0;
                    //Иначе вывести все должности
                    var model = db.Positions.Include(x => x.Departments).ToList();
                    return View(model);
                }
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
                ViewBag.u = idpos;
                //Список отделов
                ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart");
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
        public ActionResult Create([Bind(Include = "ID_Positions,Naim_Posit,Salary,Depart_ID,Kol_Vo_Pers,Kol_Vo_On_Isp,Vak_Mest,Logical_Delete")] Positions positions)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                //Проверка на повторения
                command.CommandText = "Select count(*) from Positions where Naim_Posit = '" + positions.Naim_Posit + "'";
                Program.SqlConnection.Open();
                int co = (int)command.ExecuteScalar();
                ViewBag.m = "";
                Program.SqlConnection.Close();
                if (co == 0)
                {
                    //Если валидация прошла успешно
                    if (ModelState.IsValid)
                    {
                        if (idpos == 0)
                        {
                            //Добавление записи
                            db.Positions.Add(positions);
                            //Сохранение 
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            //Добавление записи при известном id
                            positions.Depart_ID = Convert.ToInt32(idpos);
                            //Добавление
                            db.Positions.Add(positions);
                            //Сохранение
                            db.SaveChanges();
                            //Переадресация
                            return Redirect("/Positions/Index/" + idpos.ToString());
                        }
                    }
                }
                else
                {
                    //Вывод текста, что такая должность уже существует
                    ViewBag.m = "Такая должность уже существует!";
                }
                //Список отделов
                ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart", positions.Depart_ID);
                return View(positions);
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
                //Поиск по ключу
                Positions positions = db.Positions.Find(id);
                if (positions == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                //Список отделов
                ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart", positions.Depart_ID);
                return View(positions);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Positions,Naim_Posit,Salary,Depart_ID,Kol_Vo_Pers,Kol_Vo_On_Isp,Vak_Mest,Logical_Delete")] Positions positions)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(positions).State = EntityState.Modified;
                    //Сохранение 
                    db.SaveChanges();
                    return Redirect("/Positions/Index/" + idpos.ToString());
                }
                //Список должностей
                ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart", positions.Depart_ID);
                return View(positions);
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
                    //Ошибка 400
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по коду
                Positions positions = db.Positions.Find(id);
                if (positions == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(positions);
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
                Positions positions = db.Positions.Find(id);
                db.Positions.Remove(positions);
                //Сохранение 
                db.SaveChanges();
                return Redirect("/Positions/Index/" + idpos.ToString());
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
                Positions pos = await db.Positions.FindAsync(id);
                if (pos == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(pos);
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
                Positions pos = await db.Positions.FindAsync(id);
                //Логическое удаление
                pos.Logical_Delete = true;
                //изменение данных
                db.Entry(pos).State = EntityState.Modified;
                //Сохранение
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public async Task<ActionResult> VozvLogic(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Positions pos = await db.Positions.FindAsync(id);
                if (pos == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(pos);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [HttpPost, ActionName("VozvLogic")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VozvLogic(int id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                Positions pos = await db.Positions.FindAsync(id);
                //Восстановление данных, подверженных логическому удалению
                pos.Logical_Delete = false;
                //Изменение данных
                db.Entry(pos).State = EntityState.Modified;
                //Сохранение
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public async Task<ActionResult> OtchVak()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                var pos = await db.Positions.Where(v => v.Vak_Mest > 0).ToListAsync();
                var de = await db.Departments.ToListAsync();
                ViewBag.de = de;
                ViewBag.po = pos;
                return View(pos);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }
    }
}
