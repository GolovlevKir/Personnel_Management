using Personal_Management.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public ActionResult Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Проверка испытательного срока
                Program.update();
                //Список документов
                return View(db.Documents.ToList());
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
                return View();
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Doc,Doc_Naim")] Documents documents, List<string> Doc, List<string> Doc2)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Поиск документа с наименованием, которое уже существует
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                command.CommandText = "Select count(*) from Documents where Doc_Naim = '" + documents.Doc_Naim + "'";
                Program.SqlConnection.Open();
                int co = (int)command.ExecuteScalar();
                Program.SqlConnection.Close();
                var s = db.Posit_Responsibilities.OrderBy(m => m.Sotr_ID).GroupBy(m => m.Sotr_ID).ToList();
                ViewBag.s = s;
                if (co == 0)
                {
                    //Перебор всех докуентов
                    for (int i = 0; i < Doc.Count; i++)
                    {
                        if (Doc[i] != null && Doc[i] != "")
                        {
                            //Добавление данных
                            documents.Doc_Naim = Doc[i];
                            db.Documents.Add(documents);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    //Вывод сообщения, что такой документ уже существует
                    ViewBag.m = "Такой документ уже существует!";
                }

                return View(documents);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

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
                Documents documents = db.Documents.Find(id);
                if (documents == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(documents);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Doc,Doc_Naim")] Documents documents)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(documents).State = EntityState.Modified;
                    //Сохранение
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(documents);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

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
                Documents documents = db.Documents.Find(id);
                if (documents == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(documents);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Удаление данных
                Documents documents = db.Documents.Find(id);
                db.Documents.Remove(documents);
                //Сохранение 
                db.SaveChanges();
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

        [HttpPost]
        public ActionResult BatchDelete(int[] deleteInputs)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                // Удаление нескольких записей
                if (deleteInputs != null && deleteInputs.Length > 0)
                {
                    //Перебор всех выбранных записей
                    for (int i = 0; i < deleteInputs.Length; i++)
                    {
                        Documents documents = db.Documents.Find(deleteInputs[i]);
                        db.Documents.Remove(documents);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }
    }


}
