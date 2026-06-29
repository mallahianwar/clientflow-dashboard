namespace ClientFlow.Api.Entities;

public class Project
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public Client? Client { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Status { get; set; } = "Planned";

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public decimal? Budget { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}