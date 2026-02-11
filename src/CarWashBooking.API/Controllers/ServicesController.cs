using CarWashBooking.Application.DTOs;
using CarWashBooking.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarWashBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController(WashServiceService service, IValidator<CreateServiceDto> createValidator, IValidator<UpdateServiceDto> updateValidator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ServiceDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    [HttpGet("with-bookings")]
    public async Task<ActionResult<IReadOnlyList<ServiceWithBookingsDto>>> GetAllWithBookings(CancellationToken ct) =>
        Ok(await service.GetAllWithBookingsAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceDto>> GetById(int id, CancellationToken ct)
    {
        var dto = await service.GetByIdAsync(id, ct);
        return dto == null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> Create([FromBody] CreateServiceDto dto, CancellationToken ct)
    {
        var result = await createValidator.ValidateAsync(dto, ct);
        if (!result.IsValid) return BadRequest(result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServiceDto>> Update(int id, [FromBody] UpdateServiceDto dto, CancellationToken ct)
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
