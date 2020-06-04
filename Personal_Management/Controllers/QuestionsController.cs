using Personal_Management.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //Список вопросов
                return View(await db.Questions.ToListAsync());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public async Task<ActionResult> Details(int? id)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по коду
                Questions questions = await db.Questions.FindAsync(id);
                if (questions == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(questions);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public ActionResult Create()
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
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
        public async Task<ActionResult> Create([Bind(Include = "ID_Quest,Quest_Naim")] Questions questions)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Добавление данных
                    db.Questions.Add(questions);
                    //Сохранение
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(questions);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по коду
                Questions questions = await db.Questions.FindAsync(id);
                if (questions == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(questions);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID_Quest,Quest_Naim")] Questions questions)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                //если валидация пройдена успешно
                if (ModelState.IsValid)
                {
                    //Сохранение измененных данных
                    db.Entry(questions).State = EntityState.Modified;
                    //Сохранениние
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(questions);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Получение по ключу
                Questions questions = await db.Questions.FindAsync(id);
                if (questions == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(questions);
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
            if ((bool)Session["Manip_Tests"] == true && Session["Manip_Tests"] != null)
            {
                Questions questions = await db.Questions.FindAsync(id);
                //удаление строки
                db.Questions.Remove(questions);
                //Сохранение данных
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
