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
        [Authorize]
        public ActionResult Index(int? pos, int? rate, int? sche, string search)
        {
            //Проверка на испытательные сроки
            Program.update();
            //Фильтрация
            IQueryable<Sotrs> sotrs = db.Sotrs.Include(s => s.Positions).Include(s => s.Rates).Include(s => s.Work_Schedule);
            if (pos != null && pos != 0)
            {
                //Проверка на выбранную должность
                sotrs = sotrs.Where(s => s.Positions_ID == pos);
            }
            if (rate != null && rate != 0)
            {
                //Проверка на выбранную ставку
                sotrs = sotrs.Where(s => s.Rate_ID == rate);
            }
            if (sche != null && sche != 0)
            {
                //Проверка на выбранный график работы
                sotrs = sotrs.Where(s => s.Schedule_ID == sche);
            }
            ViewBag.seo = search;
            //Поиск по значениям
            if (search != null && search != "")
            {
                sotrs = sotrs.Where(s => (s.Surname_Sot.Contains(search)) || (s.Name_Sot.Contains(search)) || (s.Petronumic_Sot.Contains(search)) || (s.Positions.Naim_Posit.Contains(search)) || (s.Num_Phone.Contains(search)) || (s.Opisanie.Contains(search)) || (s.Email.Contains(search)) || (s.Day_Of_Birth.Contains(search)) || s.Date_of_adoption.Contains(search) || s.Surname_Sot.Contains(search) && s.Name_Sot.Contains(search));
            }
            //Лист должностей
            List<Positions> posit = db.Positions.ToList();
            //Добавление значения со значением 0
            posit.Insert(0, new Positions { Naim_Posit = "Все", ID_Positions = 0 });
            //Лист ставок
            List<Rates> rates = db.Rates.ToList();
            //Добавление значения со значением 0
            rates.Insert(0, new Rates { Rate = "Все", ID_Rate = 0 });
            //Лист рабочих графиков
            List<Work_Schedule> sch = db.Work_Schedule.ToList();
            //Добавление значения со значением 0
            sch.Insert(0, new Work_Schedule { Naim_Sche = "Все", ID_Schedule = 0 });
            //Осуществление фильтрации
            SotrsListViewModel plvm = new SotrsListViewModel
            {
                Sotrs = sotrs.ToList(),
                Positions = new SelectList(posit, "ID_Positions", "Naim_Posit"),
                Rates = new SelectList(rates, "ID_Rate", "Rate"),
                Work_Schedule = new SelectList(sch, "ID_Schedule", "Naim_Sche"),
            };
            return View(plvm);
        }

        // GET: Sotrs/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            //Осуществление подробных данных о сотруднике
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

        [Authorize]
        [HttpGet]
        // GET: Sotrs/Create
        public ActionResult Create()
        {
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit");
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate");
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche");
            
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult addnewrecord()
        {
            var m = new Sotrs();
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit");
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate");
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche");
            m.Date_of_adoption = DateTime.Now.ToString("ddMMyyyy");
            return View(m);
        }

        [Authorize]
        [HttpPost]
        public ActionResult addnewrecord(Sotrs sotrs, HttpPostedFileBase imgfile, HttpPostedFileBase doc)
        {
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit");
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate");
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche");
            Sotrs sot = new Sotrs();
            //Загрузка изображения
            string path = uploadimage(imgfile);
            //Загрузка документа
            string pathrez = uploaddoc(doc);
            //Осуществление добавления данных
            try
            { 

                if (pathrez.Equals("-1") && path.Equals("-1"))
                {
                    sot.Surname_Sot = sotrs.Surname_Sot;
                    sot.Name_Sot = sotrs.Name_Sot;
                    sot.Petronumic_Sot = sotrs.Petronumic_Sot;
                    sot.Day_Of_Birth = sotrs.Day_Of_Birth;
                    sot.Address = sotrs.Address;
                    sot.Num_Phone = sotrs.Num_Phone;
                    sot.Email = sotrs.Email;
                    sot.Photo = "-";
                    sot.Positions_ID = sotrs.Positions_ID;
                    sot.Rate_ID = sotrs.Rate_ID;
                    sot.Schedule_ID = sotrs.Schedule_ID;
                    sot.Date_of_adoption = sotrs.Date_of_adoption;
                    sot.Opisanie = sotrs.Opisanie;
                    sot.rezume = "-";
                    db.Sotrs.Add(sot);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (path.Equals("-1"))
                    {
                        sot.Surname_Sot = sotrs.Surname_Sot;
                        sot.Name_Sot = sotrs.Name_Sot;
                        sot.Petronumic_Sot = sotrs.Petronumic_Sot;
                        sot.Day_Of_Birth = sotrs.Day_Of_Birth;
                        sot.Address = sotrs.Address;
                        sot.Num_Phone = sotrs.Num_Phone;
                        sot.Email = sotrs.Email;
                        sot.Photo = "-";
                        sot.Positions_ID = sotrs.Positions_ID;
                        sot.Rate_ID = sotrs.Rate_ID;
                        sot.Schedule_ID = sotrs.Schedule_ID;
                        sot.Date_of_adoption = sotrs.Date_of_adoption;
                        sot.Opisanie = sotrs.Opisanie;
                        sot.rezume = pathrez;
                        db.Sotrs.Add(sot);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (pathrez.Equals("-1"))
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
                            sot.rezume = "-";
                            db.Sotrs.Add(sot);
                            db.SaveChanges();
                            return RedirectToAction("Index");
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
                            sot.rezume = pathrez;
                            db.Sotrs.Add(sot);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Возраст сотрудника должен быть больше 18 лет");
            }
            return View(sotrs);
        }

        public string uploadimage(HttpPostedFileBase file)
        {
            //Переменная с путем
            string path = "-1";
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        //Изменение пути
                        path = Path.Combine(Server.MapPath("~/Content/Photo/st/"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                        //Сохранение 
                        file.SaveAs(path);
                        //Наименование файла
                        path = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    }
                    catch
                    {
                        path = "-1";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Возможны форматы для изображения только: *.jpg/*.jpeg/*.png");
                }
            }
            return path;
        }

        public string uploaddoc(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".doc") || extension.ToLower().Equals(".docx") || extension.ToLower().Equals(".pdf"))
                {
                    try
                    {
                        //Изменение пути
                        path = Path.Combine(Server.MapPath("~/Content/Files/"), DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName));
                        //Сохранение документа
                        file.SaveAs(path);
                        //Наименование файла
                        path = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    }
                    catch
                    {
                        path = "-1";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Форматы резюме только doc ,docx или pdf");
                }
            }
            return path;
        }

        [Authorize]
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
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Sotrs sotrs, HttpPostedFileBase imgfile, HttpPostedFileBase doc)
        {
            ViewBag.Positions_ID = new SelectList(db.Positions, "ID_Positions", "Naim_Posit", sotrs.Positions_ID);
            ViewBag.Rate_ID = new SelectList(db.Rates, "ID_Rate", "Rate", sotrs.Rate_ID);
            ViewBag.Schedule_ID = new SelectList(db.Work_Schedule, "ID_Schedule", "Naim_Sche", sotrs.Schedule_ID);
            SqlCommand command;
            string path = uploadimage(imgfile);
            string pathrez = uploaddoc(doc);
            //Изменение данных
            if (pathrez.Equals("-1") && path.Equals("-1"))
            {
                command = new SqlCommand(
                        "update Sotrs " +
                        "set " +
                        "Surname_Sot = '" + sotrs.Surname_Sot + "', " +
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
                        "Photo = Photo, " +
                        "rezume = rezume " +
                        "where ID_Sotr = " + sotrs.ID_Sotr,
                        Program.SqlConnection);
                Program.SqlConnection.Open();
                command.ExecuteScalar();
                Program.SqlConnection.Close();
                return RedirectToAction("Index");
            }
            else
            {
                if (path.Equals("-1"))
                {
                    command = new SqlCommand(
                        "update Sotrs " +
                        "set " +
                        "Surname_Sot = '" + sotrs.Surname_Sot + "', " +
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
                        "Photo = Photo, " +
                        "rezume = '" + pathrez + "' " +
                        "where ID_Sotr = " + sotrs.ID_Sotr,
                        Program.SqlConnection);
                    Program.SqlConnection.Open();
                    command.ExecuteScalar();
                    Program.SqlConnection.Close();
                    return RedirectToAction("Index");
                }
                else
                {
                    if (pathrez.Equals("-1"))
                    {
                        command = new SqlCommand(
                            "update Sotrs " +
                            "set " +
                            "Surname_Sot = '" + sotrs.Surname_Sot + "', " +
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
                            "Photo = '" + path + "', " +
                            "rezume = rezume " +
                            "where ID_Sotr = " + sotrs.ID_Sotr,
                            Program.SqlConnection);
                        Program.SqlConnection.Open();
                        command.ExecuteScalar();
                        Program.SqlConnection.Close();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        command = new SqlCommand(
                            "update Sotrs " +
                            "set " +
                            "Surname_Sot = '" + sotrs.Surname_Sot + "', " +
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
                            "Photo = '" + path + "', " +
                            "rezume = '" + pathrez + "' " +
                            "where ID_Sotr = " + sotrs.ID_Sotr,
                            Program.SqlConnection);
                        Program.SqlConnection.Open();
                        command.ExecuteScalar();
                        Program.SqlConnection.Close();
                        return RedirectToAction("Index");
                    }
                }
            }

        }

        // GET: Sotrs/Delete/5
        [Authorize]
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
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sotrs sotrs = db.Sotrs.Find(id);
            //Удаление данных
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
