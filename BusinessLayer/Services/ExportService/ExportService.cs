using BusinessLayer.Services.CsvService;
using BusinessLayer.Services.ExcelService;
using BusinessLayer.Services.HtmlService;

namespace BusinessLayer.Services.ExportService
{

    public class ExportService : IExportService
    {
        private readonly IExcelService _excelService;
        private readonly ICsvService _csvService;
        private readonly IHtmlService _htmlService;

        public ExportService(IExcelService excelService, ICsvService csvService, IHtmlService htmlService)
        {
            _excelService = excelService;
            _csvService = csvService;
            _htmlService = htmlService;
        }

        public Task<byte[]> ExportToExcel<T>(List<T> data)
        {
            return _excelService.Write(data);
        }

        public byte[] ExportToCsv<T>(List<T> data)
        {
            return _csvService.Write(data);
        }

        public byte[] ExportToHtml<T>(List<T> data)
        {
            return _htmlService.Write(data);
        }
    }
}
