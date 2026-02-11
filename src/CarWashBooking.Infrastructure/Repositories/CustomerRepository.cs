using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarWashBooking.Infrastructure.Repositories;

public class CustomerRepository(CarWashDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await context.Customers.FindAsync([id], ct);

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct = default) =>
        await context.Customers.OrderBy(c => c.Name).ToListAsync(ct);

    public async Task<Customer> AddAsync(Customer entity, CancellationToken ct = default)
    {
        context.Customers.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Customer entity, CancellationToken ct = default)
    {
        context.Customers.Update(entity);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Customer entity, CancellationToken ct = default)
    {
        context.Customers.Remove(entity);
        await context.SaveChangesAsync(ct);
    }
}
