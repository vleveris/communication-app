using CommunicationApp.Models;
using CommunicationApp.Repositories;

namespace CommunicationApp.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;

        public TemplateService(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<IEnumerable<Template>> GetAllAsync()
        {
            return await _templateRepository.GetAllAsync();
        }

        public async Task<Template> GetByIdAsync(int id)
        {
            return await _templateRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Template template)
        {
            await _templateRepository.AddAsync(template);
        }

        public async Task UpdateAsync(Template template)
        {
            await _templateRepository.UpdateAsync(template);
        }

        public async Task DeleteAsync(int id)
        {
            await _templateRepository.DeleteAsync(id);
        }
    }
}
