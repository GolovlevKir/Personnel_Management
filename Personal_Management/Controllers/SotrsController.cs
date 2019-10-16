using Personal_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class SotrsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Sotrs
        public ActionResult Index(int? pos)
        {
            IQueryable<Sotrs> sotrs = db.Sotrs.Include(s => s.Positions).Include(s => s.Rates).Include(s => s.Work_Schedule);
            if (pos != null && pos != 0)
            {
                sotrs = sotrs.Where(p => p.Positions_ID == pos);
            }
            List<Positions> posit = db.Positions.ToList();
            posit.Insert(0, new Positions { Naim_Posit = "Все", ID_Positions = 0 });
            SotrsListViewModel plvm = new SotrsListViewModel {
                Sotrs = sotrs.ToList(),
                Positions = new SelectList(posit, "ID_Positions", "Naim_Posit")
            };


            return View(plvm);
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

        [HttpGet]
        // GET: Sotrs/Create
        public ActionResult Create()
        {
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit");
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate");
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche");
            return View();
        }

        [HttpGet]
        public ActionResult addnewrecord()
        {
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit");
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate");
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche");
            return View();
        }

        [HttpPost]
        public ActionResult addnewrecord(Sotrs sotrs, HttpPostedFileBase imgfile)
        {
            Sotrs sot = new Sotrs();
            string path = uploadimage(imgfile);
            if (path.Equals("-1"))
            {

            }
            else
            {
                sot.Surname_Sot = sotrs.Surname_Sot;
                sot.Name_Sot = sotrs.Name_Sot;
                sot.Petronumic_Sot = sotrs.Petronumic_Sot;
                sot.Day_Of_Birth = sotrs.Day_Of_Birth;
                sot.Address = sotrs.Address;
                sot.Num_Phone = sotrs.Num_Phone;
                sot.Email = sotrs.Email;
                sot.Photo = path;
                sot.Positions_ID = sotrs.Positions_ID;
                sot.Rate_ID = sotrs.Rate_ID;
                sot.Schedule_ID = sotrs.Schedule_ID;
                sot.Date_of_adoption = sotrs.Date_of_adoption;
                sot.Opisanie = sotrs.Opisanie;
                db.Sotrs.Add(sot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public string uploadimage(HttpPostedFileBase file)
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
                        path = Path.Combine(Server.MapPath("~/Content/Photo/Sotrs/"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }
            return path;
        }

        [HttpGet]
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
        public ActionResult Edit(Sotrs sotrs, HttpPostedFileBase imgfile)
        {
            string path = uploadimage(imgfile);
            Response.Write("<script>alert('"+ path + "'); </script>");
            if (path.Equals("-1"))
            {

            }
            else
            {
                SqlCommand command = new SqlCommand(
                    "update Sotrs " +
                    "set " +
                    "Surname_Sot = '" + sotrs.Surname_Sot + "', "+
                    "Name_Sot = '" + sotrs.Name_Sot + "', " +
                    "Petronumic_Sot = '" + sotrs.Petronumic_Sot + "', " +
                    "Day_Of_Birth = '" + sotrs.Day_Of_Birth + "', " +
                    "Address = '" + sotrs.Address + "'," +
                    "Num_Phone = '" + sotrs.Num_Phone + "', " +
                    "Email = '" + sotrs.Email + "', " +
                    "Positions_ID = " + sotrs.Positions_ID + ", " +
                    "Rate_ID = " + sotrs.Rate_ID + "," +
                    "Schedule_ID = " + sotrs.Schedule_ID + ", " +
                    "Date_of_adoption = '" + sotrs.Date_of_adoption + "', " +
                    "Opisanie = '" + sotrs.Opisanie + "'," +
                    "Photo = '" + path + "' " +
                    "where ID_Sotr = " + sotrs.ID_Sotr,
                    Program.SqlConnection);
                Program.SqlConnection.Open();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
                //var model = db.Sotrs.Find(sotrs.Positions_ID);
                //string path = uploadimage(imgfile);
                //sotrs.Photo = path;
                //db.SaveChanges();
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
