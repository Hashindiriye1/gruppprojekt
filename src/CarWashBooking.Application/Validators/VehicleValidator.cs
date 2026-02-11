using CarWashBooking.Application.DTOs;
using FluentValidation;

namespace CarWashBooking.Application.Validators;

public class CreateVehicleDtoValidator : AbstractValidator<CreateVehicleDto>
{
    public CreateVehicleDtoValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.LicensePlate).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Make).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model).MaximumLength(100);
    }
}

public class UpdateVehicleDtoValidator : AbstractValidator<UpdateVehicleDto>
{
    public UpdateVehicleDtoValidator()
    {
        RuleFor(x => x.LicensePlate).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Make).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model).MaximumLength(100);
    }
}
