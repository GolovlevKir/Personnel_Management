using Personal_Management.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //Получение аккаунтов (Незаблокированных)
                var accounts = db.Accounts.Include(a => a.Roles).Where(a => a.Block == false);
                return View(await accounts.ToListAsync());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }

        }

        public ActionResult Index2()
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //Получение аккаунтов (Заблокированных)
                var accounts = db.Accounts.Include(a => a.Roles).Where(a => a.Block == true);
                return View(accounts.ToList());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        // Изменение данных аккаунта
        public async Task<ActionResult> Edit(string id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //Если первичный ключ не указан
                if (id == null)
                {
                    //Неверный запрос (Ошибка 400)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Получение данных аккаунта по первичному ключу
                Accounts accounts = await db.Accounts.FindAsync(id);
                //Если пользователи не найдены
                if (accounts == null)
                {
                    return HttpNotFound();
                }
                //Список прав доступа
                ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim", accounts.Role_ID);
                return View(accounts);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Login,Password,Password_Shifr,Role_ID,Block,Logical_Delete")] Accounts accounts)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //Если валидация прошла
                if (ModelState.IsValid)
                {
                    //Зафиксировать изменения
                    db.Entry(accounts).State = EntityState.Modified;
                    //Сохранить данные
                    await db.SaveChangesAsync();
                    //Показать страницу со всеми данными
                    return RedirectToAction("Index");
                }
                //Список прав доступа
                ViewBag.Role_ID = new SelectList(db.Roles, "ID_Role", "Role_Naim", accounts.Role_ID);
                //Вернуть данные для корректировки
                return View(accounts);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public async Task<ActionResult> Delete(string id )
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //Ели первичный ключ пуст
                if (id == null)
                {
                    //Вывод ошибки 400
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск аккаунта по логину 
                Accounts accounts = await db.Accounts.FindAsync(id);
                if (accounts == null)
                {
                    return HttpNotFound();
                }
                return View(accounts);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //Поиск аккаунта по логину
                Accounts accounts = await db.Accounts.FindAsync(id);
                //Удаление данных
                db.Accounts.Remove(accounts);
                //Сохранение данных
                await db.SaveChangesAsync();
                //Переадресация
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        //Служит для очистки
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> BlockAcc(string id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                if (id == null)
                {
                    //Неверный запрос
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск аккаунта по логину
                Accounts dep = await db.Accounts.FindAsync(id);
                if (dep == null)
                {
                    //Страница не найдена
                    return HttpNotFound();
                }
                return View(dep);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost, ActionName("BlockAcc")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BlockAcco(string id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                Accounts pos = await db.Accounts.FindAsync(id);
                //Значение блокировки
                pos.Block = true;
                //Изменение данных аккаунта
                db.Entry(pos).State = EntityState.Modified;
                //Сохранение
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public async Task<ActionResult> UnlockAcc(string id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                if (id == null)
                {
                    //Неверный запрос
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по логину
                Accounts dep = await db.Accounts.FindAsync(id);
                if (dep == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(dep);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost, ActionName("UnlockAcc")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnlockAcco(string id)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                Accounts pos = await db.Accounts.FindAsync(id);
                //Разблокировака аккаунта
                pos.Block = false;
                //Изменение
                db.Entry(pos).State = EntityState.Modified;
                //Сохранение
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

    }
}
