using Personal_Management.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class DocumentsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Documents
        [Authorize]
        public ActionResult Index()
        {
            Program.update();

            return View(db.Documents.ToList());
        }


        // GET: Documents/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Doc,Doc_Naim")] Documents documents)
        {
            SqlCommand command = new SqlCommand("", Program.SqlConnection);
            command.CommandText = "Select count(*) from Documents where Doc_Naim = '" + documents.Doc_Naim + "'";
            Program.SqlConnection.Open();
            int co = (int)command.ExecuteScalar();
            Program.SqlConnection.Close();
            if (co == 0)
            {
                if (ModelState.IsValid)
                {
                    //Добавление данных
                    db.Documents.Add(documents);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.m = "Такой документ уже существует!";
            }

            return View(documents);
        }

        // GET: Documents/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            return View(documents);
        }

        // POST: Documents/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Doc,Doc_Naim")] Documents documents)
        {
            if (ModelState.IsValid)
            {
                //Изменение данных
                db.Entry(documents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(documents);
        }

        // GET: Documents/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            return View(documents);
        }

        // POST: Documents/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Удаление данных
            Documents documents = db.Documents.Find(id);
            db.Documents.Remove(documents);
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
