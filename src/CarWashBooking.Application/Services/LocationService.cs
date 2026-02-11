using AutoMapper;
using CarWashBooking.Application.DTOs;
using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.Extensions.Logging;

namespace CarWashBooking.Application.Services;

public class LocationService(
    ILocationRepository repository,
    IMapper mapper,
    ILogger<LocationService> logger)
{
    public async Task<LocationDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        return entity == null ? null : mapper.Map<LocationDto>(entity);
    }

    public async Task<IReadOnlyList<LocationDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await repository.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<LocationDto>>(list);
    }

    public async Task<LocationDto> CreateAsync(CreateLocationDto dto, CancellationToken ct = default)
    {
        var entity = mapper.Map<Location>(dto);
        entity = await repository.AddAsync(entity, ct);
        logger.LogInformation("Created location {Id}", entity.Id);
        return mapper.Map<LocationDto>(entity);
    }

    public async Task<LocationDto?> UpdateAsync(int id, UpdateLocationDto dto, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return null;
        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity, ct);
        logger.LogInformation("Updated location {Id}", id);
        return mapper.Map<LocationDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return false;
        await repository.DeleteAsync(entity, ct);
        logger.LogInformation("Deleted location {Id}", id);
        return true;
    }
}
