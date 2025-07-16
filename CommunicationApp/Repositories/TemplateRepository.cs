using CommunicationApp.Data;
using CommunicationApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunicationApp.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly CommunicationDbContext _context;

        public TemplateRepository(CommunicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Template>> GetAllAsync()
        {
            return await _context.Templates.ToListAsync();
        }

        public async Task<Template> GetByIdAsync(int id)
        {
            return await _context.Templates.FindAsync(id);
        }

        public async Task AddAsync(Template template)
        {
            _context.Templates.Add(template);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Template template)
        {
            _context.Templates.Update(template);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template != null)
            {
                _context.Templates.Remove(template);
                await _context.SaveChangesAsync();
            }
        }
    }
}
