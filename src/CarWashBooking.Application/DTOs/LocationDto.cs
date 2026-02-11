namespace CarWashBooking.Application.DTOs;

public class LocationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
public record CreateLocationDto(string Name, string Address);
public record UpdateLocationDto(string Name, string Address);
