using Personal_Management.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    public class AccountsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Accounts
        [Authorize]
        public ActionResult Index(string search)
        {
            //Проверка на испытательные сроки
            Program.update();
            //Связь с сотрудниками и ролями
            var accounts = db.Accounts.Include(a => a.Roles).Include(a => a.Sotrs);
            ViewBag.seo = search;
            //Поиск
            if (search != null && search != "")
            {
                accounts = accounts.Where(s => (s.Login.Contains(search)) || (s.Sotrs.Surname_Sot.Contains(search)) || (s.Sotrs.Name_Sot.Contains(search)) || (s.Sotrs.Petronumic_Sot.Contains(search)) || (s.Sotrs.Positions.Naim_Posit.Contains(search)));
            }
            return View(accounts.ToList());
        }

        // GET: Accounts/Create
        [Authorize]
        public ActionResult Create()
        {
            int selectedIndex = 0;
            //Получаем наименования ролей
            ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim");
            //Создаем лист должностей
            List<Positions> pos = db.Positions.ToList();
            //Добавляем все
            pos.Insert(0, new Positions { ID_Positions = 0, Naim_Posit = "Все" });
            //Создаем список должностей
            ViewBag.Positions = new SelectList(pos, "ID_Positions", "Naim_Posit", selectedIndex);
            if (selectedIndex > 0)
            {
                ViewBag.Sotr_ID = new SelectList(db.Sotrs.Where(p => p.Positions_ID == selectedIndex), "ID_Sotr", "Full");
            }
            else
            {
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full");
            }


            return View();
        }

        public ActionResult GetItems(int id)
        {
            //Получаем список сотрудников
            if (id > 0)
            {
                return PartialView(db.Sotrs.Where(c => c.Positions_ID == id).ToList());
            }
            else
            {
                return PartialView(db.Sotrs);
            }
                
        }

        // POST: Accounts/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Login,Password,Role_ID,Sotr_ID,Password2")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                //Создаем новый аккаунт
                accounts.Password2 = accounts.Password;
                accounts.Password = Program.Hash(accounts.Password);
                db.Accounts.Add(accounts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim", accounts.Role_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", accounts.Sotr_ID);
            return View(accounts);
        }

        // GET: Accounts/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Accounts.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            //Получем список ролей и сотрудников
            ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim", accounts.Role_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", accounts.Sotr_ID);
            return View(accounts);
        }

        // POST: Accounts/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                accounts.Password = Program.Hash(accounts.Password2);
                db.Entry(accounts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim", accounts.Role_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Full", accounts.Sotr_ID);
            return View(accounts);
        }

        // GET: Accounts/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = db.Accounts.Find(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // POST: Accounts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //Удаление данных
            Accounts accounts = db.Accounts.Find(id);
            db.Accounts.Remove(accounts);
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
