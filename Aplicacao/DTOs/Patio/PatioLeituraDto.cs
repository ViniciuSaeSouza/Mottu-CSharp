
using Aplicacao.DTOs.Moto;
using Aplicacao.DTOs.Usuario;

namespace Aplicacao.DTOs.Patio;

public record PatioLeituraDto(
    int Id,
    string Nome,
    string Endereco,
    List<MotoLeituraDto> Motos,
    List<UsuarioLeituraDto> Usuarios);