using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
        public static int idpos = 0;

        // GET: Positions
        [Authorize]
        public ActionResult Index(int id = 0)
        {
            Program.update();
            if (id != 0)
            {
                idpos = id;
                Program.id = id;
                var model = db.Positions.Include(x => x.Departments).ToList();
                model = model.Where(p => p.Depart_ID == id).ToList();
                SqlCommand command = new SqlCommand("SELECT Naim_Depart FROM Departments where ID_Depart = " + idpos.ToString(), Program.SqlConnection);
                Program.SqlConnection.Open();
                ViewBag.d = command.ExecuteScalar();
                Program.SqlConnection.Close();
                return View(model);
            }
            else
            {
                idpos = 0;
                var model = db.Positions.Include(x => x.Departments).ToList();
                return View(model);
            }
        }

        // GET: Positions/Details/5
        [Authorize]
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
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.u = idpos;
            ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart");
            return View();
        }

        // POST: Positions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Positions,Naim_Posit,Salary,Depart_ID,Kol_Vo_Pers,Kol_Vo_On_Isp")] Positions positions)
        {
            if (ModelState.IsValid)
            {
                if (idpos == 0)
                {
                    db.Positions.Add(positions);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    positions.Depart_ID = Convert.ToInt32(idpos);
                    db.Positions.Add(positions);
                    db.SaveChanges();
                    return Redirect("/Positions/Index/" + idpos.ToString()) ;
                }
            }
            ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart", positions.Depart_ID);
            return View(positions);
        }

        // GET: Positions/Edit/5
        [Authorize]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Positions,Naim_Posit,Salary,Depart_ID,Kol_Vo_Pers,Kol_Vo_On_Isp")] Positions positions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(positions).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Positions/Index/" + idpos.ToString());
            }
            ViewBag.Depart_ID = new SelectList(db.Departments, "ID_Depart", "Naim_Depart", positions.Depart_ID);
            return View(positions);
        }

        // GET: Positions/Delete/5
        [Authorize]
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Positions positions = db.Positions.Find(id);
            db.Positions.Remove(positions);
            db.SaveChanges();
            return Redirect("/Positions/Index/" + idpos.ToString());
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
