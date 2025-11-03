using Microsoft.AspNetCore.Mvc;
using API.Servicos;
using Asp.Versioning;
using Aplicacao.DTOs.Usuario;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;

namespace API.Controladores
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Gera um token JWT para autenticação
        /// </summary>
        /// <param name="loginDto">Dados de login do usuário</param>
        /// <returns>Token JWT válido</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthTokenDto), 200)]
        [ProducesResponseType(401)]
        public ActionResult<AuthTokenDto> Login([FromBody] UsuarioLoginDto loginDto)
        {
            var usuariosValidos = new Dictionary<string, string>
            {
                { "admin", "admin123" },
                { "mottu", "mottu2024" },
                { "demo", "demo123" },
                { "test", "test123" },
                { "admin@mottu.com", "admin123" },
                { "mottu@mottu.com", "mottu2024" },
                { "demo@mottu.com", "demo123" },
                { "test@mottu.com", "test123" }
            };

            if (usuariosValidos.TryGetValue(loginDto.email, out var senhaCorreta) && 
                senhaCorreta == loginDto.senha)
            {
                var token = _jwtService.GerarToken(loginDto.email);
                
                return Ok(new AuthTokenDto
                {
                    Token = token,
                    TipoToken = "Bearer",
                    ExpiracaoEm = 3600,
                    Usuario = loginDto.email,
                    DataGeracao = DateTime.UtcNow
                });
            }

            return Unauthorized(new ApiMensagemDto { Mensagem = "Usuário ou senha inválidos" });
        }

        /// <summary>
        /// Valida um token JWT
        /// </summary>
        /// <param name="token">Token JWT para validação</param>
        /// <returns>Status de validade do token</returns>
        [HttpPost("validar-token")]
        [ProducesResponseType(typeof(ValidacaoTokenDto), 200)]
        [ProducesResponseType(400)]
        public ActionResult<ValidacaoTokenDto> ValidarToken([FromBody] string token)
        {
            var isValid = _jwtService.ValidarToken(token);
            
            if (isValid)
            {
                return Ok(new ValidacaoTokenDto { Valido = true, Mensagem = "Token válido" });
            }

            return BadRequest(new ValidacaoTokenDto { Valido = false, Mensagem = "Token inválido ou expirado" });
        }
    }
}
