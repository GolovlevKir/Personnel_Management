using Personal_Management.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class status_isp_srokaController : Controller
    {

        private PersonalContext db = new PersonalContext();


        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Список статусов
                return View(await db.status_isp_sroka.ToListAsync());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //поиск по ключу
                status_isp_sroka status_isp_sroka = await db.status_isp_sroka.FindAsync(id);
                if (status_isp_sroka == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(status_isp_sroka);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID_St,Name_St")] status_isp_sroka status_isp_sroka)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если валидация прошла успешно
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(status_isp_sroka).State = EntityState.Modified;
                    //Сохранение 
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(status_isp_sroka);
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
