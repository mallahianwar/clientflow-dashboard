namespace ClientFlow.Api.DTOs.Clients;

public class ClientUpdateDto
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? CompanyName { get; set; }

    public string? PhoneNumber { get; set; }

    public string Status { get; set; } = "Active";
}