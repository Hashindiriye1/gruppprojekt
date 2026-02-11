namespace CarWashBooking.Application.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int VehicleId { get; set; }
    public string VehicleInfo { get; set; } = string.Empty;
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public record CreateBookingDto(int CustomerId, int VehicleId, int ServiceId, int LocationId, DateTime ScheduledDate, string? Notes);
public record UpdateBookingDto(DateTime ScheduledDate, string Status, string? Notes);
