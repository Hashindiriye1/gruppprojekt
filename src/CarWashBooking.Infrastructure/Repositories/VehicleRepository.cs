using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarWashBooking.Infrastructure.Repositories;

public class VehicleRepository(CarWashDbContext context) : IVehicleRepository
{
    public async Task<Vehicle?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Vehicles.Include(v => v.Customer).FirstOrDefaultAsync(v => v.Id == id, ct);

    public async Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken ct = default) =>
        await context.Vehicles.Include(v => v.Customer).OrderBy(v => v.LicensePlate).ToListAsync(ct);

    public async Task<IReadOnlyList<Vehicle>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default) =>
        await context.Vehicles.Where(v => v.CustomerId == customerId).OrderBy(v => v.LicensePlate).ToListAsync(ct);

    public async Task<Vehicle> AddAsync(Vehicle entity, CancellationToken ct = default)
    {
        context.Vehicles.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Vehicle entity, CancellationToken ct = default)
    {
        context.Vehicles.Update(entity);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Vehicle entity, CancellationToken ct = default)
    {
        context.Vehicles.Remove(entity);
        await context.SaveChangesAsync(ct);
    }
}
