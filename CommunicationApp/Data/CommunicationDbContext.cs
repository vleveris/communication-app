using Microsoft.EntityFrameworkCore;
using CommunicationApp.Models;

namespace CommunicationApp.Data
{
    public class CommunicationDbContext : DbContext
    {
        public CommunicationDbContext(DbContextOptions<CommunicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Template> Templates { get; set; }
    }
}
