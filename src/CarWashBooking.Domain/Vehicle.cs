namespace CarWashBooking.Domain;

public class Vehicle
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;

    public Customer Customer { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
