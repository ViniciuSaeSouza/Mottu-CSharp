using System.ComponentModel.DataAnnotations;

namespace Aplicacao.DTOs.Moto;
public record MotoCriarDto (string? Placa, string? Chassi, int IdPatio);
