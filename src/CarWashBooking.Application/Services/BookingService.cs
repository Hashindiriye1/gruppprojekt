using AutoMapper;
using CarWashBooking.Application.DTOs;
using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.Extensions.Logging;

namespace CarWashBooking.Application.Services;

public class BookingService(
    IBookingRepository repository,
    IMapper mapper,
    ILogger<BookingService> logger)
{
    public async Task<BookingDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        return entity == null ? null : mapper.Map<BookingDto>(entity);
    }

    public async Task<IReadOnlyList<BookingDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await repository.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<BookingDto>>(list);
    }

    /// <summary>
    /// Advanced: filter by status and/or date, sort by date or status.
    /// </summary>
    public async Task<IReadOnlyList<BookingDto>> GetFilteredAsync(
        string? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? sortBy = "ScheduledDate",
        string? sortOrder = "asc",
        CancellationToken ct = default)
    {
        BookingStatus? s = null;
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<BookingStatus>(status, true, out var parsed))
            s = parsed;
        var sortDesc = sortOrder?.ToLowerInvariant() == "desc";
        var list = await repository.GetFilteredAsync(s, fromDate, toDate, sortBy ?? "ScheduledDate", sortDesc, ct);
        return mapper.Map<IReadOnlyList<BookingDto>>(list);
    }

    /// <summary>
    /// Advanced: upcoming bookings for a location (special query).
    /// </summary>
    public async Task<IReadOnlyList<BookingDto>> GetUpcomingByLocationAsync(int locationId, int limit = 20, CancellationToken ct = default)
    {
        var list = await repository.GetUpcomingByLocationAsync(locationId, limit, ct);
        return mapper.Map<IReadOnlyList<BookingDto>>(list);
    }

    public async Task<BookingDto> CreateAsync(CreateBookingDto dto, CancellationToken ct = default)
    {
        var entity = mapper.Map<Booking>(dto);
        entity = await repository.AddAsync(entity, ct);
        logger.LogInformation("Created booking {Id}", entity.Id);
        var full = await repository.GetByIdAsync(entity.Id, ct);
        return mapper.Map<BookingDto>(full!);
    }

    public async Task<BookingDto?> UpdateAsync(int id, UpdateBookingDto dto, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return null;
        if (Enum.TryParse<BookingStatus>(dto.Status, true, out var status))
            entity.Status = status;
        entity.ScheduledDate = dto.ScheduledDate;
        entity.Notes = dto.Notes;
        await repository.UpdateAsync(entity, ct);
        logger.LogInformation("Updated booking {Id}", id);
        return mapper.Map<BookingDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return false;
        await repository.DeleteAsync(entity, ct);
        logger.LogInformation("Deleted booking {Id}", id);
        return true;
    }
}
