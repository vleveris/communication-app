using CommunicationApp.Models;

namespace CommunicationApp.Services
{
    public interface ITemplateService
    {
        Task<IEnumerable<Template>> GetAllAsync();
        Task<Template> GetByIdAsync(int id);
        Task AddAsync(Template template);
        Task UpdateAsync(Template template);
        Task DeleteAsync(int id);
    }
}
