using Moq;
using HelpdeskAPI.Controllers;
using HelpdeskAPI.Entities;
using HelpdeskAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

public class TicketsControllerTests
{
    [Fact]
    public async Task CreateTicket_ReturnsCreatedAtAction()
    {
        var mockRepo = new Mock<IMongoRepository<Ticket>>();
        var ticket = new Ticket { UserId = "user1", Title = "Test" };
        mockRepo.Setup(repo => repo.InsertAsync(It.IsAny<Ticket>()))
                .ReturnsAsync(ticket);
        var controller = new TicketsController(mockRepo.Object);
        var result = await controller.CreateTicket(ticket);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(ticket, createdResult.Value);
    }
}