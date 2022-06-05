using CarDirectrory.Core.Cars.Models;
using FluentValidation;

namespace CarDirectrory.Core.Cars.Validators;

public class UpdateCarValidator : AbstractValidator<UpdateCarModel>
{
    public UpdateCarValidator()
    {
        RuleFor(it => it.ReleaseYear)
            .GreaterThanOrEqualTo(1950).WithMessage("Год выпуска автомобиля не может быть меньше 1950");
        RuleFor(it => it.ReleaseYear)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Год выпуска автомобиля превышает текущий год");
    }
}