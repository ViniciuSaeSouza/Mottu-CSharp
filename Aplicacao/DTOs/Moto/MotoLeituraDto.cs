using Dominio.Enumeradores;

namespace Aplicacao.DTOs.Moto;

public record MotoLeituraDto(int Id, string Placa, string Modelo, string NomeFilial, string Chassi, ZonaEnum Zona);