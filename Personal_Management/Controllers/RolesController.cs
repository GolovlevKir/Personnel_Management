using Personal_Management.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //полчение списка прав доступа
                return View(await db.Roles.ToListAsync());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public async Task<ActionResult> Details(int? id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //получение по ключу
                Roles roles = await db.Roles.FindAsync(id);
                if (roles == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(roles);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public ActionResult Create()
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID_Role,Role_Naim,Manip_Roles,Manip_Sotrs,Manip_Department,Buh_Ych,Logical_Delete")] Roles roles)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Добавление данных
                    db.Roles.Add(roles);
                    //Сохранение
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(roles);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Roles roles = await db.Roles.FindAsync(id);
                if (roles == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(roles);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID_Role,Role_Naim,Manip_Roles,Manip_Sotrs,Manip_Department,Manip_Tests,Logical_Delete")] Roles roles)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(roles).State = EntityState.Modified;
                    //сохранение
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(roles);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //поиск по ключу
                Roles roles = await db.Roles.FindAsync(id);
                if (roles == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(roles);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                Roles roles = await db.Roles.FindAsync(id);
                //Удаление строки
                db.Roles.Remove(roles);
                //Сохранение
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        //Очистка мусора
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
