using Personal_Management.Models;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class StepsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Steps
        [Authorize]
        public ActionResult Index()
        {
            var steps = db.Steps.Include(s => s.Sotrs);
            return View(steps.ToList());
        }

        // GET: Steps/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Steps steps = db.Steps.Find(id);
            if (steps == null)
            {
                return HttpNotFound();
            }
            return View(steps);
        }

        // GET: Steps/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot");
            return View();
        }

        // POST: Steps/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Step,Sotr_ID,AddSotrInIS,AddRezume,AddSobesedovanie,AddIspSrok,RezimOzidaniya,Reshenie")] Steps steps)
        {
            if (ModelState.IsValid)
            {
                db.Steps.Add(steps);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", steps.Sotr_ID);
            return View(steps);
        }

        // GET: Steps/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Steps steps = db.Steps.Find(id);
            if (steps == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", steps.Sotr_ID);
            return View(steps);
        }

        // POST: Steps/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Step,Sotr_ID,AddSotrInIS,AddRezume,AddSobesedovanie,AddIspSrok,RezimOzidaniya,Reshenie")] Steps steps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(steps).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", steps.Sotr_ID);
            return View(steps);
        }

        // GET: Steps/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Steps steps = db.Steps.Find(id);
            if (steps == null)
            {
                return HttpNotFound();
            }
            return View(steps);
        }

        // POST: Steps/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Steps steps = db.Steps.Find(id);
            db.Steps.Remove(steps);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddRezume(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sotrs sotrs = db.Sotrs.Find(id);
            if (sotrs == null)
            {
                return HttpNotFound();
            }
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit", sotrs.Positions_ID);
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate", sotrs.Rate_ID);
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche", sotrs.Schedule_ID);
            return View(sotrs);
        }

        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        public ActionResult AddRezume(Sotrs sotrs, HttpPostedFileBase doc)
        {
            SqlCommand command;
            string pathrez = uploaddoc(doc);
            //Изменение данных
            if (pathrez.Equals("-1"))
            {
                command = new SqlCommand(
                        "update Sotrs " +
                        "set " +
                        "rezume = rezume " +
                        "where ID_Sotr = " + sotrs.ID_Sotr,
                        Program.SqlConnection);
                Program.SqlConnection.Open();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
                return RedirectToAction("Index");
            }
            else
            {
                command = new SqlCommand(
                    "update Sotrs " +
                    "set " +
                    "rezume = '" + pathrez + "' " +
                    "where ID_Sotr = " + sotrs.ID_Sotr,
                    Program.SqlConnection);
                Program.SqlConnection.Open();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
                return RedirectToAction("Index");
            }

        }

        public string uploaddoc(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".doc") || extension.ToLower().Equals(".docx") || extension.ToLower().Equals(".pdf"))
                {
                    try
                    {
                        //Изменение пути
                        path = Path.Combine(Server.MapPath("~/Content/Files/"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                        //Сохранение документа
                        file.SaveAs(path);
                        //Наименование файла
                        path = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    }
                    catch
                    {
                        path = "-1";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Форматы резюме только doc ,docx или pdf");
                }
            }
            return path;
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddOpisanie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sotrs sotrs = db.Sotrs.Find(id);
            if (sotrs == null)
            {
                return HttpNotFound();
            }
            return View(sotrs);
        }

        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        public ActionResult AddOpisanie(Sotrs sotrs, string znaniya, string obr, string staz)
        {
            SqlCommand command;
            if (staz == "")
            {
                staz = "Не известно (Отсутствие)";
            }
            if (obr == "")
            {
                staz = "Не известно (Отсутствует)";
            }
            if (znaniya == "")
            {
                staz = "Не известно (Отсутствуют)";
            }
            //Изменение данных
            if (znaniya == "" && obr == "" && staz == "")
            {
                command = new SqlCommand(
                        "update Sotrs " +
                        "set " +
                        "Opisanie = Opisanie " +
                        "where ID_Sotr = " + sotrs.ID_Sotr,
                        Program.SqlConnection);
                Program.SqlConnection.Open();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
                return RedirectToAction("Index");
            }
            else
            {
                command = new SqlCommand(
                    "update Sotrs " +
                    "set " +
                    "Opisanie = '" + "Стаж работы: " + staz + "\nОбразование:\n" + obr + "\nОбщие знания:\n" + znaniya + "' " +
                    "where ID_Sotr = " + sotrs.ID_Sotr,
                    Program.SqlConnection);
                Program.SqlConnection.Open();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
                return RedirectToAction("Index");
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult AddIsp(int id)
        {
            SelectList sot = new SelectList(db.Sotrs, "ID_Sotr", "Full",id);
            ViewBag.Sotrs = sot;
            ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St");
            return View();
        }

        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        public ActionResult AddIsp([Bind(Include = "ID_Isp,Sotr_ID,Date_Start,Date_Finish,Status_ID")] Isp_Sroki isp_Sroki)
        {
            ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St", isp_Sroki.Status_ID);
            SelectList sot = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            ViewBag.Sotrs = sot;
            SqlCommand command;
            command = new SqlCommand(
                "insert into [dbo].[Isp_Sroki] (Sotr_ID, Date_Start, Date_Finish, Status_ID, itog) values (" + isp_Sroki.Sotr_ID.ToString() + ",'" + isp_Sroki.Date_Start + "','" + isp_Sroki.Date_Finish + "'," + isp_Sroki.Status_ID.ToString() + ",'')", Program.SqlConnection);
            Program.SqlConnection.Open();
            command.ExecuteScalar();
            command = new SqlCommand(
                "update Steps set AddIspSrok = 1 where Sotr_ID = " + isp_Sroki.Sotr_ID.ToString(), Program.SqlConnection);
            command.ExecuteScalar();
            Program.SqlConnection.Close();
            return RedirectToAction("Index");


        }

        [Authorize]
        [HttpGet]
        public ActionResult UpdIsp(int id)
        {
            SqlCommand command;
            command = new SqlCommand(
                "select ID_Isp from [dbo].[Isp_Sroki] where Sotr_ID = " + id.ToString(), Program.SqlConnection);
            Program.SqlConnection.Open();
            int i = (int)command.ExecuteScalar();
            Program.SqlConnection.Close();
            Isp_Sroki isp_Sroki = db.Isp_Sroki.Find(i);
            if (isp_Sroki == null)
            {
                return HttpNotFound();
            }
            SelectList sot = new SelectList(db.Sotrs, "ID_Sotr", "Full",id);
            ViewBag.Sotrs = sot;
            ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St");
            return View(isp_Sroki);
        }

        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        public ActionResult UpdIsp([Bind(Include = "ID_Isp,Sotr_ID,Date_Start,Date_Finish,Status_ID")] Isp_Sroki isp_Sroki)
        {
            ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St", isp_Sroki.Status_ID);
            SelectList sot = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            ViewBag.Sotrs = sot;
            SqlCommand command;
            command = new SqlCommand(
                "update Isp_Sroki set Sotr_ID = Sotr_ID, Date_Start = '" + isp_Sroki.Date_Start + "', Date_Finish = '" + isp_Sroki.Date_Finish + "', Status_ID = " + isp_Sroki.Status_ID.ToString() + " where ID_Isp = " + isp_Sroki.ID_Isp.ToString(), Program.SqlConnection);
            Program.SqlConnection.Open();
            command.ExecuteScalar();
            Program.SqlConnection.Close();
            return RedirectToAction("Index");
        }
    }
}
