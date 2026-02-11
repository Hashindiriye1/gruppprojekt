using CarWashBooking.Application.Interfaces;
using CarWashBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarWashBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CarWashDbContext>(o =>
            o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        return services;
    }
}
