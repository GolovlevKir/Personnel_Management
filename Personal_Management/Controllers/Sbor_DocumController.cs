using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
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
        [Authorize]
        public ActionResult Index()
        {
            Program.update();
            var sbor_Docum = db.Sbor_Docum.Include(s => s.Documents).Include(s => s.Sotrs);
            return View(sbor_Docum.ToList());
        }


        // GET: Sbor_Docum/Details/5
        [Authorize]
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
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Doc_ID = new SelectList(db.Documents, "ID_Doc", "Doc_Naim");
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            return View();
        }

        // POST: Sbor_Docum/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sbor_Docum sbor_Docum, HttpPostedFileBase imgfile)
        {
            ViewBag.Doc_ID = new SelectList(db.Documents, "ID_Doc", "Doc_Naim", sbor_Docum.Doc_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", sbor_Docum.Sotr_ID);
            SqlCommand command;
            string path = upload(imgfile);
            if (!path.Equals("-1"))
            {
                    command = new SqlCommand(
                        "update Sbor_Docum " +
                        "set " +
                        "Doc_ID = '" + sbor_Docum.Doc_ID + "', " +
                        "Sotr_ID = '" + sbor_Docum.Sotr_ID + "', " +
                        "Itog = 1, " +
                        "Photo_Doc = '" + path + "' " +
                        "where ID_Sbora = " + sbor_Docum.ID_Sbora.ToString(),
                        Program.SqlConnection);
                    Program.SqlConnection.Open();
                    command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    return RedirectToAction("Index");
                }
                else
                {
                    command = new SqlCommand(
                        "update Sbor_Docum " +
                        "set " +
                        "Doc_ID = '" + sbor_Docum.Doc_ID + "', " +
                        "Sotr_ID = '" + sbor_Docum.Sotr_ID + "', " +
                        "Itog = 0, " +
                        "Photo_Doc = Photo_Doc " +
                        "where ID_Sbora = " + sbor_Docum.ID_Sbora.ToString(),
                        Program.SqlConnection);
                    Program.SqlConnection.Open();
                    command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    return RedirectToAction("Index");
                }
            
        }

        public string upload(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/Photo/dok/"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch 
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }
            return path;
        }

        // GET: Sbor_Docum/Delete/5
        [Authorize]
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sbor_Docum sbor_Docum = db.Sbor_Docum.Find(id);
            SqlCommand command;
            command = new SqlCommand(
                            "update Sbor_Docum " +
                            "set " +
                            "Itog = 0, " +
                            "Photo_Doc = '' " +
                            "where ID_Sbora = " + sbor_Docum.ID_Sbora.ToString(),
                            Program.SqlConnection);
            Program.SqlConnection.Open();
            command.ExecuteScalar();
            Program.SqlConnection.Close();
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
