using Microsoft.AspNetCore.Mvc;
using API.Servicos;
using Asp.Versioning;
using Aplicacao.DTOs.Usuario;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using Aplicacao.Abstracoes;

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
        [HttpPost("login", Name = nameof(Login))]
        [ProducesResponseType(typeof(Recurso<AuthTokenDto>), 200)]
        [ProducesResponseType(401)]
        public ActionResult<Recurso<AuthTokenDto>> Login([FromBody] UsuarioLoginDto loginDto)
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
                
                var dto = new AuthTokenDto
                {
                    Token = token,
                    TipoToken = "Bearer",
                    ExpiracaoEm = 3600,
                    Usuario = loginDto.email,
                    DataGeracao = DateTime.UtcNow
                };

                var recurso = new Recurso<AuthTokenDto>
                {
                    Dados = dto,
                    Links = CriarLinksAuth()
                };


                // Keep compatibility for existing clients/tests that expect token at root
                var compat = new
                {
                    token = dto.Token,
                    tipoToken = dto.TipoToken,
                    expiracaoEm = dto.ExpiracaoEm,
                    usuario = dto.Usuario,
                    dataGeracao = dto.DataGeracao,
                    links = CriarLinksAuth()
                };

                return Ok(compat);
            }


            return Unauthorized(new { Mensagem = "Usuário ou senha inválidos" });
        }

        /// <summary>
        /// Valida um token JWT
        /// </summary>
        /// <param name="token">Token JWT para validação</param>
        /// <returns>Status de validade do token</returns>
        [HttpPost("validar-token", Name = nameof(ValidarToken))]
        [ProducesResponseType(typeof(Recurso<ValidacaoTokenDto>), 200)]
        [ProducesResponseType(400)]
        public ActionResult<Recurso<ValidacaoTokenDto>> ValidarToken([FromBody] string token)
        {
            var isValid = _jwtService.ValidarToken(token);
            
            var dto = new ValidacaoTokenDto { Valido = isValid, Mensagem = isValid ? "Token válido" : "Token inválido ou expirado" };
            
            var recurso = new Recurso<ValidacaoTokenDto>
            {
                Dados = dto,
                Links = CriarLinksValidate()
            };

            if (isValid)
            {
                return Ok(recurso);
            }


            return BadRequest(recurso);
        }

        private List<Link> CriarLinksAuth()
        {
            return new List<Link>
            {
                new Link { Rel = "self", Href = Url.Link(nameof(Login), null) ?? string.Empty, Method = "POST" },
                new Link { Rel = "validar-token", Href = Url.Link(nameof(ValidarToken), null) ?? string.Empty, Method = "POST" }
            };
        }

        private List<Link> CriarLinksValidate()
        {
            return new List<Link>
            {
                new Link { Rel = "self", Href = Url.Link(nameof(ValidarToken), null) ?? string.Empty, Method = "POST" },
                new Link { Rel = "login", Href = Url.Link(nameof(Login), null) ?? string.Empty, Method = "POST" }
            };
        }
    }
}
