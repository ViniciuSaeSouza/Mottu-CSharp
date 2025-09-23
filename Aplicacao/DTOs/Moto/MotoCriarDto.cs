using System.ComponentModel.DataAnnotations;

namespace Aplicacao.DTOs.Moto;
public record MotoCriarDto (string Placa, string Modelo,  string Chassi,int IdFilial);