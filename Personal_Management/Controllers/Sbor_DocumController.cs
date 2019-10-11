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
    public class Sbor_DocumController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Sbor_Docum
        public ActionResult Index()
        {
            var sbor_Docum = db.Sbor_Docum.Include(s => s.Documents).Include(s => s.Sotrs);
            return View(sbor_Docum.ToList());
        }

        // GET: Sbor_Docum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sbor_Docum sbor_Docum = db.Sbor_Docum.Find(id);
            if (sbor_Docum == null)
            {
                return HttpNotFound();
            }
            return View(sbor_Docum);
        }

        // GET: Sbor_Docum/Create
        public ActionResult Create()
        {
            ViewBag.Doc_ID = new SelectList(db.Documents, "ID_Doc", "Doc_Naim");
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            return View();
        }

        // POST: Sbor_Docum/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Sbora,Doc_ID,Sotr_ID,Itog,Photo_Doc")] Sbor_Docum sbor_Docum)
        {
            if (ModelState.IsValid)
            {
                db.Sbor_Docum.Add(sbor_Docum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Doc_ID = new SelectList(db.Documents, "ID_Doc", "Doc_Naim", sbor_Docum.Doc_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", sbor_Docum.Sotr_ID);
            return View(sbor_Docum);
        }

        // GET: Sbor_Docum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sbor_Docum sbor_Docum = db.Sbor_Docum.Find(id);
            if (sbor_Docum == null)
            {
                return HttpNotFound();
            }
            ViewBag.Doc_ID = new SelectList(db.Documents, "ID_Doc", "Doc_Naim", sbor_Docum.Doc_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", sbor_Docum.Sotr_ID);
            return View(sbor_Docum);
        }

        // POST: Sbor_Docum/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Sbora,Doc_ID,Sotr_ID,Itog,Photo_Doc")] Sbor_Docum sbor_Docum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sbor_Docum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Doc_ID = new SelectList(db.Documents, "ID_Doc", "Doc_Naim", sbor_Docum.Doc_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", sbor_Docum.Sotr_ID);
            return View(sbor_Docum);
        }

        // GET: Sbor_Docum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sbor_Docum sbor_Docum = db.Sbor_Docum.Find(id);
            if (sbor_Docum == null)
            {
                return HttpNotFound();
            }
            return View(sbor_Docum);
        }

        // POST: Sbor_Docum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sbor_Docum sbor_Docum = db.Sbor_Docum.Find(id);
            db.Sbor_Docum.Remove(sbor_Docum);
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
