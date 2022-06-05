using CarDirectrory.Core.Fines.Models;
using FluentValidation;

namespace CarDirectrory.Core.Fines.Validators;

public class CreateFineValidator : AbstractValidator<CreateFineModel>
{
    public CreateFineValidator()
    {
        RuleFor(it => it.Price)
            .GreaterThan(0)
            .WithMessage("Размер штрафа не может быть меньше или равен нулю");
    }
}