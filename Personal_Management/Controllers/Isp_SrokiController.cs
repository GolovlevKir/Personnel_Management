using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Personal_Management.Models;

namespace Personal_Management.Controllers
{
    public class Isp_SrokiController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Isp_Sroki
        [Authorize]
        public ActionResult Index()
        {
            Program.update();
            var isp_Sroki = db.Isp_Sroki.Include(i => i.Sotrs).Include(i => i.status_isp_sroka);
            return View(isp_Sroki.ToList());
        }

        // GET: Isp_Sroki/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Isp_Sroki isp_Sroki = db.Isp_Sroki.Find(id);
            if (isp_Sroki == null)
            {
                return HttpNotFound();
            }
            return View(isp_Sroki);
        }

        // GET: Isp_Sroki/Create
        [Authorize]
        public ActionResult Create()
        {
            SelectList sot = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            ViewBag.Sotrs = sot;
            ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St");
            return View();
        }

        // POST: Isp_Sroki/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Isp,Sotr_ID,Date_Start,Date_Finish,Status_ID")] Isp_Sroki isp_Sroki)
        {
            if (ModelState.IsValid)
            {
                db.Isp_Sroki.Add(isp_Sroki);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St", isp_Sroki.Status_ID);
            return View(isp_Sroki);
        }

        // GET: Isp_Sroki/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Isp_Sroki isp_Sroki = db.Isp_Sroki.Find(id);
            if (isp_Sroki == null)
            {
                return HttpNotFound();
            }
            SelectList sot = new SelectList(db.Sotrs, "ID_Sotr", "Full", isp_Sroki.Sotr_ID);
            ViewBag.Sotrs = sot;
            ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St", isp_Sroki.Status_ID);
            return View(isp_Sroki);
        }

        // POST: Isp_Sroki/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Isp_Sroki isp_Sroki)
        {
            if (ModelState.IsValid)
            {
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                Program.SqlConnection.Open();
                command.CommandText = "update Isp_Sroki set Sotr_ID = " + isp_Sroki.Sotr_ID.ToString() + " , Date_Start = '" + isp_Sroki.Date_Start + "', Date_Finish = '" + isp_Sroki.Date_Finish + "', Status_ID = " + isp_Sroki.Status_ID.ToString() + " where ID_Isp = " + isp_Sroki.ID_Isp.ToString();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
                return RedirectToAction("Index");
            }
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", isp_Sroki.Sotr_ID);
            ViewBag.Status_ID = new SelectList(db.status_isp_sroka, "ID_St", "Name_St", isp_Sroki.Status_ID);
            return View(isp_Sroki);
        }

        // GET: Isp_Sroki/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Isp_Sroki isp_Sroki = db.Isp_Sroki.Find(id);
            if (isp_Sroki == null)
            {
                return HttpNotFound();
            }
            return View(isp_Sroki);
        }

        // POST: Isp_Sroki/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Isp_Sroki isp_Sroki = db.Isp_Sroki.Find(id);
            db.Isp_Sroki.Remove(isp_Sroki);
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
    }
}
