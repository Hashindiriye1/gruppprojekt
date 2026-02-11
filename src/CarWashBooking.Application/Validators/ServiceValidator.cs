using CarWashBooking.Application.DTOs;
using FluentValidation;

namespace CarWashBooking.Application.Validators;

public class CreateServiceDtoValidator : AbstractValidator<CreateServiceDto>
{
    public CreateServiceDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DurationMinutes).GreaterThan(0);
    }
}

public class UpdateServiceDtoValidator : AbstractValidator<UpdateServiceDto>
{
    public UpdateServiceDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DurationMinutes).GreaterThan(0);
    }
}
