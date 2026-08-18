using ClosedXML.Excel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BusinessLayer.Services.ExcelService
{
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> Write<T>(IList<T> list)
        {
            var registersTotalRows = list.Count;
            using (var workbook = new XLWorkbook())
            {
                Type type = typeof(T);
                var properties = type.GetProperties();

                //Create worksheet
                var worksheet = workbook.Worksheets.Add(type.Name);

                //Create the header row
                for (var i = 0; i < properties.Length; i++)
                {
                    var cellHeaderText = properties[i].Name;
                    var attribute = properties[i].GetCustomAttribute(typeof(DisplayAttribute));
                    if (attribute != null)
                    {
                        cellHeaderText = ((DisplayAttribute)attribute).Name;
                    }
                    worksheet.Cell(1, i + 1).Value = cellHeaderText;
                }

                //Insert data starting from 2nd row, 1st cell
                worksheet.Cell(2, 1).InsertData(list);

                //Apply styles
                worksheet.Columns().AdjustToContents();
                var range = worksheet.Range(1, 1, registersTotalRows + 1, properties.Length);
                var table = range.CreateTable();
                table.ShowHeaderRow = true;
                table.Theme = XLTableTheme.TableStyleLight16;

                var ms = new MemoryStream();
                workbook.SaveAs(ms);
                return ms.ToArray();
            }
        }
    }
}
