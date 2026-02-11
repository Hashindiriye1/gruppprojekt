namespace CarWashBooking.Domain;

public class Booking
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int VehicleId { get; set; }
    public int ServiceId { get; set; }
    public int LocationId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Customer Customer { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public Location Location { get; set; } = null!;
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
}
