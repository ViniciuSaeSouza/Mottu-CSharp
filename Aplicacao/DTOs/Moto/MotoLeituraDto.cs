using Dominio.Enumeradores;

namespace Aplicacao.DTOs.Moto;

public record MotoLeituraDto(int Id, string Placa, string Modelo, string NomePatio, string Chassi, ZonaEnum Zona);