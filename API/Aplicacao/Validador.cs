using API.Application.DTOs.Moto;
using FluentValidation;


// CLASE APENAS DE TESTE E EXEMPLO
namespace API.Aplicacao
{
    public class Validador : AbstractValidator<MotoCriarDto>
    {
        public Validador()
        {
            RuleFor(c => c.Placa)
            .NotEmpty()
                                            .WithMessage("A placa é obrigatória.")
                                            .Matches(@"^[A-Z]{3}\d{4}$|^[A-Z]{3}[0-9][A-Z][0-9]{2}$").WithMessage("A placa deve estar no formato válido (ABC1234 ou ABC1D23).");
            RuleFor(c => c.Modelo).NotEmpty().WithMessage("Motorização é obrigatória.").MaximumLength(50).WithMessage("Motorização deve ter no máximo 50 caracteres.");
            RuleFor(c => c.IdFilial).IsInEnum().WithMessage("Marcha inválida.");
            RuleFor(c => c.Modelo).NotEmpty().WithMessage("Marca do carro é obrigatória.");
        }
    }
}


