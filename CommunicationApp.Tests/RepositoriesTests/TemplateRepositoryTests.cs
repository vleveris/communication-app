using CommunicationApp.Data;
using CommunicationApp.Models;
using CommunicationApp.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CommunicationApp.Tests.RepositoriesTests
{
    public class TemplateRepositoryTests
    {
        private CommunicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CommunicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new CommunicationDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTemplates()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Templates.Add(new Template { Id = 1, Name = "Template 1", Subject = "Sub1", Body = "Body1" });
            context.Templates.Add(new Template { Id = 2, Name = "Template 2", Subject = "Sub2", Body = "Body2" });
            await context.SaveChangesAsync();
            var repository = new TemplateRepository(context);

            // Act
            var templates = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, templates.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTemplate_WhenTemplateExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Templates.Add(new Template { Id = 1, Name = "Template 1", Subject = "Sub1", Body = "Body1" });
            await context.SaveChangesAsync();
            var repository = new TemplateRepository(context);

            // Act
            var template = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(template);
            Assert.Equal("Template 1", template.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenTemplateDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TemplateRepository(context);

            // Act
            var template = await repository.GetByIdAsync(1);

            // Assert
            Assert.Null(template);
        }

        [Fact]
        public async Task AddAsync_AddsTemplate()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TemplateRepository(context);
            var newTemplate = new Template { Name = "New Template", Subject = "New Sub", Body = "New Body" };

            // Act
            await repository.AddAsync(newTemplate);

            // Assert
            Assert.Equal(1, context.Templates.Count());
        }

        [Fact]
        public async Task UpdateAsync_UpdatesTemplate()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var existingTemplate = new Template { Id = 1, Name = "Original Name", Subject = "Original Sub", Body = "Original Body" };
            context.Templates.Add(existingTemplate);
            await context.SaveChangesAsync();
            var repository = new TemplateRepository(context);

            existingTemplate.Name = "Updated Name";

            // Act
            await repository.UpdateAsync(existingTemplate);

            // Assert
            var updatedTemplate = await context.Templates.FindAsync(1);
            Assert.Equal("Updated Name", updatedTemplate.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesTemplate()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Templates.Add(new Template { Id = 1, Name = "Template to Delete", Subject = "Sub", Body = "Body" });
            await context.SaveChangesAsync();
            var repository = new TemplateRepository(context);

            // Act
            await repository.DeleteAsync(1);

            // Assert
            Assert.Equal(0, context.Templates.Count());
        }

        [Fact]
        public async Task DeleteAsync_DoesNothing_WhenTemplateDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TemplateRepository(context);

            // Act
            await repository.DeleteAsync(1);

            // Assert
            Assert.Equal(0, context.Templates.Count());
        }
    }
}
