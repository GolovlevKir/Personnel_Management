using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Personal_Management.Models;

namespace Personal_Management.Controllers
{
    [Authorize]
    public class DaysOfWeeksController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public async Task<ActionResult> Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение всех дней недели
                return View(await db.DaysOfWeek.ToListAsync());
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
    }
}
