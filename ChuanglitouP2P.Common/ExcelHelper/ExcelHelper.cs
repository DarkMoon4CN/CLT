using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Aspose.Cells;
using System.IO;
using System.Xml;
using System.Web;
using OfficeOpenXml;

namespace ChuanglitouP2P.Common.ExcelHelper
{
    public class ExcelHelper
    {

        private static Style CreateStyle(Workbook workbooks)
        {
            Style style3 = workbooks.Styles[workbooks.Styles.Add()];//新增样式 
            style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            style3.Font.Name = "宋体";//文字字体 
            style3.Font.Size = 12;//文字大小 
            style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            return style3;
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool SaveExcel(DataTable dt, string fileName)
        {
            Workbook workbooks = new Workbook();
            Worksheet worksheet = workbooks.Worksheets[0];
            Style style = CreateStyle(workbooks);
            try
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    worksheet.Cells[0, j].PutValue(dt.Columns[j].ColumnName);
                    worksheet.Cells[0, j].SetStyle(style);
                }
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        worksheet.Cells[r + 1, c].PutValue(dt.Rows[r][c].ToString());
                        worksheet.Cells[r + 1, c].SetStyle(style);
                    }
                }

                var s = worksheet.Cells[1, 1].GetStyle();
                s.ForegroundColor = System.Drawing.Color.FromArgb(216, 228, 188);
                s.Pattern = BackgroundType.Solid;
                s.Font.IsBold = true;


                Aspose.Cells.Range rang = worksheet.Cells.CreateRange(0, 0, 1, dt.Columns.Count);
                rang.SetStyle(s);
                //Aspose.Cells.Range rang1 = worksheet.Cells.CreateRange(0, 0, dt.Rows.Count, 1);
                //rang1.SetStyle(s);

                worksheet.AutoFitColumns();
                worksheet.FreezePanes(1, 0, dt.Rows.Count, dt.Columns.Count);

                workbooks.Save(fileName);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ds"></param>
        /// <param name="hasTitle"></param>
        public static void LoadExcel(string FileName, DataSet ds, bool hasTitle = false)
        {
            Workbook book = new Workbook(FileName);
            Worksheet sheet = book.Worksheets[0];
            Cells cells = sheet.Cells;
            //获取excel中的数据保存到一个datatable中
            DataTable dt_Import = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, false);
            ds.Tables.Add(dt_Import);
        }


        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ds"></param>
        /// <param name="hasTitle"></param>
        public static void LoadExcelHasTitle(string FileName, DataSet ds)
        {
            Workbook book = new Workbook(FileName);
            Worksheet sheet = book.Worksheets[0];
            Cells cells = sheet.Cells;
            //获取excel中的数据保存到一个datatable中
            DataTable dt_Import = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            ds.Tables.Add(dt_Import);
        }


        public static bool OutExcel2007(DataTable dt, string fileName)
        {
            try
            {
                FileInfo newFile = new FileInfo(fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("ExportData");
                    worksheet.Cells.LoadFromDataTable(dt, true);
                    xlPackage.Save();
                    return true;
                }
            }
            catch (Exception ex) { return false; }
        }

        //public static bool OutExcel2007(IEnumerable<dynamic> data, string fileName)
        //{
        //    try
        //    {
        //        FileInfo newFile = new FileInfo(fileName);
        //        using (ExcelPackage xlPackage = new ExcelPackage(newFile))
        //        {
        //            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("ExportData");
        //            //worksheet.Cells.LoadFromCollection(data, true);
        //            worksheet.Cells["A1"].LoadFromCollection(data, true, OfficeOpenXml.Table.TableStyles.Medium10);
        //            xlPackage.Save();
        //            return true;
        //        }
        //    }
        //    catch (InvalidOperationException iex) { return false; }
        //    catch (Exception ex) { return false; }
        //}
    }
}
