using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarWashBooking.Infrastructure.Repositories;

public class BookingRepository(CarWashDbContext context) : IBookingRepository
{
    private static IQueryable<Booking> WithIncludes(DbSet<Booking> set) =>
        set.Include(b => b.Customer).Include(b => b.Vehicle).Include(b => b.Service).Include(b => b.Location);

    public async Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await WithIncludes(context.Bookings).FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken ct = default) =>
        await WithIncludes(context.Bookings).OrderBy(b => b.ScheduledDate).ToListAsync(ct);

    public async Task<IReadOnlyList<Booking>> GetFilteredAsync(BookingStatus? status, DateTime? fromDate, DateTime? toDate, string sortBy, bool sortDesc, CancellationToken ct = default)
    {
        var q = WithIncludes(context.Bookings).AsQueryable();
        if (status.HasValue) q = q.Where(b => b.Status == status.Value);
        if (fromDate.HasValue) q = q.Where(b => b.ScheduledDate >= fromDate.Value);
        if (toDate.HasValue) q = q.Where(b => b.ScheduledDate <= toDate.Value);

        q = sortBy.ToLowerInvariant() switch
        {
            "status" => sortDesc ? q.OrderByDescending(b => b.Status) : q.OrderBy(b => b.Status),
            "createdat" => sortDesc ? q.OrderByDescending(b => b.CreatedAt) : q.OrderBy(b => b.CreatedAt),
            _ => sortDesc ? q.OrderByDescending(b => b.ScheduledDate) : q.OrderBy(b => b.ScheduledDate)
        };
        return await q.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Booking>> GetUpcomingByLocationAsync(int locationId, int limit, CancellationToken ct = default)
    {
        var from = DateTime.UtcNow;
        return await WithIncludes(context.Bookings)
            .Where(b => b.LocationId == locationId && b.ScheduledDate >= from && b.Status != BookingStatus.Cancelled)
            .OrderBy(b => b.ScheduledDate)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<Booking> AddAsync(Booking entity, CancellationToken ct = default)
    {
        context.Bookings.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task UpdateAsync(Booking entity, CancellationToken ct = default)
    {
        context.Bookings.Update(entity);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Booking entity, CancellationToken ct = default)
    {
        context.Bookings.Remove(entity);
        await context.SaveChangesAsync(ct);
    }
}
