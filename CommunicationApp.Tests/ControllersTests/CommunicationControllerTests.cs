using CommunicationApp.Controllers;
using CommunicationApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommunicationApp.Tests.ControllersTests
{
    public class CommunicationControllerTests
    {
        [Fact]
        public async Task SendMessage_ReturnsOk_WhenMessageSentSuccessfully()
        {
            // Arrange
            var mockCommunicationService = new Mock<ICommunicationService>();
            mockCommunicationService.Setup(service => service.SendMessageAsync(It.IsAny<int>(), It.IsAny<int>()))
                                    .Returns(Task.CompletedTask);

            var controller = new CommunicationController(mockCommunicationService.Object);

            // Act
            var result = await controller.SendMessage(1, 1);

            // Assert
            Assert.IsType<OkResult>(result);
            mockCommunicationService.Verify(service => service.SendMessageAsync(1, 1), Times.Once);
        }

        [Fact]
        public async Task SendMessage_ReturnsNotFound_WhenCustomerOrTemplateNotFound()
        {
            // Arrange
            var mockCommunicationService = new Mock<ICommunicationService>();
            mockCommunicationService.Setup(service => service.SendMessageAsync(It.IsAny<int>(), It.IsAny<int>()))
                                    .ThrowsAsync(new KeyNotFoundException());

            var controller = new CommunicationController(mockCommunicationService.Object);

            // Act
            var result = await controller.SendMessage(1, 1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockCommunicationService.Verify(service => service.SendMessageAsync(1, 1), Times.Once);
        }
    }
}
