namespace CarWashBooking.Application.DTOs;

public class VehicleDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}
public record CreateVehicleDto(int CustomerId, string LicensePlate, string Make, string Model);
public record UpdateVehicleDto(string LicensePlate, string Make, string Model);
