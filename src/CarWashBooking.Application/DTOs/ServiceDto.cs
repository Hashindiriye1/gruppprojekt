namespace CarWashBooking.Application.DTOs;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
}
public record CreateServiceDto(string Name, string Description, decimal Price, int DurationMinutes);
public record UpdateServiceDto(string Name, string Description, decimal Price, int DurationMinutes);

/// <summary>Customer and vehicle scheduled for a service (from a booking).</summary>
public class ServiceBookingInfoDto
{
    public int BookingId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleInfo { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>Service with list of bookings (customers and vehicles undergoing this service).</summary>
public class ServiceWithBookingsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }
    public IReadOnlyList<ServiceBookingInfoDto> Bookings { get; set; } = Array.Empty<ServiceBookingInfoDto>();
}
