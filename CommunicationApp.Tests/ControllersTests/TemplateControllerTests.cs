using CommunicationApp.Controllers;
using CommunicationApp.Models;
using CommunicationApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommunicationApp.Tests.ControllersTests
{
    public class TemplateControllerTests
    {
        [Fact]
        public async Task GetTemplate_ReturnsNotFound_WhenTemplateDoesNotExist()
        {
            // Arrange
            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
                               .ReturnsAsync((Template?)null);

            var controller = new TemplateController(mockTemplateService.Object);

            // Act
            var result = await controller.GetTemplate(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTemplate_ReturnsTemplate_WhenTemplateExists()
        {
            // Arrange
            var template = new Template { Id = 1, Name = "Test Template", Subject = "Subject", Body = "Body" };
            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync(template);

            var controller = new TemplateController(mockTemplateService.Object);

            // Act
            var result = await controller.GetTemplate(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTemplate = Assert.IsType<Template>(okResult.Value);
            Assert.Equal(template.Id, returnedTemplate.Id);
            Assert.Equal(template.Name, returnedTemplate.Name);
        }

        [Fact]
        public async Task PostTemplate_ReturnsCreatedAtAction_WithTemplate()
        {
            // Arrange
            var newTemplate = new Template { Name = "New Template", Subject = "New Subject", Body = "New Body" };
            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(service => service.AddAsync(It.IsAny<Template>()))
                               .Callback<Template>(t => newTemplate.Id = 1); // Simulate ID generation

            var controller = new TemplateController(mockTemplateService.Object);

            // Act
            var result = await controller.CreateTemplate(newTemplate);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedTemplate = Assert.IsType<Template>(createdAtActionResult.Value);
            Assert.Equal(1, returnedTemplate.Id);
            mockTemplateService.Verify(service => service.AddAsync(newTemplate), Times.Once);
        }

        [Fact]
        public async Task PutTemplate_ReturnsNoContent_WhenTemplateExists()
        {
            // Arrange
            var existingTemplate = new Template { Id = 1, Name = "Existing Template", Subject = "Subject", Body = "Body" };
            var updatedTemplate = new Template { Id = 1, Name = "Updated Template", Subject = "Updated Subject", Body = "Updated Body" };
            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync(existingTemplate);
            mockTemplateService.Setup(service => service.UpdateAsync(It.IsAny<Template>()))
                               .Returns(Task.CompletedTask);

            var controller = new TemplateController(mockTemplateService.Object);

            // Act
            var result = await controller.UpdateTemplate(1, updatedTemplate);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockTemplateService.Verify(service => service.UpdateAsync(updatedTemplate), Times.Once);
        }

        [Fact]
        public async Task PutTemplate_ReturnsNotFound_WhenTemplateDoesNotExist()
        {
            // Arrange
            var updatedTemplate = new Template { Id = 1, Name = "Updated Template", Subject = "Updated Subject", Body = "Updated Body" };
            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync((Template?)null);

            var controller = new TemplateController(mockTemplateService.Object);

            // Act
            var result = await controller.UpdateTemplate(1, updatedTemplate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockTemplateService.Verify(service => service.UpdateAsync(It.IsAny<Template>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTemplate_ReturnsNoContent_WhenTemplateExists()
        {
            // Arrange
            var existingTemplate = new Template { Id = 1, Name = "Existing Template", Subject = "Subject", Body = "Body" };
            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync(existingTemplate);
            mockTemplateService.Setup(service => service.DeleteAsync(1))
                               .Returns(Task.CompletedTask);

            var controller = new TemplateController(mockTemplateService.Object);

            // Act
            var result = await controller.DeleteTemplate(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockTemplateService.Verify(service => service.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteTemplate_ReturnsNotFound_WhenTemplateDoesNotExist()
        {
            // Arrange
            var mockTemplateService = new Mock<ITemplateService>();
            mockTemplateService.Setup(service => service.GetByIdAsync(1))
                               .ReturnsAsync((Template?)null);

            var controller = new TemplateController(mockTemplateService.Object);

            // Act
            var result = await controller.DeleteTemplate(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockTemplateService.Verify(service => service.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
