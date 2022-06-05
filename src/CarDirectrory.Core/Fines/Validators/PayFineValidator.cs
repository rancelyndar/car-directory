using CarDirectrory.Core.Fines.Models;
using FluentValidation;

namespace CarDirectrory.Core.Fines.Validators;

public class PayFineValidator : AbstractValidator<PayFineModel>
{
    public PayFineValidator()
    {
        RuleFor(it => it.IsPayed).Equal(false).WithMessage("Штраф уже оплачен");
    }
}