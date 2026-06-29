namespace ClientFlow.Api.DTOs.Projects;

public class ProjectResponseDto
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string ClientName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public decimal? Budget { get; set; }

    public DateTime CreatedAt { get; set; }
}