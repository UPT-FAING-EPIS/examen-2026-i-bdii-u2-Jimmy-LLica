using HelpdeskAPI.Entities;
using HelpdeskAPI.Repositories;

namespace HelpdeskAPI.Services;

public class TicketService
{
    private readonly IMongoRepository<Ticket> _ticketRepo;

    public TicketService(IMongoRepository<Ticket> ticketRepo)
    {
        _ticketRepo = ticketRepo;
    }

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        // Asignación automática: si no tiene agente asignado, podrías asignar el de menor carga
        // (por simplicidad lo dejamos sin asignar)
        return await _ticketRepo.InsertAsync(ticket);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(string userId)
    {
        return await _ticketRepo.FindAsync(t => t.UserId == userId);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByAgentAsync(string agentId)
    {
        return await _ticketRepo.FindAsync(t => t.AssignedAgentId == agentId);
    }

    public async Task<Ticket?> GetTicketByIdAsync(string id) => await _ticketRepo.GetByIdAsync(id);

    public async Task<bool> UpdateTicketAsync(string id, Ticket ticket) => await _ticketRepo.UpdateAsync(id, ticket);

    public async Task<bool> AddCommentAsync(string ticketId, Comment comment)
    {
        var ticket = await _ticketRepo.GetByIdAsync(ticketId);
        if (ticket == null) return false;
        ticket.Comments.Add(comment);
        ticket.UpdatedAt = DateTime.UtcNow;
        return await _ticketRepo.UpdateAsync(ticketId, ticket);
    }
}