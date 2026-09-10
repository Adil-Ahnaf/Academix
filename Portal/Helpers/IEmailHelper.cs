namespace Portal.Helpers
{
    public interface IEmailHelper
    {
        Task SendAsync(string to, string subject, string body);
        Task SendBulkEmailAsync(List<string> toemails, string subject, string body);
    }
}
