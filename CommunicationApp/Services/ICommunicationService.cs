namespace CommunicationApp.Services
{
    public interface ICommunicationService
    {
        Task SendMessageAsync(int customerId, int templateId);
        Task<string> GenerateMessageAsync(int customerId, int templateId);
    }
}
