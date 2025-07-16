using CommunicationApp.Controllers;
using CommunicationApp.Models;
using CommunicationApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommunicationApp.Tests.ControllersTests
{
    public class CustomerControllerTests
    {
        [Fact]
        public async Task GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
                               .ReturnsAsync((Customer?)null);

            var controller = new CustomerController(mockCustomerService.Object);

            // Act
            var result = await controller.GetCustomer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCustomer_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer", Email = "test@example.com" };
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync(customer);

            var controller = new CustomerController(mockCustomerService.Object);

            // Act
            var result = await controller.GetCustomer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(customer.Id, returnedCustomer.Id);
            Assert.Equal(customer.Name, returnedCustomer.Name);
            Assert.Equal(customer.Email, returnedCustomer.Email);
        }

        [Fact]
        public async Task PostCustomer_ReturnsCreatedAtAction_WithCustomer()
        {
            // Arrange
            var newCustomer = new Customer { Name = "New Customer", Email = "new@example.com" };
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(service => service.AddAsync(It.IsAny<Customer>()))
                               .Callback<Customer>(c => newCustomer.Id = 1); // Simulate ID generation

            var controller = new CustomerController(mockCustomerService.Object);

            // Act
            var result = await controller.CreateCustomer(newCustomer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCustomer = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal(1, returnedCustomer.Id);
            mockCustomerService.Verify(service => service.AddAsync(newCustomer), Times.Once);
        }

        [Fact]
        public async Task PutCustomer_ReturnsNoContent_WhenCustomerExists()
        {
            // Arrange
            var existingCustomer = new Customer { Id = 1, Name = "Existing Customer", Email = "existing@example.com" };
            var updatedCustomer = new Customer { Id = 1, Name = "Updated Customer", Email = "updated@example.com" };
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync(existingCustomer);
            mockCustomerService.Setup(service => service.UpdateAsync(It.IsAny<Customer>()))
                               .Returns(Task.CompletedTask);

            var controller = new CustomerController(mockCustomerService.Object);

            // Act
            var result = await controller.UpdateCustomer(1, updatedCustomer);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockCustomerService.Verify(service => service.UpdateAsync(updatedCustomer), Times.Once);
        }

        [Fact]
        public async Task PutCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            var updatedCustomer = new Customer { Id = 1, Name = "Updated Customer", Email = "updated@example.com" };
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync((Customer?)null);

            var controller = new CustomerController(mockCustomerService.Object);

            // Act
            var result = await controller.UpdateCustomer(1, updatedCustomer);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockCustomerService.Verify(service => service.UpdateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNoContent_WhenCustomerExists()
        {
            // Arrange
            var existingCustomer = new Customer { Id = 1, Name = "Existing Customer", Email = "existing@example.com" };
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync(existingCustomer);
            mockCustomerService.Setup(service => service.DeleteAsync(1))
                               .Returns(Task.CompletedTask);

            var controller = new CustomerController(mockCustomerService.Object);

            // Act
            var result = await controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockCustomerService.Verify(service => service.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync((Customer?)null);

            var controller = new CustomerController(mockCustomerService.Object);

            // Act
            var result = await controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockCustomerService.Verify(service => service.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
