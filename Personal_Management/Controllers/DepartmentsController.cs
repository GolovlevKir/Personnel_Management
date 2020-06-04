using Personal_Management.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Personal_Management.Controllers

{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private PersonalContext db = new PersonalContext();

        public ActionResult Index()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Если заход с мобильного устройства
                if (Request.Browser.IsMobileDevice)
                {
                    ViewBag.mob = 1;
                }
                else
                {
                    ViewBag.mob = 0;
                }
                //Проверка испытательных сроков
                Program.update();
                //Список отделов
                return View(db.Departments.ToList());
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public ActionResult Details(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Получение подробной информации по отделу
                if (id == null)
                {
                    //Неверный запрос
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск отдела по ключу
                Departments departments = db.Departments.Find(id);
                if (departments == null)
                {
                    //Страница не найдена
                    return HttpNotFound();
                }
                return View(departments);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public ActionResult Create()
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Представление
                return View();
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Depart,Naim_Depart")] Departments departments)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Добавление команды
                SqlCommand command = new SqlCommand("", Program.SqlConnection);
                //Поиск отделов с таким же наименованием
                command.CommandText = "Select count(*) from Departments where Naim_Depart = '" + departments.Naim_Depart + "'";
                //Открытие подключения
                Program.SqlConnection.Open();
                //Выполнение команды
                int co = (int)command.ExecuteScalar();
                //Закрытие подключения
                Program.SqlConnection.Close();
                if (co == 0)
                {
                    //Если валидация успешно прошла
                    if (ModelState.IsValid)
                    {
                        //Добавление новой записи
                        db.Departments.Add(departments);
                        //Сохранение
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //Вывод сообщения, что отдел такой уже существует
                    ViewBag.m = "Такой отдел уже существует!";
                }

                return View(departments);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public ActionResult Edit(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //Неверный запрос
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Departments departments = db.Departments.Find(id);
                if (departments == null)
                {
                    //404 ошибка
                    return HttpNotFound();
                }
                return View(departments);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Depart,Naim_Depart")] Departments departments)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (ModelState.IsValid)
                {
                    //Изменение данных
                    db.Entry(departments).State = EntityState.Modified;
                    //Сохранение 
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(departments);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        public ActionResult Delete(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //Неверный запрос
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Departments departments = db.Departments.Find(id);
                if (departments == null)
                {
                    return HttpNotFound();
                }
                return View(departments);
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id )
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Удаление данных
                Departments departments = db.Departments.Find(id);
                db.Departments.Remove(departments);
                //Сохранение 
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //ЕСЛИ ОТЧЕТ НЕОБХОДИМО СФОРМИРОВАТЬ С ПОМОЩЬЮ ITEXTSHARP
        //public static string file_name;
        //public static System.Data.DataTable table = new System.Data.DataTable();
        //public static System.Data.DataTable table2 = new System.Data.DataTable();
        //[HttpGet]
        //public FileResult Generar()
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        //Initialize the PDF document object.
        //        using (Document pdfDoc = new Document())
        //        {
        //            BaseFont baseFont = BaseFont.CreateFont(Server.MapPath("~/Content/Photo/") + "/times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 21, iTextSharp.text.Font.BOLD);
        //            iTextSharp.text.Font font1 = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);
        //            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //            pdfDoc.Open();

        //            //Add the Image file to the PDF document object.
        //            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Content/Photo/") + "logo.png");
        //            img.Alignment = Element.ALIGN_RIGHT;
        //            img.ScaleToFit(300, 250);
        //            pdfDoc.Add(img);

        //            String phrase = "Данные по отделам";
        //            Paragraph elements = new Paragraph(phrase, font);
        //            elements.Alignment = Element.ALIGN_CENTER;
        //            pdfDoc.Add(elements);
        //            pdfDoc.Add(new Paragraph(" "));
        //            DataBaseTables data1 = new DataBaseTables();
        //            data1.dtDepFill();
        //            table = data1.dtDepartments;
        //            foreach (DataRow row in table.Rows)
        //            {
        //                DataBaseTables data = new DataBaseTables();
        //                data.qrPositions = "Select Naim_Posit as 'Наименование должности', Salary as 'Оклад' from Positions join Departments on ID_Depart = Depart_ID where Naim_Depart = '" + row["Naim_Depart"].ToString() + "'";
        //                data.dtPositFill();
        //                table2 = data.dtPositions;
        //                String phras = "Отдел: " + row["Naim_Depart"].ToString();
        //                Paragraph el = new Paragraph(phras, font1);
        //                el.Alignment = Element.ALIGN_LEFT;
        //                pdfDoc.Add(el);

        //                PdfPTable tab = new PdfPTable(table2.Columns.Count);
        //                tab.SpacingAfter = 20;
        //                PdfPCell cell = new PdfPCell();
        //                cell.Colspan = table2.Columns.Count;
        //                cell.HorizontalAlignment = 1;
        //                tab.AddCell(cell);
        //                for (int j = 0; j < table2.Columns.Count; j++)
        //                {
        //                    cell = new PdfPCell(new Phrase(new Phrase(table2.Columns[j].ColumnName, font1)));
        //                    //Фоновый цвет (необязательно, просто сделаем по красивее)
        //                    cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
        //                    tab.AddCell(cell);
        //                }
        //                for (int j = 0; j < table2.Rows.Count; j++)
        //                {
        //                    for (int k = 0; k < table2.Columns.Count; k++)
        //                    {
        //                        tab.AddCell(new Phrase(table2.Rows[j][k].ToString(), font1));
        //                    }
        //                }
        //                //Добавляем таблицу в документ
        //                pdfDoc.Add(new Paragraph(" "));
        //                pdfDoc.Add(tab);

        //            }
        //            pdfDoc.Close();
        //            //Download the PDF file.
        //            return File(stream.ToArray(), "application/pdf", "ДО_Данные_По_Отделам" + DateTime.Now.ToString("_hh_mm_ss_dd_MM_yyyy") + ".pdf");
        //        }
        //    }
        //}

        public async Task<ActionResult> DeleteLogic(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Departments dep = await db.Departments.FindAsync(id);
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


        [HttpPost, ActionName("DeleteLogic")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedLog(int id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Осуществление логического удаления
                Departments pos = await db.Departments.FindAsync(id);
                pos.Logical_Delete = true;
                db.Entry(pos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Error/NotRight");
            }
        }

        public async Task<ActionResult> VozvLogic(int? id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                if (id == null)
                {
                    //400 ошибка
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Поиск по ключу
                Departments dep = await db.Departments.FindAsync(id);
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


        [HttpPost, ActionName("VozvLogic")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VozvLogic(int id)
        {
            if ((bool)Session["Manip_Sotrs"] == true && Session["Manip_Sotrs"] != null)
            {
                //Осуществление возврата логического удаления
                Departments pos = await db.Departments.FindAsync(id);
                pos.Logical_Delete = false;
                db.Entry(pos).State = EntityState.Modified;
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
