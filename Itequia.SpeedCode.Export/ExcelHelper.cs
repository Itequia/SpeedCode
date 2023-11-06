using ClosedXML.Excel;
using Itequia.SpeedCode.Export.Extensions;
using Itequia.SpeedCode.Export.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
    
namespace Itequia.SpeedCode.Export
{
    public static class ExcelHelpers
    {
        public static string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }

        public static byte[] GenerateExcelFile(string workbookName, IEnumerable<object> items, IEnumerable<string> columnIds, IEnumerable<string> displayNames, ExtractionInfo extractionInfo, out string error)
        {
            error = string.Empty;
            var currentRow = 7;
            using var workbook = CreateWorkbook(currentRow - 1, workbookName, displayNames, out var worksheet);

            try
            {
                
                if (!string.IsNullOrEmpty(extractionInfo.Entity))
                {
                    worksheet.Range("C1", "F1").Merge();
                    worksheet.Cell(1, "C").Style.Font.FontSize = 14;
                    worksheet.Cell(1, "C").Style.Font.SetBold(true);
                    worksheet.Cell(1, "C").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    worksheet.Cell(1, "C").Value = extractionInfo.Entity;
                }

                if (!string.IsNullOrEmpty(extractionInfo.FullUserName))
                {
                    worksheet.Cell(2, "A").Value = extractionInfo.Date;
                    worksheet.Cell(2, "A").Style.DateFormat.Format = "dd/MM/yyyy hh:mm";
                    worksheet.Cell(2, "A").Style.Font.SetBold(true);
                    worksheet.Cell(2, "A").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    worksheet.Cell(2, "B").Value = extractionInfo.FullUserName;
                    worksheet.Cell(2, "B").Style.Font.SetBold(true);

                }

                if (!string.IsNullOrEmpty(extractionInfo.Filters))
                {
                    string letter = ColumnIndexToColumnLetter(displayNames.Count());
                    worksheet.Range("A4", letter + "4").Merge();
                    worksheet.Cell(4, "A").Value = extractionInfo.Filters;
                    worksheet.Cell(4, "A").Style.Font.SetBold(true);
                }

                if (!string.IsNullOrEmpty(extractionInfo.Logo))
                {
                    try
                    {
                        MemoryStream logo = new MemoryStream(ImageHelper.ResizeImage(extractionInfo.Logo, extractionInfo.LogoWidth, extractionInfo.LogoHeight));
                        worksheet.Column(1).Width = extractionInfo.LogoWidth;
                        worksheet.Row(1).Height = extractionInfo.LogoHeight;

                        worksheet.AddPicture(logo).MoveTo(worksheet.Cell("A1"));

                    }
                    catch (Exception ex)
                    {

                    }
                }
                var currentColumn = 1;
                foreach (object accion in items)
                {
                    currentColumn = 1;
                    string labelName = string.Empty;

                    foreach (string column in columnIds)
                    {
                        worksheet.Cell(currentRow, currentColumn).Value = accion.GetPropertyValue(column);

                        if (column.ToLower() == "label") labelName = worksheet.Cell(currentRow, currentColumn).Value.ToString() ?? string.Empty;

                        if (accion.GetPropertyValue(column) != null && (accion.GetPropertyValue(column).GetType().Equals(typeof(double)) || (accion.GetPropertyValue(column).GetType().Equals(typeof(double?)))))
                        {
                            if (worksheet.Cell(currentRow, currentColumn).Value.ToString().Contains(',')) worksheet.Cell(currentRow, currentColumn).Style.NumberFormat.Format = "#,###.##";
                            else worksheet.Cell(currentRow, currentColumn).Style.NumberFormat.Format = "#,###";
                            worksheet.Cell(currentRow, currentColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        }
                        if (accion.GetPropertyValue(column) != null && (accion.GetPropertyValue(column).GetType().Equals(typeof(String)) || accion.GetPropertyValue(column).GetType().Equals(typeof(Int32))))
                        {
                            worksheet.Cell(currentRow, currentColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        }
                        if (accion.GetPropertyValue(column) != null && (accion.GetPropertyValue(column).GetType().Equals(typeof(DateTime)) || accion.GetPropertyValue(column).GetType().Equals(typeof(DateTime?))))
                        {
                            worksheet.Cell(currentRow, currentColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(currentRow, currentColumn).Style.DateFormat.Format = "dd/MM/yyyy";
                        }
                        // Para los teléfonos le añadimos formateo
                        if (accion.GetPropertyValue(column) != null && column.ToLower().Contains("telefono"))
                        {
                            var phoneNumber = worksheet.Cell(currentRow, currentColumn).Value.ToString();
                            var phoneFormat = GetFormatPhoneNumber(phoneNumber);

                            worksheet.Cell(currentRow, currentColumn).Value = phoneFormat;
                        }
                        currentColumn++;
                    }
                    ApplyRowConfigOnlyLines(worksheet, currentRow, 1, currentColumn - 1);

                    if (labelName == "TOTAL") worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, currentColumn - 1)).Style.Font.Bold = true;

                    currentRow++;
                }

                workbook.Worksheets.First().Range("A6", ColumnIndexToColumnLetter(currentColumn - 1) + currentRow.ToString()).SetAutoFilter();

                extractionInfo.FileRows = workbook.Worksheets.First().RowsUsed().Count(); //We remove the header row

               
            }
            catch(Exception ex)
            {
                error = string.Format("Message: {0}| Exception: {1}", ex.Message, ex.InnerException?.Message);
            }

            return workbook.GetStreamAndDispose(worksheet);
        }

        public static XLWorkbook CreateWorkbook(int startRow, string name, IEnumerable<string> columns, out IXLWorksheet worksheet)
        {
            var workbook = new XLWorkbook();

            // Add Sheet
            worksheet = workbook.Worksheets.Add(name);

            int currentColumn = 1;

            // Column Header
            foreach (string field in columns)
            {
                string title = field.DeCamelCase();
                IXLCell cell = worksheet.Cell(startRow, currentColumn);
                cell.Style.Font.SetBold();
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#BCBCBC");
                cell.Value = title;

                currentColumn++;
            }

            IXLCell cell2 = worksheet.Cell(startRow, currentColumn);
            cell2.Style.Font.SetBold();

            return workbook;
        }

        public static byte[] GenerateExcelFileTable(string workbookName, string objeto, string filters, IEnumerable<object> items, IEnumerable<string> columnIds, IEnumerable<string> displayNames)
        {
            using var workbook = CreateWorkbookTable(workbookName, objeto, filters, displayNames, out var worksheet);

            var currentRow = 6;

            foreach (object accion in items)
            {
                var currentColumn = 1;
                string labelName = string.Empty;

                foreach (string column in columnIds)
                {
                    worksheet.Cell(currentRow, currentColumn).Value = accion.GetPropertyValue(column);
                    if (column.ToLower() == "label") labelName = worksheet.Cell(currentRow, currentColumn).Value.ToString() ?? string.Empty;

                    if (accion.GetPropertyValue(column) != null && (accion.GetPropertyValue(column).GetType().Equals(typeof(double)) || (accion.GetPropertyValue(column).GetType().Equals(typeof(double?)))))
                    {
                        if (worksheet.Cell(currentRow, currentColumn).Value.ToString().Contains('.')) worksheet.Cell(currentRow, currentColumn).Style.NumberFormat.Format = "#,###.##";
                        else worksheet.Cell(currentRow, currentColumn).Style.NumberFormat.Format = "#,###";
                        worksheet.Cell(currentRow, currentColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                    if (accion.GetPropertyValue(column) != null && (accion.GetPropertyValue(column).GetType().Equals(typeof(String)) || accion.GetPropertyValue(column).GetType().Equals(typeof(Int32)) || accion.GetPropertyValue(column).GetType().Equals(typeof(Int32?))))
                    {
                        worksheet.Cell(currentRow, currentColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    }
                    if (accion.GetPropertyValue(column) != null && (accion.GetPropertyValue(column).GetType().Equals(typeof(DateTime)) || accion.GetPropertyValue(column).GetType().Equals(typeof(DateTime?))))
                    {
                        worksheet.Cell(currentRow, currentColumn).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        worksheet.Cell(currentRow, currentColumn).Style.DateFormat.Format = "dd/MM/yyyy";
                    }
                    // Para los teléfonos le añadimos formateo
                    if (accion.GetPropertyValue(column) != null && column.ToLower().Contains("telefono"))
                    {
                        var phoneNumber = worksheet.Cell(currentRow, currentColumn).Value.ToString();
                        var phoneFormat = GetFormatPhoneNumber(phoneNumber);

                        worksheet.Cell(currentRow, currentColumn).Value = phoneFormat;
                    }
                    currentColumn++;
                }

                ApplyRowConfigOnlyLines(worksheet, currentRow, 1, currentColumn - 1);

                if (labelName == "TOTAL") worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, currentColumn - 1)).Style.Font.Bold = true;

                currentRow++;
            }

            worksheet.Column(1).AdjustToContents(4);
            worksheet.Columns("B", "BV").AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            byte[] result = stream.ToArray();

            workbook.Dispose();

            return result;
        }

        public static XLWorkbook CreateWorkbookTable(string name, string objeto, string filters, IEnumerable<string> columns, out IXLWorksheet worksheet)
        {
            var workbook = new XLWorkbook();

            // Add Sheet
            worksheet = workbook.Worksheets.Add(name);

            // Object Finca or Contacto
            worksheet.Cell(1, "A").Value = objeto;
            worksheet.Cell(1, "A").Style.Font.SetBold();
            
            if (filters != null)
            {
                worksheet.Cell(3, "A").Value = filters;
            }
            int currentColumn = 1;

            // Column Header
            foreach (string field in columns)
            {
                string title = field;
                IXLCell cell = worksheet.Cell(5, currentColumn);
                ApplyRowConfig(cell);
                cell.Style.Font.SetBold();

                cell.Value = title;

                currentColumn++;
            }

            IXLCell cell2 = worksheet.Cell(1, currentColumn);
            cell2.Style.Font.SetBold();

            return workbook;
        }

        public static byte[] GetStreamAndDispose(this XLWorkbook workbook, IXLWorksheet worksheet)
        {
            // Style
            worksheet.Columns().AdjustToContents();

            // Return workbook
            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            byte[] result = stream.ToArray();

            workbook.Dispose();

            return result;
        }

        private static string GetFormatPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return string.Empty;

            var value = Regex.Replace(phone, @"\D", string.Empty);
            if (value.Length < 8) return "+" + phone;

            try
            {
                var valueInt = long.Parse(value);
            }
            catch (Exception ex)
            {
                return phone;
            }

            if (value.Length == 8)
                return long.Parse(value).ToString("+## ### ## #");
            if (value.Length == 9)
                return long.Parse(value).ToString("+## ### ## ##");
            if (value.Length == 10)
                return long.Parse(value).ToString("+## ### ## ## #");
            if (value.Length == 11)
                return long.Parse(value).ToString("+## ### ## ## ##");
            if (value.Length > 11)
                return long.Parse(value).ToString("+## ### ## ## ##" + new String('#', (value.Length - 9)));
            return string.Empty;
        }

        private static string GetNifFromRow(IXLWorksheet worksheet, int rowNumber)
        {
            var nifColumns = new[] { "B", "C", "D", "E", "F" };
            foreach (var nifColumn in nifColumns)
            {
                var nif = worksheet.Cell(rowNumber, nifColumn).Value;
                if (!nif.IsNullOrEmpty()) return nif.ToString();
            }

            return string.Empty;
        }

        private static void ApplyRowConfig(IXLCell cell)
        {
            cell.Style.Font.FontColor = XLColor.Black;
            cell.Style.Font.Bold = false;
            cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        }

        private static string GetFormattedValuesForRegistroField(string value1, string value2)
        {
            string result1 = FormatValues(value1);
            string result2 = FormatValues(value2);

            return $"{result1} de registro nº {result2}";
        }

        private static string FormatValues(string value)
        {
            var isIS = new CultureInfo("is-IS");
            bool isNumeric;
            isNumeric = long.TryParse(value, out long helper);
            string result = isNumeric ? helper.ToString("N", isIS).Replace(",00", "") : value;
            return result;
        }

        private static void ApplyRowConfigOnlyLines(IXLWorksheet ws, int row, int iCol, int eCol)
        {
            ws.Range(ws.Cell(row, iCol), ws.Cell(row, eCol)).Style.Font.FontColor = XLColor.Black;
            ws.Range(ws.Cell(row, iCol), ws.Cell(row, eCol)).Style.Font.Bold = false;
            ws.Range(ws.Cell(row, iCol), ws.Cell(row, eCol)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range(ws.Cell(row, iCol), ws.Cell(row, eCol)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(ws.Cell(row, iCol), ws.Cell(row, eCol)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        }
    }
}