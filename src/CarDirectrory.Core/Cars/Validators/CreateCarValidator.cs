using CarDirectrory.Core.Cars.Models;
using FluentValidation;

namespace CarDirectrory.Core.Cars.Validators;

public class CreateCarValidator : AbstractValidator<CreateCarModel>
{
    public CreateCarValidator()
    {
        RuleFor(it => it.StateNumber)
            .Matches(@"^[АВЕКМНОРСТУХ]{2}\d{3}(?<!000)[АВЕКМНОРСТУХ]{1}\d{2,3}$")
            .WithMessage("Неверный формат номера");
        RuleFor(it => it.ReleaseYear)
            .GreaterThanOrEqualTo(1950).WithMessage("Год выпуска автомобиля не может быть меньше 1950");
        RuleFor(it => it.ReleaseYear)
            .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Год выпуска автомобиля превышает текущий год");
    }
}