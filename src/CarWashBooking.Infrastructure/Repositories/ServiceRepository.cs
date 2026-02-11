using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarWashBooking.Infrastructure.Repositories;

public class ServiceRepository(CarWashDbContext context) : IServiceRepository
{
    public async Task<Domain.Service?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Services.FindAsync([id], ct);

    public async Task<IReadOnlyList<Domain.Service>> GetAllAsync(CancellationToken ct = default) =>
        await context.Services.OrderBy(s => s.Name).ToListAsync(ct);

    public async Task<Domain.Service> AddAsync(Domain.Service entity, CancellationToken ct = default)
    {
        context.Services.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Domain.Service entity, CancellationToken ct = default)
    {
        context.Services.Update(entity);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Domain.Service entity, CancellationToken ct = default)
    {
        context.Services.Remove(entity);
        await context.SaveChangesAsync(ct);
    }
}
