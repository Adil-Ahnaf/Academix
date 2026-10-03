using System.Reflection;

namespace BusinessLayer.Services.ExcelService
{
    public interface IExcelService
    {
        Task<byte[]> Write<T>(IList<T> list);
    }
}
