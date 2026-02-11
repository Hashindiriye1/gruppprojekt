using CarWashBooking.Application.DTOs;
using CarWashBooking.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CarWashBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(BookingService service, IValidator<CreateBookingDto> createValidator, IValidator<UpdateBookingDto> updateValidator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookingDto>>> GetAll(CancellationToken ct) =>
        Ok(await service.GetAllAsync(ct));

    /// <summary>
    /// Advanced: filter by status and/or date range, sort by ScheduledDate (default), Status, or CreatedAt.
    /// </summary>
    [HttpGet("filtered")]
    public async Task<ActionResult<IReadOnlyList<BookingDto>>> GetFiltered(
        [FromQuery] string? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortOrder,
        CancellationToken ct) =>
        Ok(await service.GetFilteredAsync(status, fromDate, toDate, sortBy, sortOrder, ct));

    /// <summary>
    /// Advanced: upcoming bookings for a location (special query).
    /// </summary>
    [HttpGet("upcoming-by-location/{locationId:int}")]
    public async Task<ActionResult<IReadOnlyList<BookingDto>>> GetUpcomingByLocation(int locationId, [FromQuery] int limit = 20, CancellationToken ct = default) =>
        Ok(await service.GetUpcomingByLocationAsync(locationId, limit, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingDto>> GetById(int id, CancellationToken ct)
    {
        var dto = await service.GetByIdAsync(id, ct);
        return dto == null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto dto, CancellationToken ct)
    {
        var result = await createValidator.ValidateAsync(dto, ct);
        if (!result.IsValid) return BadRequest(result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookingDto>> Update(int id, [FromBody] UpdateBookingDto dto, CancellationToken ct)
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
