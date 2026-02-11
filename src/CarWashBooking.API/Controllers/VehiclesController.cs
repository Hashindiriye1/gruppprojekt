using CarWashBooking.Application.DTOs;
using CarWashBooking.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarWashBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController(VehicleService service, IValidator<CreateVehicleDto> createValidator, IValidator<UpdateVehicleDto> updateValidator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    [HttpGet("by-customer/{customerId:int}")]
    public async Task<ActionResult<IReadOnlyList<VehicleDto>>> GetByCustomer(int customerId, CancellationToken ct) =>
        Ok(await service.GetByCustomerIdAsync(customerId, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehicleDto>> GetById(int id, CancellationToken ct)
    {
        var dto = await service.GetByIdAsync(id, ct);
        return dto == null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDto>> Create([FromBody] CreateVehicleDto dto, CancellationToken ct)
    {
        var result = await createValidator.ValidateAsync(dto, ct);
        if (!result.IsValid) return BadRequest(result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VehicleDto>> Update(int id, [FromBody] UpdateVehicleDto dto, CancellationToken ct)
    {
        var result = await updateValidator.ValidateAsync(dto, ct);
        if (!result.IsValid) return BadRequest(result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
        var updated = await service.UpdateAsync(id, dto, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await service.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }
}
