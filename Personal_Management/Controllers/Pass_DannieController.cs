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
    public class Pass_DannieController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Pass_Dannie
        [Authorize]
        public ActionResult Index()
        {
            var pass_Dannie = db.Pass_Dannie.Include(p => p.Sotrs);
            return View(pass_Dannie.ToList());
        }

        // GET: Pass_Dannie/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pass_Dannie pass_Dannie = db.Pass_Dannie.Find(id);
            if (pass_Dannie == null)
            {
                return HttpNotFound();
            }
            return View(pass_Dannie);
        }

        // GET: Pass_Dannie/Create
        [Authorize]
        public ActionResult Create()
        {
            Program.update();
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            return View();
        }

        // POST: Pass_Dannie/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Pass,S_Pas,N_Pas,Sotr_ID")] Pass_Dannie pass_Dannie)
        {
            if (ModelState.IsValid)
            {
                db.Pass_Dannie.Add(pass_Dannie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", pass_Dannie.Sotr_ID);
            return View(pass_Dannie);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pass_Dannie pass_Dannie = db.Pass_Dannie.Find(id);
            if (pass_Dannie == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", pass_Dannie.Sotr_ID);
            return View(pass_Dannie);
        }

        // POST: Pass_Dannie/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Pass,S_Pas,N_Pas,Sotr_ID")] Pass_Dannie pass_Dannie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pass_Dannie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", pass_Dannie.Sotr_ID);
            return View(pass_Dannie);
        }

        // GET: Pass_Dannie/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pass_Dannie pass_Dannie = db.Pass_Dannie.Find(id);
            if (pass_Dannie == null)
            {
                return HttpNotFound();
            }
            return View(pass_Dannie);
        }

        // POST: Pass_Dannie/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pass_Dannie pass_Dannie = db.Pass_Dannie.Find(id);
            db.Pass_Dannie.Remove(pass_Dannie);
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
