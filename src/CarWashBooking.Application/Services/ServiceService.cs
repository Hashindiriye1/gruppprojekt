using AutoMapper;
using CarWashBooking.Application.DTOs;
using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.Extensions.Logging;

namespace CarWashBooking.Application.Services;

public class WashServiceService(
    IServiceRepository repository,
    IBookingRepository bookingRepository,
    IMapper mapper,
    ILogger<WashServiceService> logger)
{
    public async Task<ServiceDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        return entity == null ? null : mapper.Map<ServiceDto>(entity);
    }

    public async Task<IReadOnlyList<ServiceDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await repository.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ServiceDto>>(list);
    }

    /// <summary>Get all services with their bookings (customer and vehicle per booking).</summary>
    public async Task<IReadOnlyList<ServiceWithBookingsDto>> GetAllWithBookingsAsync(CancellationToken ct = default)
    {
        var services = await repository.GetAllAsync(ct);
        var allBookings = await bookingRepository.GetAllAsync(ct);
        return services.Select(s => new ServiceWithBookingsDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Price = s.Price,
            DurationMinutes = s.DurationMinutes,
            Bookings = allBookings
                .Where(b => b.ServiceId == s.Id)
                .Select(b => new ServiceBookingInfoDto
                {
                    BookingId = b.Id,
                    CustomerName = b.Customer.Name,
                    VehicleInfo = $"{b.Vehicle.Make} {b.Vehicle.Model} ({b.Vehicle.LicensePlate})",
                    ScheduledDate = b.ScheduledDate,
                    Status = b.Status.ToString()
                })
                .ToList()
        }).ToList();
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto, CancellationToken ct = default)
    {
        var entity = mapper.Map<Domain.Service>(dto);
        entity = await repository.AddAsync(entity, ct);
        logger.LogInformation("Created service {Id}", entity.Id);
        return mapper.Map<ServiceDto>(entity);
    }

    public async Task<ServiceDto?> UpdateAsync(int id, UpdateServiceDto dto, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return null;
        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity, ct);
        logger.LogInformation("Updated service {Id}", id);
        return mapper.Map<ServiceDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return false;
        await repository.DeleteAsync(entity, ct);
        logger.LogInformation("Deleted service {Id}", id);
        return true;
    }
}
