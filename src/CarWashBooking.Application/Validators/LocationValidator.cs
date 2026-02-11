using CarWashBooking.Application.DTOs;
using FluentValidation;

namespace CarWashBooking.Application.Validators;

public class CreateLocationDtoValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
    }
}

public class UpdateLocationDtoValidator : AbstractValidator<UpdateLocationDto>
{
    public UpdateLocationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
    }
}
