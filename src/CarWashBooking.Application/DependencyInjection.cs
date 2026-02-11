using CarWashBooking.Application.Mapping;
using CarWashBooking.Application.Services;
using CarWashBooking.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CarWashBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssemblyContaining<CreateCustomerDtoValidator>();
        services.AddScoped<CustomerService>();
        services.AddScoped<VehicleService>();
        services.AddScoped<WashServiceService>();
        services.AddScoped<LocationService>();
        services.AddScoped<BookingService>();
        return services;
    }
}
