﻿using System;
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
    public class Work_ScheduleController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Work_Schedule
        public ActionResult Index()
        {
            return View(db.Work_Schedule.ToList());
        }

        // GET: Work_Schedule/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work_Schedule work_Schedule = db.Work_Schedule.Find(id);
            if (work_Schedule == null)
            {
                return HttpNotFound();
            }
            return View(work_Schedule);
        }

        // GET: Work_Schedule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Work_Schedule/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Schedule,Naim_Sche")] Work_Schedule work_Schedule)
        {
            if (ModelState.IsValid)
            {
                db.Work_Schedule.Add(work_Schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(work_Schedule);
        }

        // GET: Work_Schedule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work_Schedule work_Schedule = db.Work_Schedule.Find(id);
            if (work_Schedule == null)
            {
                return HttpNotFound();
            }
            return View(work_Schedule);
        }

        // POST: Work_Schedule/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Schedule,Naim_Sche")] Work_Schedule work_Schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work_Schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(work_Schedule);
        }

        // GET: Work_Schedule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work_Schedule work_Schedule = db.Work_Schedule.Find(id);
            if (work_Schedule == null)
            {
                return HttpNotFound();
            }
            return View(work_Schedule);
        }

        // POST: Work_Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work_Schedule work_Schedule = db.Work_Schedule.Find(id);
            db.Work_Schedule.Remove(work_Schedule);
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