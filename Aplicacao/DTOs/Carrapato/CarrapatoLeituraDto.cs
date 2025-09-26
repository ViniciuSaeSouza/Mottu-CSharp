namespace Aplicacao.DTOs.Carrapato;

public record CarrapatoLeituraDto(
    int Id,
    string CodigoSerial,
    string StatusBateria,
    string StatusDeUso,
    int IdPatio
);

