using CommunicationApp.Models;
using CommunicationApp.Repositories;
using CommunicationApp.Services;
using Moq;
using Xunit;

namespace CommunicationApp.Tests.ServicesTests
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllCustomers()
        {
            // Arrange
            var customers = new List<Customer> { new Customer { Id = 1, Name = "Test1", Email = "test1@example.com" }, new Customer { Id = 2, Name = "Test2", Email = "test2@example.com" } };
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);
            var service = new CustomerService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com" };
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(customer);
            var service = new CustomerService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.Equal(customer.Id, result.Id);
            mockRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task AddAsync_CallsRepositoryAdd()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "New Customer", Email = "new@example.com" };
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(repo => repo.AddAsync(customer)).Returns(Task.CompletedTask);
            var service = new CustomerService(mockRepo.Object);

            // Act
            await service.AddAsync(customer);

            // Assert
            mockRepo.Verify(repo => repo.AddAsync(customer), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Updated Customer", Email = "updated@example.com" };
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(customer)).Returns(Task.CompletedTask);
            var service = new CustomerService(mockRepo.Object);

            // Act
            await service.UpdateAsync(customer);

            // Assert
            mockRepo.Verify(repo => repo.UpdateAsync(customer), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDelete()
        {
            // Arrange
            var mockRepo = new Mock<ICustomerRepository>();
            mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);
            var service = new CustomerService(mockRepo.Object);

            // Act
            await service.DeleteAsync(1);

            // Assert
            mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
