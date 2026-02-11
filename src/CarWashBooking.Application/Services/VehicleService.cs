using AutoMapper;
using CarWashBooking.Application.DTOs;
using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.Extensions.Logging;

namespace CarWashBooking.Application.Services;

public class VehicleService(
    IVehicleRepository repository,
    IMapper mapper,
    ILogger<VehicleService> logger)
{
    public async Task<VehicleDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        return entity == null ? null : mapper.Map<VehicleDto>(entity);
    }

    public async Task<IReadOnlyList<VehicleDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await repository.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<VehicleDto>>(list);
    }

    public async Task<IReadOnlyList<VehicleDto>> GetByCustomerIdAsync(int customerId, CancellationToken ct = default)
    {
        var list = await repository.GetByCustomerIdAsync(customerId, ct);
        return mapper.Map<IReadOnlyList<VehicleDto>>(list);
    }

    public async Task<VehicleDto> CreateAsync(CreateVehicleDto dto, CancellationToken ct = default)
    {
        var entity = mapper.Map<Vehicle>(dto);
        entity = await repository.AddAsync(entity, ct);
        logger.LogInformation("Created vehicle {Id}", entity.Id);
        return mapper.Map<VehicleDto>(entity);
    }

    public async Task<VehicleDto?> UpdateAsync(int id, UpdateVehicleDto dto, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return null;
        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity, ct);
        logger.LogInformation("Updated vehicle {Id}", id);
        return mapper.Map<VehicleDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return false;
        await repository.DeleteAsync(entity, ct);
        logger.LogInformation("Deleted vehicle {Id}", id);
        return true;
    }
}
