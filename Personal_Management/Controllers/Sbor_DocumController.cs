using Personal_Management.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class Sbor_DocumController : Controller
    {
        private PersonalContext db = new PersonalContext();

        [Authorize]
        public ActionResult Index(string search, int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение данных реестра документов сотрудников
                var sbor_Docum = db.Sbor_Docum.Include(s => s.Documents).Include(s => s.Sotrs).Where(s => s.Sotrs.fired == false && s.Sotrs.Accounts.Block == false);
                //Обновление испытательных сроков
                Program.update();
                if (id != null && id != 0)
                {
                    //Вывод по ключу сотрудника
                    sbor_Docum = sbor_Docum.Where(s => s.Sotr_ID == id);
                }
                if (search != null && search != "")
                {
                    //Поиск по данным
                    sbor_Docum = sbor_Docum.Where(s => s.Sotrs.Surname_Sot.Contains(search) || s.Sotrs.Name_Sot.Contains(search) || s.Sotrs.Petronumic_Sot.Contains(search) || s.Sotrs.Surname_Sot.Contains(search) && s.Sotrs.Name_Sot.Contains(search) || s.Sotrs.Name_Sot.Contains(search) && s.Sotrs.Petronumic_Sot.Contains(search) || s.Sotrs.Surname_Sot.Contains(search) && s.Sotrs.Name_Sot.Contains(search) && s.Sotrs.Petronumic_Sot.Contains(search) || s.Sotrs.Login_Acc.Contains(search));
                }
                return View(sbor_Docum.ToList());
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
                Sbor_Docum sbor_Docum = db.Sbor_Docum.Find(id);
                if (sbor_Docum == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                //Получение должностей
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                command.CommandText = "SELECT dbo.Positions.Naim_Posit FROM dbo.Sotrs JOIN dbo.Posit_Responsibilities ON dbo.Sotrs.ID_Sotr = dbo.Posit_Responsibilities.Sotr_ID INNER JOIN dbo.Positions ON dbo.Posit_Responsibilities.Positions_ID = dbo.Positions.ID_Positions where id_Sotr = " + sbor_Docum.Sotr_ID;
                Program.SqlConnection.Open();
                string i = command.ExecuteScalar().ToString();
                Program.SqlConnection.Close();
                if (i != null && i != "")
                    ViewBag.Dolj = "Должноть: " + i;
                else
                    ViewBag.Dolj = "";
                //Список документов
                ViewBag.Doc_ID = new SelectList(db.Documents, "ID_Doc", "Doc_Naim", sbor_Docum.Doc_ID);
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", sbor_Docum.Sotr_ID);
                return View(sbor_Docum);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sbor_Docum sbor_Docum, HttpPostedFileBase imgfile)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Список документов
                ViewBag.Doc_ID = new SelectList(db.Documents, "ID_Doc", "Doc_Naim", sbor_Docum.Doc_ID);
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", sbor_Docum.Sotr_ID);
                SqlCommand command;
                //Если загружен файл
                string path = upload(imgfile);
                if (!path.Equals("-1"))
                {
                    //Обновление данных
                    command = new SqlCommand(
                        "update Sbor_Docum " +
                        "set " +
                        "Doc_ID = '" + sbor_Docum.Doc_ID + "', " +
                        "Sotr_ID = '" + sbor_Docum.Sotr_ID + "', " +
                        "Itog = 1, " +
                        "Photo_Doc = '" + path + "' " +
                        "where ID_Sbora = " + sbor_Docum.ID_Sbora.ToString(),
                        Program.SqlConnection);
                    Program.SqlConnection.Open();
                    command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    return Redirect(Session["perehod"].ToString());
                }
                else
                {
                    //Обновление данных
                    command = new SqlCommand(
                        "update Sbor_Docum " +
                        "set " +
                        "Doc_ID = '" + sbor_Docum.Doc_ID + "', " +
                        "Sotr_ID = '" + sbor_Docum.Sotr_ID + "', " +
                        "Itog = 0, " +
                        "Photo_Doc = Photo_Doc " +
                        "where ID_Sbora = " + sbor_Docum.ID_Sbora.ToString(),
                        Program.SqlConnection);
                    Program.SqlConnection.Open();
                    command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    return Redirect(Session["perehod"].ToString());
                }
            }
            else
            {
                return Redirect("/Error/NotRight");
            }

        }

        public string upload(HttpPostedFileBase file)
        {
            Random r = new Random();
            //Изначальное значение пути
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                try
                {
                    //Путь к файлу
                    path = Path.Combine(Server.MapPath("~/Content/Photo/dok/"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                    //Сохранение файла
                    file.SaveAs(path);
                    //Имя файла
                    path = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    //    ViewBag.Message = "File uploaded successfully";
                }
                catch
                {
                    path = "-1";
                }
            }
            return path;
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
                //Поиск по коду
                Sbor_Docum sbor_Docum = db.Sbor_Docum.Find(id);
                if (sbor_Docum == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(sbor_Docum);
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
                //Очистка данных
                Sbor_Docum sbor_Docum = db.Sbor_Docum.Find(id);
                SqlCommand command;
                command = new SqlCommand(
                                "update Sbor_Docum " +
                                "set " +
                                "Itog = 0, " +
                                "Photo_Doc = '' " +
                                "where ID_Sbora = " + sbor_Docum.ID_Sbora.ToString(),
                                Program.SqlConnection);
                Program.SqlConnection.Open();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
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
