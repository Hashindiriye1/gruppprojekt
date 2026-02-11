using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarWashBooking.Infrastructure.Repositories;

public class LocationRepository(CarWashDbContext context) : ILocationRepository
{
    public async Task<Location?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Locations.FindAsync([id], ct);

    public async Task<IReadOnlyList<Location>> GetAllAsync(CancellationToken ct = default) =>
        await context.Locations.OrderBy(l => l.Name).ToListAsync(ct);

    public async Task<Location> AddAsync(Location entity, CancellationToken ct = default)
    {
        context.Locations.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Location entity, CancellationToken ct = default)
    {
        context.Locations.Update(entity);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Location entity, CancellationToken ct = default)
    {
        context.Locations.Remove(entity);
        await context.SaveChangesAsync(ct);
    }
}
