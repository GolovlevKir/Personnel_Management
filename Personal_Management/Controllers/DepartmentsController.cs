using iTextSharp.text;
using iTextSharp.text.pdf;
using Personal_Management.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Personal_Management.Controllers

{
    public class DepartmentsController : Controller
    {
        private PersonalContext db = new PersonalContext();


        // GET: Departments
        [Authorize]
        public ActionResult Index()
        {
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
            return View(db.Departments.ToList());
        }

        // GET: Departments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            //Получение подробной информации по отделу
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departments departments = db.Departments.Find(id);
            if (departments == null)
            {
                return HttpNotFound();
            }
            return View(departments);
        }

        // GET: Departments/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Depart,Naim_Depart")] Departments departments)
        {
            SqlCommand command = new SqlCommand("", Program.SqlConnection);
            command.CommandText = "Select count(*) from Departments where Naim_Depart = '" + departments.Naim_Depart + "'";
            Program.SqlConnection.Open();
            int co = (int)command.ExecuteScalar();
            Program.SqlConnection.Close();
            if (co == 0)
            {
                if (ModelState.IsValid)
                {
                    //Добавление новой записи
                    db.Departments.Add(departments);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.m = "Такой отдел уже существует!";
            }

            return View(departments);
        }

        // GET: Departments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departments departments = db.Departments.Find(id);
            if (departments == null)
            {
                return HttpNotFound();
            }
            return View(departments);
        }

        // POST: Departments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Depart,Naim_Depart")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                //Изменение данных
                db.Entry(departments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departments);
        }

        // GET: Departments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departments departments = db.Departments.Find(id);
            if (departments == null)
            {
                return HttpNotFound();
            }
            return View(departments);
        }

        // POST: Departments/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Удаление данных
            Departments departments = db.Departments.Find(id);
            db.Departments.Remove(departments);
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


        public static string file_name;
        public static System.Data.DataTable table = new System.Data.DataTable();
        public static System.Data.DataTable table2 = new System.Data.DataTable();
        [HttpGet]
        public FileResult Generar()
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                //Initialize the PDF document object.
                using (Document pdfDoc = new Document())
                {
                    BaseFont baseFont = BaseFont.CreateFont(Server.MapPath("~/Content/Photo/") + "/times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 21, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font font1 = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    //Add the Image file to the PDF document object.
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Content/Photo/") + "logo.png");
                    img.Alignment = Element.ALIGN_RIGHT;
                    img.ScaleToFit(300, 250);
                    pdfDoc.Add(img);

                    String phrase = "Данные по отделам";
                    Paragraph elements = new Paragraph(phrase, font);
                    elements.Alignment = Element.ALIGN_CENTER;
                    pdfDoc.Add(elements);
                    pdfDoc.Add(new Paragraph(" "));
                    DataBaseTables data1 = new DataBaseTables();
                    data1.dtDepFill();
                    table = data1.dtDepartments;
                    foreach (DataRow row in table.Rows)
                    {
                        DataBaseTables data = new DataBaseTables();
                        data.qrPositions = "Select Naim_Posit as 'Наименование должности', Salary as 'Оклад' from Positions join Departments on ID_Depart = Depart_ID where Naim_Depart = '" + row["Naim_Depart"].ToString() + "'";
                        data.dtPositFill();
                        table2 = data.dtPositions;
                        String phras = "Отдел: " + row["Naim_Depart"].ToString();
                        Paragraph el = new Paragraph(phras, font1);
                        el.Alignment = Element.ALIGN_LEFT;
                        pdfDoc.Add(el);

                        PdfPTable tab = new PdfPTable(table2.Columns.Count);
                        tab.SpacingAfter = 20;
                        PdfPCell cell = new PdfPCell();
                        cell.Colspan = table2.Columns.Count;
                        cell.HorizontalAlignment = 1;
                        tab.AddCell(cell);
                        for (int j = 0; j < table2.Columns.Count; j++)
                        {
                            cell = new PdfPCell(new Phrase(new Phrase(table2.Columns[j].ColumnName, font1)));
                            //Фоновый цвет (необязательно, просто сделаем по красивее)
                            cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                            tab.AddCell(cell);
                        }
                        for (int j = 0; j < table2.Rows.Count; j++)
                        {
                            for (int k = 0; k < table2.Columns.Count; k++)
                            {
                                tab.AddCell(new Phrase(table2.Rows[j][k].ToString(), font1));
                            }
                        }
                        //Добавляем таблицу в документ
                        pdfDoc.Add(new Paragraph(" "));
                        pdfDoc.Add(tab);

                    }
                    pdfDoc.Close();
                    //Download the PDF file.
                    return File(stream.ToArray(), "application/pdf", "ДО_Данные_По_Отделам" + DateTime.Now.ToString("_hh_mm_ss_dd_MM_yyyy") + ".pdf");
                }
            }

            //document.Sections.PageSetup.LeftMargin
            //    = application.CentimetersToPoints(Convert.ToSingle(2.5));
            //document.Sections.PageSetup.RightMargin
            //    = application.CentimetersToPoints(Convert.ToSingle(1));
            //document.Sections.PageSetup.TopMargin
            //    = application.CentimetersToPoints(Convert.ToSingle(2));
            //document.Sections.PageSetup.BottomMargin
            //    = application.CentimetersToPoints(Convert.ToSingle(1.5));
            //range.Text = "Служба технической поддержки IT Liga";
            //range.ParagraphFormat.Alignment
            //    = word.WdParagraphAlignment.wdAlignParagraphCenter;
            //range.ParagraphFormat.SpaceAfter = 1;
            //range.ParagraphFormat.SpaceBefore = 1;
            //range.ParagraphFormat.LineSpacingRule = word.WdLineSpacing.wdLineSpaceSingle;
            //range.Font.Name = "Times New Roman";
            //range.Font.Size = 16;
            //document.Paragraphs.Add();
            //document.Paragraphs.Add();
            //word.Paragraph Name_Doc = document.Paragraphs.Add();
            //Name_Doc.Format.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
            //Name_Doc.Range.Font.Name = "Times New Roman";
            //Name_Doc.Range.Font.Size = 14;
            //Name_Doc.Range.Text = "Список должностей";
            //document.Paragraphs.Add();
            //document.Paragraphs.Add();
            //DataBaseTables data1 = new DataBaseTables();
            //data1.dtDepFill();
            //table = data1.dtDepartments;
            //foreach (DataRow row in table.Rows)
            //{
            //    DataBaseTables data = new DataBaseTables();
            //    data.qrPositions = "Select Naim_Posit, Salary from Positions join Departments on ID_Depart = Depart_ID where Naim_Depart = '" + row["Naim_Depart"].ToString() + "'";
            //    data.dtPositFill();
            //    table2 = data.dtPositions;
            //    Name_Doc.Range.Font.Name = "Times New Roman";
            //    Name_Doc.Range.Text = "Отдел: " + row["Naim_Depart"].ToString();
            //    document.Paragraphs.Add();
            //    word.Paragraph pTable = document.Paragraphs.Add();
            //    word.Table tbDanTab = document.Tables.Add(pTable.Range, table2.Rows.Count + 1,
            //        table2.Columns.Count);
            //    tbDanTab.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
            //    tbDanTab.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
            //    tbDanTab.Cell(1, 1).Range.Text = "Наименование должности";
            //    tbDanTab.Cell(1, 2).Range.Text = "Оклад";
            //    tbDanTab.Range.Font.Size = 12;
            //    tbDanTab.Range.Font.Name = "Times New Roman";
            //    tbDanTab.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            //    tbDanTab.Columns[1].Width = 250;
            //    tbDanTab.Columns[2].Width = 150;
            //    for (int i = 2; i <= tbDanTab.Rows.Count; i++)
            //        for (int j = 1; j <= tbDanTab.Columns.Count; j++)
            //        {
            //            tbDanTab.Cell(i, j).Range.Text
            //                = table2.Rows[i - 2][j - 1].ToString();
            //        }
            //    document.Paragraphs.Add();
            //}
            //document.Paragraphs.Add();
            //Name_Doc.Range.Font.Name = "Times New Roman";
            //Name_Doc.Range.Font.Size = 14;
            //Name_Doc.Format.Alignment = word.WdParagraphAlignment.wdAlignParagraphRight;
            //Name_Doc.Range.Text = "Руководитель отдела кадров ____________ (_________________)";
            //document.Paragraphs.Add();
            //Name_Doc.Range.Text = "Менеджер по персоналу ____________ (_________________)";
            //document.Paragraphs.Add();
            //Name_Doc.Range.Text = "Генеральный директор ____________ (_________________)";
            //document.Paragraphs.Add();
            //Name_Doc.Range.Text = DateTime.Now.ToLongDateString();
            //document.SaveAs2(file_name, word.WdSaveFormat.wdFormatDocumentDefault);
            //document.Close();
            //application.Quit();
            //Скачка файла

        }

    }
}
