namespace HelpdeskAPI.Entities;

public class Ticket : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "media"; // baja, media, alta
    public string CategoryId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty; // quien reporta
    public string? AssignedAgentId { get; set; }
    public string Status { get; set; } = "abierto"; // abierto, en progreso, resuelto, cerrado
    public List<Comment> Comments { get; set; } = new();
    public List<string> Attachments { get; set; } = new();
}

public class Comment
{
    public string UserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}