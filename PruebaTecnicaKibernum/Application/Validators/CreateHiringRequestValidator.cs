using FluentValidation;
using PruebaTecnicaKibernum.Application.Dtos.HiringDto;

namespace PruebaTecnicaKibernum.Application.Validators
{
    /// <summary>
    /// Validador para la creación de solicitudes de contratación
    /// </summary>
    /// <remarks>
    /// Este validador define las reglas de negocio básicas para garantizar
    /// que los datos de entrada sean válidos antes de procesar la solicitud
    /// </remarks>
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
