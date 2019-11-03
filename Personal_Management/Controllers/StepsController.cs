using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Personal_Management.Models;

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
    }
}
