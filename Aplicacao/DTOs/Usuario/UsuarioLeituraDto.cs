using Aplicacao.DTOs.Patio;

namespace Aplicacao.DTOs.Usuario;

public record UsuarioLeituraDto(int IdUsuario, string Nome, string Email, string Senha, string NomePatio, int IdPatio);