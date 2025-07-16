using CommunicationApp.Controllers;
using CommunicationApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommunicationApp.Tests.ControllersTests
{
    public class PlaceholderControllerTests
    {
        [Fact]
        public void GetPlaceholders_ReturnsAllAvailablePlaceholders()
        {
            // Arrange
            var expectedPlaceholders = new Dictionary<string, string>
            {
                { "name", "The customer's full name." },
                { "email", "The customer's email address." }
            };

            var mockPlaceholderService = new Mock<IPlaceholderService>();
            mockPlaceholderService.Setup(service => service.GetAvailablePlaceholders())
                                  .Returns(expectedPlaceholders);

            var controller = new PlaceholderController(mockPlaceholderService.Object);

            // Act
            var result = controller.GetPlaceholders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPlaceholders = Assert.IsType<Dictionary<string, string>>(okResult.Value);
            Assert.Equal(expectedPlaceholders.Count, returnedPlaceholders.Count);
            foreach (var kvp in expectedPlaceholders)
            {
                Assert.Contains(kvp.Key, returnedPlaceholders.Keys);
                Assert.Equal(kvp.Value, returnedPlaceholders[kvp.Key]);
            }
            mockPlaceholderService.Verify(service => service.GetAvailablePlaceholders(), Times.Once);
        }
    }
}
