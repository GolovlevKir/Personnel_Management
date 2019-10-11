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
    public class Zar_PlataController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Zar_Plata
        public ActionResult Index()
        {
            var zar_Plata = db.Zar_Plata.Include(z => z.Sotrs);
            return View(zar_Plata.ToList());
        }

        // GET: Zar_Plata/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zar_Plata zar_Plata = db.Zar_Plata.Find(id);
            if (zar_Plata == null)
            {
                return HttpNotFound();
            }
            return View(zar_Plata);
        }

        // GET: Zar_Plata/Create
        public ActionResult Create()
        {
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot");
            return View();
        }

        // POST: Zar_Plata/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Zar,Sotr_ID,Data_Viplati,Vremya_Viplati,NDS,Itog")] Zar_Plata zar_Plata)
        {
            if (ModelState.IsValid)
            {
                db.Zar_Plata.Add(zar_Plata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", zar_Plata.Sotr_ID);
            return View(zar_Plata);
        }

        // GET: Zar_Plata/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zar_Plata zar_Plata = db.Zar_Plata.Find(id);
            if (zar_Plata == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", zar_Plata.Sotr_ID);
            return View(zar_Plata);
        }

        // POST: Zar_Plata/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Zar,Sotr_ID,Data_Viplati,Vremya_Viplati,NDS,Itog")] Zar_Plata zar_Plata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zar_Plata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", zar_Plata.Sotr_ID);
            return View(zar_Plata);
        }

        // GET: Zar_Plata/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zar_Plata zar_Plata = db.Zar_Plata.Find(id);
            if (zar_Plata == null)
            {
                return HttpNotFound();
            }
            return View(zar_Plata);
        }

        // POST: Zar_Plata/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Zar_Plata zar_Plata = db.Zar_Plata.Find(id);
            db.Zar_Plata.Remove(zar_Plata);
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
