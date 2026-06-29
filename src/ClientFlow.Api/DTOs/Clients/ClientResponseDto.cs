namespace ClientFlow.Api.DTOs.Clients;

public class ClientResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? CompanyName { get; set; }

    public string? PhoneNumber { get; set; }

    public string Status { get; set; } = string.Empty;

    public int ProjectCount { get; set; }

    public DateTime CreatedAt { get; set; }
}