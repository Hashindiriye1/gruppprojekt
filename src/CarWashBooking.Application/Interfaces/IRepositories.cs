using CarWashBooking.Domain;

namespace CarWashBooking.Application.Interfaces;


public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct = default);
    Task<Customer> AddAsync(Customer entity, CancellationToken ct = default);
    Task UpdateAsync(Customer entity, CancellationToken ct = default);
    Task DeleteAsync(Customer entity, CancellationToken ct = default);
}

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Vehicle>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default);
    Task<Vehicle> AddAsync(Vehicle entity, CancellationToken ct = default);
    Task UpdateAsync(Vehicle entity, CancellationToken ct = default);
    Task DeleteAsync(Vehicle entity, CancellationToken ct = default);
}

public interface IServiceRepository
{
    Task<Domain.Service?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Domain.Service>> GetAllAsync(CancellationToken ct = default);
    Task<Domain.Service> AddAsync(Domain.Service entity, CancellationToken ct = default);
    Task UpdateAsync(Domain.Service entity, CancellationToken ct = default);
    Task DeleteAsync(Domain.Service entity, CancellationToken ct = default);
}

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Location>> GetAllAsync(CancellationToken ct = default);
    Task<Location> AddAsync(Location entity, CancellationToken ct = default);
    Task UpdateAsync(Location entity, CancellationToken ct = default);
    Task DeleteAsync(Location entity, CancellationToken ct = default);
}

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetFilteredAsync(BookingStatus? status, DateTime? fromDate, DateTime? toDate, string sortBy, bool sortDesc, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetUpcomingByLocationAsync(int locationId, int limit, CancellationToken ct = default);
    Task<Booking> AddAsync(Booking entity, CancellationToken ct = default);
    Task UpdateAsync(Booking entity, CancellationToken ct = default);
    Task DeleteAsync(Booking entity, CancellationToken ct = default);
}
