namespace ClientFlow.Api.DTOs.Projects;

public class ProjectCreateDto
{
    public int ClientId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public decimal? Budget { get; set; }
}