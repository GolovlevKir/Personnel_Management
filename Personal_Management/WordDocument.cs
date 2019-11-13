using Microsoft.Office.Interop.Word;
using System;
using System.Data;
using System.Data.SqlClient;
using word = Microsoft.Office.Interop.Word;


namespace Personal_Management
{
    public class WordDocument
    {
        SqlCommand command = new SqlCommand("", Program.SqlConnection);
        public static System.Data.DataTable table = new System.Data.DataTable();
        public static System.Data.DataTable table2 = new System.Data.DataTable();
        public static string file_name;
        public static void PrihOtch()
        {
            word.Application application = new word.Application();
            word.Document document = application.Documents.Add(Visible: true);
            word.Range range = document.Range(0, 0);
            file_name = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\СпДолж_" + "Список должностей"
                + DateTime.Now.ToString("_hh_mm_ss_dd_MM_yyyy") + ".docx";
            try
            {
                document.Sections.PageSetup.LeftMargin
                    = application.CentimetersToPoints(Convert.ToSingle(2.5));
                document.Sections.PageSetup.RightMargin
                    = application.CentimetersToPoints(Convert.ToSingle(1));
                document.Sections.PageSetup.TopMargin
                    = application.CentimetersToPoints(Convert.ToSingle(2));
                document.Sections.PageSetup.BottomMargin
                    = application.CentimetersToPoints(Convert.ToSingle(1.5));
                range.Text = "Служба технической поддержки IT Liga";
                range.ParagraphFormat.Alignment
                    = word.WdParagraphAlignment.wdAlignParagraphCenter;
                range.ParagraphFormat.SpaceAfter = 1;
                range.ParagraphFormat.SpaceBefore = 1;
                range.ParagraphFormat.LineSpacingRule = word.WdLineSpacing.wdLineSpaceSingle;
                range.Font.Name = "Times New Roman";
                range.Font.Size = 16;
                document.Paragraphs.Add();
                document.Paragraphs.Add();
                word.Paragraph Name_Doc = document.Paragraphs.Add();
                Name_Doc.Format.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                Name_Doc.Range.Font.Name = "Times New Roman";
                Name_Doc.Range.Font.Size = 14;
                Name_Doc.Range.Text = "Список должностей";
                document.Paragraphs.Add();
                document.Paragraphs.Add();
                foreach (DataRow row in table.Rows)
                {
                    DataBaseTables data = new DataBaseTables();
                    data.qrPositions += " where Naim_Depart = '"+ row["Naim_Depart"].ToString() + "'";
                    data.dtPositFill();
                    table2 = data.dtPositions;
                    Name_Doc.Range.Font.Name = "Times New Roman";
                    Name_Doc.Range.Text = "Отдел: " + row["Naim_Depart"].ToString();
                    document.Paragraphs.Add();
                    word.Paragraph pTable = document.Paragraphs.Add();
                    word.Table tbDanTab = document.Tables.Add(pTable.Range, table2.Rows.Count + 1,
                        table2.Columns.Count);
                    tbDanTab.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                    tbDanTab.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                    tbDanTab.Cell(1, 1).Range.Text = "Наименование должности";
                    tbDanTab.Cell(1, 2).Range.Text = "Оклад";
                    tbDanTab.Range.Font.Size = 12;
                    tbDanTab.Range.Font.Name = "Times New Roman";
                    tbDanTab.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
                    tbDanTab.Columns[1].Width = 250;
                    tbDanTab.Columns[2].Width = 150;
                    for (int i = 2; i <= tbDanTab.Rows.Count; i++)
                        for (int j = 1; j <= tbDanTab.Columns.Count; j++)
                        {
                            tbDanTab.Cell(i, j).Range.Text
                                = table2.Rows[i - 2][j - 1].ToString();
                        }
                    document.Paragraphs.Add();
                }
                document.Paragraphs.Add();
                Name_Doc.Range.Font.Name = "Times New Roman";
                Name_Doc.Range.Font.Size = 14;
                Name_Doc.Format.Alignment = word.WdParagraphAlignment.wdAlignParagraphRight;
                Name_Doc.Range.Text = "Руководитель отдела кадров ____________ (_________________)";
                document.Paragraphs.Add();
                Name_Doc.Range.Text = "Менеджер по персоналу ____________ (_________________)";
                document.Paragraphs.Add();
                Name_Doc.Range.Text = "Генеральный директор ____________ (_________________)";
                document.Paragraphs.Add();
                Name_Doc.Range.Text = DateTime.Now.ToLongDateString();
            }
            catch
            {
               
            }
            finally
            {
                document.SaveAs2(file_name, word.WdSaveFormat.wdFormatDocumentDefault);
                document.Close();
                application.Quit();
            }
        }

        public static void PrihZaDen()
        {
                    DataBaseTables data = new DataBaseTables();
                    data.dtDepFill();
                    table = data.dtDepartments;
                    PrihOtch();
        }
    }
}