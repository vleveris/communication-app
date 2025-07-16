using CommunicationApp.Models;
using CommunicationApp.Repositories;
using CommunicationApp.Services;
using Moq;
using Xunit;

namespace CommunicationApp.Tests.ServicesTests
{
    public class TemplateServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllTemplates()
        {
            // Arrange
            var templates = new List<Template> { new Template { Id = 1, Name = "Test1", Subject = "Sub1", Body = "Body1" }, new Template { Id = 2, Name = "Test2", Subject = "Sub2", Body = "Body2" } };
            var mockRepo = new Mock<ITemplateRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(templates);
            var service = new TemplateService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTemplate_WhenTemplateExists()
        {
            // Arrange
            var template = new Template { Id = 1, Name = "Test Template", Subject = "Test Subject", Body = "Test Body" };
            var mockRepo = new Mock<ITemplateRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(template);
            var service = new TemplateService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.Equal(template.Id, result.Id);
            mockRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAdd()
        {
            // Arrange
            var template = new Template { Id = 1, Name = "New Template", Subject = "New Subject", Body = "New Body" };
            var mockRepo = new Mock<ITemplateRepository>();
            mockRepo.Setup(repo => repo.AddAsync(template)).Returns(Task.CompletedTask);
            var service = new TemplateService(mockRepo.Object);

            // Act
            await service.AddAsync(template);

            // Assert
            mockRepo.Verify(repo => repo.AddAsync(template), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var template = new Template { Id = 1, Name = "Updated Template", Subject = "Updated Subject", Body = "Updated Body" };
            var mockRepo = new Mock<ITemplateRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(template)).Returns(Task.CompletedTask);
            var service = new TemplateService(mockRepo.Object);

            // Act
            await service.UpdateAsync(template);

            // Assert
            mockRepo.Verify(repo => repo.UpdateAsync(template), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDelete()
        {
            // Arrange
            var mockRepo = new Mock<ITemplateRepository>();
            mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);
            var service = new TemplateService(mockRepo.Object);

            // Act
            await service.DeleteAsync(1);

            // Assert
            mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
