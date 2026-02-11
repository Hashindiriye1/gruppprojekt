using CarWashBooking.Application.DTOs;
using FluentValidation;

namespace CarWashBooking.Application.Validators;

public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingDtoValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.VehicleId).GreaterThan(0);
        RuleFor(x => x.ServiceId).GreaterThan(0);
        RuleFor(x => x.LocationId).GreaterThan(0);
        RuleFor(x => x.ScheduledDate).NotEmpty();
    }
}

public class UpdateBookingDtoValidator : AbstractValidator<UpdateBookingDto>
{
    public UpdateBookingDtoValidator()
    {
        RuleFor(x => x.Status).Must(s => new[] { "Pending", "Confirmed", "Completed", "Cancelled" }.Contains(s));
    }
}
