using CommunicationApp.Models;
using CommunicationApp.Repositories;
using CommunicationApp.Services;
using Moq;
using Xunit;

namespace CommunicationApp.Tests.ServicesTests
{
    public class CommunicationServiceTests
    {
        [Fact]
        public async Task SendMessageAsync_LogsMessage_WhenCustomerAndTemplateExist()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", City = "Test City", PostalCode = "12345" };
            var template = new Template { Id = 1, Name = "Test Template", Subject = "Hello {{name}}", Body = "Hi {{name}}, this is a test." };

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);

            var mockTemplateRepo = new Mock<ITemplateRepository>();
            mockTemplateRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(template);

            var mockPlaceholderService = new Mock<IPlaceholderService>();
            mockPlaceholderService.Setup(service => service.GetAvailablePlaceholders())
                                  .Returns(new Dictionary<string, string>
                                  {
                                      { "name", "" },
                                      { "email", "" },
                                      { "city", "" },
                                      { "postalcode", "" }
                                  });

            var service = new CommunicationApp.Services.CommunicationService(mockCustomerRepo.Object, mockTemplateRepo.Object, mockPlaceholderService.Object);

            // Act
            await service.SendMessageAsync(1, 1);

            // Assert (This test primarily verifies that no exception is thrown and mocks are called)
            mockCustomerRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            mockTemplateRepo.Verify(repo => repo.GetByIdAsync(1), Times.Exactly(2));
        }

        [Fact]
        public async Task SendMessageAsync_ThrowsKeyNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var template = new Template { Id = 1, Name = "Test Template", Subject = "Hello {{name}}", Body = "Hi {{name}}, this is a test." };

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Customer?)null);

            var mockTemplateRepo = new Mock<ITemplateRepository>();
            mockTemplateRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(template);

            var mockPlaceholderService = new Mock<IPlaceholderService>();
            mockPlaceholderService.Setup(service => service.GetAvailablePlaceholders())
                                  .Returns(new Dictionary<string, string>
                                  {
                                      { "name", "" },
                                      { "email", "" },
                                      { "city", "" },
                                      { "postalcode", "" }
                                  });

            var service = new CommunicationApp.Services.CommunicationService(mockCustomerRepo.Object, mockTemplateRepo.Object, mockPlaceholderService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.SendMessageAsync(1, 1));
            mockCustomerRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            mockTemplateRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_ThrowsKeyNotFoundException_WhenTemplateDoesNotExist()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com", City = "Test City", PostalCode = "12345" };

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);

            var mockTemplateRepo = new Mock<ITemplateRepository>();
            mockTemplateRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Template?)null);

            var mockPlaceholderService = new Mock<IPlaceholderService>();
            mockPlaceholderService.Setup(service => service.GetAvailablePlaceholders())
                                  .Returns(new Dictionary<string, string>
                                  {
                                      { "name", "" },
                                      { "email", "" },
                                      { "city", "" },
                                      { "postalcode", "" }
                                  });

            var service = new CommunicationApp.Services.CommunicationService(mockCustomerRepo.Object, mockTemplateRepo.Object, mockPlaceholderService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.SendMessageAsync(1, 1));
            mockCustomerRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            mockTemplateRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GenerateMessageAsync_ReplacesPlaceholdersCorrectly()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "John Doe", Email = "john.doe@example.com", City = "New York", PostalCode = "10001" };
            var template = new Template { Id = 1, Name = "Test Template", Subject = "Hello {{name}}", Body = "Hi {{name}}, your email is {{email}} and you live in {{city}} ({{postalcode}})." };

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);

            var mockTemplateRepo = new Mock<ITemplateRepository>();
            mockTemplateRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(template);

            var mockPlaceholderService = new Mock<IPlaceholderService>();
            mockPlaceholderService.Setup(service => service.GetAvailablePlaceholders())
                                  .Returns(new Dictionary<string, string>
                                  {
                                      { "name", "" },
                                      { "email", "" },
                                      { "city", "" },
                                      { "postalcode", "" }
                                  });

            var service = new CommunicationApp.Services.CommunicationService(mockCustomerRepo.Object, mockTemplateRepo.Object, mockPlaceholderService.Object);

            // Act
            var message = await service.GenerateMessageAsync(1, 1);

            // Assert
            Assert.Equal("Hi John Doe, your email is john.doe@example.com and you live in New York (10001).", message);
        }

        [Fact]
        public async Task GenerateMessageAsync_ThrowsNotSupportedException_ForUnsupportedPlaceholder()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "John Doe", Email = "john.doe@example.com", City = "New York", PostalCode = "10001" };
            var template = new Template { Id = 1, Name = "Test Template", Subject = "Hello {{name}}", Body = "Hi {{name}}, your phone is {{phone}}." };

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);

            var mockTemplateRepo = new Mock<ITemplateRepository>();
            mockTemplateRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(template);

            var mockPlaceholderService = new Mock<IPlaceholderService>();
            mockPlaceholderService.Setup(service => service.GetAvailablePlaceholders())
                                  .Returns(new Dictionary<string, string>
                                  {
                                      { "name", "" },
                                      { "email", "" },
                                      { "city", "" },
                                      { "postalcode", "" },
                                      { "phone", "" }
                                  });

            var service = new CommunicationApp.Services.CommunicationService(mockCustomerRepo.Object, mockTemplateRepo.Object, mockPlaceholderService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => service.GenerateMessageAsync(1, 1));
        }

        [Fact]
        public async Task GenerateMessageAsync_DoesNotReplaceUnknownPlaceholder()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "John Doe", Email = "john.doe@example.com", City = "New York", PostalCode = "10001" };
            var template = new Template { Id = 1, Name = "Test Template", Subject = "Hello {{name}}", Body = "Hi {{name}}, your favorite color is {{color}}." };

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);

            var mockTemplateRepo = new Mock<ITemplateRepository>();
            mockTemplateRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(template);

            var mockPlaceholderService = new Mock<IPlaceholderService>();
            mockPlaceholderService.Setup(service => service.GetAvailablePlaceholders())
                                  .Returns(new Dictionary<string, string>
                                  {
                                      { "name", "" },
                                      { "email", "" },
                                      { "city", "" },
                                      { "postalcode", "" }
                                  });

            var service = new CommunicationApp.Services.CommunicationService(mockCustomerRepo.Object, mockTemplateRepo.Object, mockPlaceholderService.Object);

            // Act
            var message = await service.GenerateMessageAsync(1, 1);

            // Assert
            Assert.Equal("Hi John Doe, your favorite color is {{color}}.", message);
        }
    }
}
