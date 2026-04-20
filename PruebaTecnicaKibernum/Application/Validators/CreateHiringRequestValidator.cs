using FluentValidation;
using PruebaTecnicaKibernum.Application.Dtos.HiringDto;

namespace PruebaTecnicaKibernum.Application.Validators
{
    public class CreateHiringRequestValidator : AbstractValidator<CreateHiringRequestDto>
    {
        public CreateHiringRequestValidator()
        {
            RuleFor(x => x.CharacterId).GreaterThan(0);
            RuleFor(x => x.Applicant).NotEmpty();
            RuleFor(x => x.Event).NotEmpty();
        }
    }
}
