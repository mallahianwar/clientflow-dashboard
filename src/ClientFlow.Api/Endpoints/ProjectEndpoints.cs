using ClientFlow.Api.Data;
using ClientFlow.Api.DTOs.Projects;
using ClientFlow.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientFlow.Api.Endpoints;

public static class ProjectEndpoints
{
    public static RouteGroupBuilder MapProjectEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/projects")
            .WithTags("Projects");

        group.MapGet("/", async (
            AppDbContext db,
            string? search,
            string? status,
            int? clientId,
            int page = 1,
            int pageSize = 10) =>
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            var query = db.Projects
                .AsNoTracking()
                .Include(p => p.Client)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(p => p.Status == status);
            }

            if (clientId.HasValue)
            {
                query = query.Where(p => p.ClientId == clientId.Value);
            }

            var totalCount = await query.CountAsync();

            var projects = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    ClientId = p.ClientId,
                    ClientName = p.Client != null ? p.Client.Name : string.Empty,
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status,
                    StartDate = p.StartDate,
                    DueDate = p.DueDate,
                    Budget = p.Budget,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return Results.Ok(new
            {
                totalCount,
                page,
                pageSize,
                data = projects
            });
        });

        group.MapGet("/{id:int}", async (AppDbContext db, int id) =>
        {
            var project = await db.Projects
                .AsNoTracking()
                .Include(p => p.Client)
                .Where(p => p.Id == id)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    ClientId = p.ClientId,
                    ClientName = p.Client != null ? p.Client.Name : string.Empty,
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status,
                    StartDate = p.StartDate,
                    DueDate = p.DueDate,
                    Budget = p.Budget,
                    CreatedAt = p.CreatedAt
                })
                .FirstOrDefaultAsync();

            return project is null
                ? Results.NotFound()
                : Results.Ok(project);
        });

        group.MapPost("/", async (AppDbContext db, ProjectCreateDto dto) =>
        {
            if (dto.ClientId <= 0)
            {
                return Results.BadRequest("Valid client is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest("Project name is required.");
            }

            var clientExists = await db.Clients.AnyAsync(c => c.Id == dto.ClientId);

            if (!clientExists)
            {
                return Results.BadRequest("Client does not exist.");
            }

            var project = new Project
            {
                ClientId = dto.ClientId,
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim(),
                Status = "Planned",
                StartDate = dto.StartDate,
                DueDate = dto.DueDate,
                Budget = dto.Budget,
                CreatedAt = DateTime.UtcNow
            };

            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return Results.Created($"/api/projects/{project.Id}", new
            {
                project.Id,
                project.ClientId,
                project.Name,
                project.Description,
                project.Status,
                project.StartDate,
                project.DueDate,
                project.Budget,
                project.CreatedAt
            });
        });

        group.MapPut("/{id:int}", async (AppDbContext db, int id, ProjectUpdateDto dto) =>
        {
            var project = await db.Projects.FindAsync(id);

            if (project is null)
            {
                return Results.NotFound();
            }

            if (dto.ClientId <= 0)
            {
                return Results.BadRequest("Valid client is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest("Project name is required.");
            }

            var clientExists = await db.Clients.AnyAsync(c => c.Id == dto.ClientId);

            if (!clientExists)
            {
                return Results.BadRequest("Client does not exist.");
            }

            project.ClientId = dto.ClientId;
            project.Name = dto.Name.Trim();
            project.Description = dto.Description?.Trim();
            project.Status = dto.Status.Trim();
            project.StartDate = dto.StartDate;
            project.DueDate = dto.DueDate;
            project.Budget = dto.Budget;
            project.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (AppDbContext db, int id) =>
        {
            var project = await db.Projects.FindAsync(id);

            if (project is null)
            {
                return Results.NotFound();
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        return group;
    }
}