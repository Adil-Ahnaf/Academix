namespace BusinessLayer.Services.HtmlService
{
    public interface IHtmlService
    {
        byte[] Write<T>(IList<T> list);
    }
}
