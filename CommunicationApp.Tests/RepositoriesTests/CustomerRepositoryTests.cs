using CommunicationApp.Data;
using CommunicationApp.Models;
using CommunicationApp.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CommunicationApp.Tests.RepositoriesTests
{
    public class CustomerRepositoryTests
    {
        private CommunicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CommunicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new CommunicationDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllCustomers()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Customers.Add(new Customer { Id = 1, Name = "Customer 1", Email = "c1@example.com" });
            context.Customers.Add(new Customer { Id = 2, Name = "Customer 2", Email = "c2@example.com" });
            await context.SaveChangesAsync();
            var repository = new CustomerRepository(context);

            // Act
            var customers = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, customers.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Customers.Add(new Customer { Id = 1, Name = "Customer 1", Email = "c1@example.com" });
            await context.SaveChangesAsync();
            var repository = new CustomerRepository(context);

            // Act
            var customer = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal("Customer 1", customer.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            // Act
            var customer = await repository.GetByIdAsync(1);

            // Assert
            Assert.Null(customer);
        }

        [Fact]
        public async Task AddAsync_AddsCustomer()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);
            var newCustomer = new Customer { Name = "New Customer", Email = "new@example.com" };

            // Act
            await repository.AddAsync(newCustomer);

            // Assert
            Assert.Equal(1, context.Customers.Count());
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCustomer()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var existingCustomer = new Customer { Id = 1, Name = "Original Name", Email = "original@example.com" };
            context.Customers.Add(existingCustomer);
            await context.SaveChangesAsync();
            var repository = new CustomerRepository(context);

            existingCustomer.Name = "Updated Name";

            // Act
            await repository.UpdateAsync(existingCustomer);

            // Assert
            var updatedCustomer = await context.Customers.FindAsync(1);
            Assert.Equal("Updated Name", updatedCustomer.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesCustomer()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Customers.Add(new Customer { Id = 1, Name = "Customer to Delete", Email = "delete@example.com" });
            await context.SaveChangesAsync();
            var repository = new CustomerRepository(context);

            // Act
            await repository.DeleteAsync(1);

            // Assert
            Assert.Equal(0, context.Customers.Count());
        }

        [Fact]
        public async Task DeleteAsync_DoesNothing_WhenCustomerDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new CustomerRepository(context);

            // Act
            await repository.DeleteAsync(1);

            // Assert
            Assert.Equal(0, context.Customers.Count());
        }
    }
}
