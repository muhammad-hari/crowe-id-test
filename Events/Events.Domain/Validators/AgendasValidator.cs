using Events.Domain.Models;
using FluentValidation;

namespace Events.Domain.Validators
{
    public class AgendasValidator : AbstractValidator<AgendasModel>
    {
        public AgendasValidator()
        {
            RuleFor(p => p.Id).NotEmpty().WithMessage("id is required.");
            RuleFor(p => p.Name).NotEmpty().WithMessage("name is required.");
            RuleFor(p => p.StartDate).NotEmpty().WithMessage("start date must be set.");
            RuleFor(p => p.EndDate).NotEmpty().WithMessage("start date must be set.");
            RuleFor(p => p.StartDate).LessThan(p => p.EndDate).WithMessage("start date must be less than end date");
            RuleFor(p => p.EndDate).GreaterThan(p => p.StartDate).WithMessage("end date must be greater than start date");
        }
    }
}
