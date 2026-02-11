using AutoMapper;
using CarWashBooking.Application.DTOs;
using CarWashBooking.Application.Interfaces;
using CarWashBooking.Domain;
using Microsoft.Extensions.Logging;

namespace CarWashBooking.Application.Services;

public class CustomerService(
    ICustomerRepository repository,
    IMapper mapper,
    ILogger<CustomerService> logger)
{
    public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        return entity == null ? null : mapper.Map<CustomerDto>(entity);
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await repository.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<CustomerDto>>(list);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto, CancellationToken ct = default)
    {
        var entity = mapper.Map<Customer>(dto);
        entity = await repository.AddAsync(entity, ct);
        logger.LogInformation("Created customer {Id}", entity.Id);
        return mapper.Map<CustomerDto>(entity);
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return null;
        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity, ct);
        logger.LogInformation("Updated customer {Id}", id);
        return mapper.Map<CustomerDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.GetByIdAsync(id, ct);
        if (entity == null) return false;
        await repository.DeleteAsync(entity, ct);
        logger.LogInformation("Deleted customer {Id}", id);
        return true;
    }
}
