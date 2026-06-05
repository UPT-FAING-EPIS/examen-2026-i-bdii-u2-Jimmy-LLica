using Microsoft.AspNetCore.Mvc;
using HelpdeskAPI.Entities;
using HelpdeskAPI.Repositories;

namespace HelpdeskAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IMongoRepository<Ticket> _ticketRepo;

    public TicketsController(IMongoRepository<Ticket> ticketRepo)
    {
        _ticketRepo = ticketRepo;
    }

    // POST /tickets
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
    {
        if (string.IsNullOrEmpty(ticket.UserId))
            return BadRequest("UserId is required");
        ticket.Status = "abierto";
        ticket.CreatedAt = DateTime.UtcNow;
        ticket.UpdatedAt = DateTime.UtcNow;
        var created = await _ticketRepo.InsertAsync(ticket);
        return CreatedAtAction(nameof(GetTicketById), new { id = created.Id }, created);
    }

    // GET /tickets?userId=xxx&agentId=xxx
    [HttpGet]
    public async Task<IActionResult> GetTickets([FromQuery] string? userId, [FromQuery] string? agentId)
    {
        IEnumerable<Ticket> tickets;
        if (!string.IsNullOrEmpty(userId))
            tickets = await _ticketRepo.FindAsync(t => t.UserId == userId);
        else if (!string.IsNullOrEmpty(agentId))
            tickets = await _ticketRepo.FindAsync(t => t.AssignedAgentId == agentId);
        else
            tickets = await _ticketRepo.GetAllAsync();
        return Ok(tickets);
    }

    // GET /tickets/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketById(string id)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null) return NotFound();
        return Ok(ticket);
    }

    // PUT /tickets/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(string id, [FromBody] Ticket updatedTicket)
    {
        var existing = await _ticketRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();
        updatedTicket.Id = id;
        updatedTicket.CreatedAt = existing.CreatedAt;
        updatedTicket.UpdatedAt = DateTime.UtcNow;
        var success = await _ticketRepo.UpdateAsync(id, updatedTicket);
        return success ? NoContent() : StatusCode(500);
    }

    // POST /tickets/{id}/comments
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(string id, [FromBody] Comment comment)
    {
        var ticket = await _ticketRepo.GetByIdAsync(id);
        if (ticket == null) return NotFound();
        comment.CreatedAt = DateTime.UtcNow;
        ticket.Comments.Add(comment);
        ticket.UpdatedAt = DateTime.UtcNow;
        var success = await _ticketRepo.UpdateAsync(id, ticket);
        return success ? Ok(ticket) : StatusCode(500);
    }
}