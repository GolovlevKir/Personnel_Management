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
    public class PositionsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Positions
        public ActionResult Index()
        {
            var positions = db.Positions.Include(p => p.Departments);
            return View(positions.ToList());
        }

        // GET: Positions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Positions positions = db.Positions.Find(id);
            if (positions == null)
            {
                return HttpNotFound();
            }
            return View(positions);
        }

        // GET: Positions/Create
        public ActionResult Create()
        {
            ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart");
            return View();
        }

        // POST: Positions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Positions,Naim_Posit,Salary,Depart_ID,Kol_Vo_Pers,Kol_Vo_On_Isp")] Positions positions)
        {
            if (ModelState.IsValid)
            {
                db.Positions.Add(positions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart", positions.Depart_ID);
            return View(positions);
        }

        // GET: Positions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Positions positions = db.Positions.Find(id);
            if (positions == null)
            {
                return HttpNotFound();
            }
            ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart", positions.Depart_ID);
            return View(positions);
        }

        // POST: Positions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Positions,Naim_Posit,Salary,Depart_ID,Kol_Vo_Pers,Kol_Vo_On_Isp")] Positions positions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(positions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart", positions.Depart_ID);
            return View(positions);
        }

        // GET: Positions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Positions positions = db.Positions.Find(id);
            if (positions == null)
            {
                return HttpNotFound();
            }
            return View(positions);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Positions positions = db.Positions.Find(id);
            db.Positions.Remove(positions);
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
