using Personal_Management.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class StepsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        //Список этапов принятия 
        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение этапов принятия
                var steps = db.Steps.Include(s => s.Sotrs);
                return View(await steps.ToListAsync());
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
                Steps steps = await db.Steps.FindAsync(id);
                //Список сотрудников
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot");
                if (steps == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(steps);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Steps steps)
        {
            if ((bool)Session["Manip_Roles"] == true && Session["Manip_Roles"] != null)
            {
                //если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(steps).State = EntityState.Modified;
                    //сохранение
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.Sotr_ID = new SelectList(db.Sotrs, "ID_Sotr", "Surname_Sot", steps.Sotr_ID);
                return View(steps);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

    }
}
