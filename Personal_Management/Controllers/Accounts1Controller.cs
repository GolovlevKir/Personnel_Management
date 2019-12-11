using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Personal_Management.Models;

namespace Personal_Management.Controllers
{
    public class Accounts1Controller : Controller
    {
        private PersonalContext db = new PersonalContext();

        // GET: Accounts1
        public async Task<ActionResult> Index()
        {
            var accounts = db.Accounts.Include(a => a.Roles).Include(a => a.Sotrs);
            return View(await accounts.ToListAsync());
        }

        // GET: Accounts1/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = await db.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // GET: Accounts1/Create
        public ActionResult Create()
        {
            ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim");
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot");
            return View();
        }

        // POST: Accounts1/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Login,Password,Role_ID,Sotr_ID,Password2")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(accounts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim", accounts.Role_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", accounts.Sotr_ID);
            return View(accounts);
        }

        // GET: Accounts1/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = await db.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim", accounts.Role_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", accounts.Sotr_ID);
            return View(accounts);
        }

        // POST: Accounts1/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Login,Password,Role_ID,Sotr_ID,Password2")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accounts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim", accounts.Role_ID);
            ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", accounts.Sotr_ID);
            return View(accounts);
        }

        // GET: Accounts1/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accounts accounts = await db.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return HttpNotFound();
            }
            return View(accounts);
        }

        // POST: Accounts1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Accounts accounts = await db.Accounts.FindAsync(id);
            db.Accounts.Remove(accounts);
            await db.SaveChangesAsync();
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
