using ClientFlow.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ClientFlow.Api.Endpoints;

public static class DashboardEndpoints
{
    public static RouteGroupBuilder MapDashboardEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/dashboard")
            .WithTags("Dashboard");

        group.MapGet("/summary", async (AppDbContext db) =>
        {
            var totalClients = await db.Clients.CountAsync();
            var activeClients = await db.Clients.CountAsync(c => c.Status == "Active");

            var totalProjects = await db.Projects.CountAsync();
            var plannedProjects = await db.Projects.CountAsync(p => p.Status == "Planned");
            var inProgressProjects = await db.Projects.CountAsync(p => p.Status == "InProgress");
            var completedProjects = await db.Projects.CountAsync(p => p.Status == "Completed");

            var totalBudget = await db.Projects
                .Where(p => p.Budget != null)
                .SumAsync(p => p.Budget);

            return Results.Ok(new
            {
                totalClients,
                activeClients,
                totalProjects,
                plannedProjects,
                inProgressProjects,
                completedProjects,
                totalBudget
            });
        });

        return group;
    }
}