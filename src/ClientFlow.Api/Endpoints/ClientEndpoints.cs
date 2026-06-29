using ClientFlow.Api.Data;
using ClientFlow.Api.DTOs.Clients;
using ClientFlow.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientFlow.Api.Endpoints;

public static class ClientEndpoints
{
    public static RouteGroupBuilder MapClientEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/clients")
            .WithTags("Clients");

        group.MapGet("/", async (
            AppDbContext db,
            string? search,
            int page = 1,
            int pageSize = 10) =>
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            var query = db.Clients
                .AsNoTracking()
                .Include(c => c.Projects)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.Email.Contains(search) ||
                    (c.CompanyName != null && c.CompanyName.Contains(search)));
            }

            var totalCount = await query.CountAsync();

            var clients = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ClientResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    CompanyName = c.CompanyName,
                    PhoneNumber = c.PhoneNumber,
                    Status = c.Status,
                    ProjectCount = c.Projects.Count,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return Results.Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = clients
            });
        });

        group.MapGet("/{id:int}", async (AppDbContext db, int id) =>
        {
            var client = await db.Clients
                .AsNoTracking()
                .Include(c => c.Projects)
                .Where(c => c.Id == id)
                .Select(c => new ClientResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    CompanyName = c.CompanyName,
                    PhoneNumber = c.PhoneNumber,
                    Status = c.Status,
                    ProjectCount = c.Projects.Count,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync();

            return client is null
                ? Results.NotFound()
                : Results.Ok(client);
        });

        group.MapPost("/", async (AppDbContext db, ClientCreateDto dto) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest("Client name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Results.BadRequest("Client email is required.");
            }

            var client = new Client
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim(),
                CompanyName = dto.CompanyName?.Trim(),
                PhoneNumber = dto.PhoneNumber?.Trim(),
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            db.Clients.Add(client);
            await db.SaveChangesAsync();

            return Results.Created($"/api/clients/{client.Id}", new
            {
                client.Id,
                client.Name,
                client.Email,
                client.CompanyName,
                client.PhoneNumber,
                client.Status,
                client.CreatedAt
            });
        });

        group.MapPut("/{id:int}", async (AppDbContext db, int id, ClientUpdateDto dto) =>
        {
            var client = await db.Clients.FindAsync(id);

            if (client is null)
            {
                return Results.NotFound();
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest("Client name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Results.BadRequest("Client email is required.");
            }

            client.Name = dto.Name.Trim();
            client.Email = dto.Email.Trim();
            client.CompanyName = dto.CompanyName?.Trim();
            client.PhoneNumber = dto.PhoneNumber?.Trim();
            client.Status = dto.Status.Trim();
            client.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (AppDbContext db, int id) =>
        {
            var client = await db.Clients.FindAsync(id);

            if (client is null)
            {
                return Results.NotFound();
            }

            db.Clients.Remove(client);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}