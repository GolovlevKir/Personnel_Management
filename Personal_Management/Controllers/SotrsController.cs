using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Personal_Management.Models;

namespace Personal_Management.Controllers
{
    public class SotrsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Sotrs
        public ActionResult Index()
        {
            var sotrs = db.Sotrs.Include(s => s.Positions).Include(s => s.Rates).Include(s => s.Work_Schedule);
            return View(sotrs.ToList());
        }

        // GET: Sotrs/Details/5
        public ActionResult Details(int? id)
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

        // GET: Sotrs/Create
        public ActionResult Create()
        {
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit");
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate");
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche");
            return View();
        }

        // POST: Sotrs/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Sotr,Surname_Sot,Name_Sot,Petronumic_Sot,Day_Of_Birth,Address,Num_Phone,Email,Photo,Positions_ID,Rate_ID,Schedule_ID,Date_of_adoption,Opisanie")] Sotrs sotrs)
        {
            if (ModelState.IsValid)
            {
                db.Sotrs.Add(sotrs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit", sotrs.Positions_ID);
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate", sotrs.Rate_ID);
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche", sotrs.Schedule_ID);
            return View(sotrs);
        }

        // GET: Sotrs/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Sotrs/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Sotr,Surname_Sot,Name_Sot,Petronumic_Sot,Day_Of_Birth,Address,Num_Phone,Email,Photo,Positions_ID,Rate_ID,Schedule_ID,Date_of_adoption,Opisanie")] Sotrs sotrs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sotrs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit", sotrs.Positions_ID);
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate", sotrs.Rate_ID);
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche", sotrs.Schedule_ID);
            return View(sotrs);
        }

        // GET: Sotrs/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Sotrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sotrs sotrs = db.Sotrs.Find(id);
            db.Sotrs.Remove(sotrs);
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
