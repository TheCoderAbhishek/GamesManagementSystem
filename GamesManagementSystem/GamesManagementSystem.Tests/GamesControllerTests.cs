using GamesManagementSystem.Application.Interfaces;
using GamesManagementSystem.Domain.Entities;
using GamesManagementSystem.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GamesManagementSystem.Tests
{
    public class GamesControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfGames()
        {
            // Arrange: Set up the test environment.
            var mockRepo = new Mock<IGameRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Game>
                {
                    new Game { Id = 1, Name = "Game One" },
                    new Game { Id = 2, Name = "Game Two" }
                });

            var mockFileService = new Mock<IFileService>();
            var mockWebHost = new Mock<IWebHostEnvironment>();

            var controller = new GamesController(
                mockRepo.Object,
                mockFileService.Object,
                mockWebHost.Object);

            // Act: Execute the method being tested.
            var result = await controller.Index();

            // Assert: Verify the outcome.
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Game>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }
    }
}