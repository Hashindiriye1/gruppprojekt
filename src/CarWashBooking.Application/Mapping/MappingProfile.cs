using AutoMapper;
using CarWashBooking.Application.DTOs;
using CarWashBooking.Domain;

namespace CarWashBooking.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>().ForAllMembers(o => o.Condition((_, _, v) => v != null));

        CreateMap<Vehicle, VehicleDto>();
        CreateMap<CreateVehicleDto, Vehicle>();
        CreateMap<UpdateVehicleDto, Vehicle>().ForAllMembers(o => o.Condition((_, _, v) => v != null));

        CreateMap<Domain.Service, ServiceDto>();
        CreateMap<CreateServiceDto, Domain.Service>();
        CreateMap<UpdateServiceDto, Domain.Service>().ForAllMembers(o => o.Condition((_, _, v) => v != null));

        CreateMap<Location, LocationDto>();
        CreateMap<CreateLocationDto, Location>();
        CreateMap<UpdateLocationDto, Location>().ForAllMembers(o => o.Condition((_, _, v) => v != null));

        CreateMap<Booking, BookingDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.Name))
            .ForMember(d => d.VehicleInfo, o => o.MapFrom(s => $"{s.Vehicle.Make} {s.Vehicle.Model} ({s.Vehicle.LicensePlate})"))
            .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service.Name))
            .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location.Name))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<CreateBookingDto, Booking>();
        CreateMap<UpdateBookingDto, Booking>().ForAllMembers(o => o.Condition((_, _, v) => v != null));
    }
}
