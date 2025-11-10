using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.ML;
using Asp.Versioning;
using API.ML.DTOs;
using Aplicacao.Abstracoes;

namespace API.Controladores
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/predicao")]
    [Authorize]
    public class PredicaoControlador : ControllerBase
    {
        private readonly MotoMLService _motoMlService;

        public PredicaoControlador(MotoMLService motoMlService)
        {
            _motoMlService = motoMlService;
        }

        /// <summary>
        /// Prediz se uma moto precisa de manutenção com base nos dados fornecidos
        /// </summary>
        /// <param name="dadosMoto">Dados da moto para análise</param>
        /// <returns>Predição de manutenção</returns>
        [HttpPost("manutencao-moto", Name = nameof(PredizirManutencao))]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public ActionResult<Recurso<PredicaoResultadoDto>> PredizirManutencao([FromBody] DadosPredicaoDto dadosMoto)
        {
            try
            {
                var predicao = _motoMlService.PredizirManutencao(
                    dadosMoto.KmRodados,
                    dadosMoto.TempoUso,
                    dadosMoto.NumeroViagens,
                    dadosMoto.IdadeVeiculo);

                var avaliacaoRisco = _motoMlService.AvaliarRiscoManutencao(predicao);
                
                var recomendacao = predicao.PrecisaManutencao
                    ? $"Manutenção urgente necessária. Veículo com {dadosMoto.KmRodados:N0} km rodados."
                    : $"Veículo em bom estado. Próxima revisão recomendada em {10000 - (dadosMoto.KmRodados % 10000):N0} km.";

                var resultado = new PredicaoResultadoDto
                {
                    PrecisaManutencao = predicao.PrecisaManutencao,
                    Probabilidade = Math.Round(predicao.Probabilidade * 100, 1),
                    Score = Math.Round(predicao.Score, 4),
                    AvaliacaoRisco = avaliacaoRisco,
                    Recomendacao = recomendacao,
                    DataAnalise = DateTime.UtcNow
                };

                var recurso = new Recurso<PredicaoResultadoDto>
                {
                    Dados = resultado,
                    Links = CriarLinksPredicao()
                };

                return Ok(recurso);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Erro ao processar predição", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Analisa uma frota de motos e retorna um ranking de prioridade de manutenção (Requer autenticação JWT)
        /// </summary>
        /// <param name="frota">Lista de motos para análise</param>
        /// <returns>Análise da frota com ranking de prioridade</returns>
        [HttpPost("analise-frota", Name = nameof(AnalisarFrota))]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public ActionResult<Recurso<PredicaoAnaliseFrotaResumoDto>> AnalisarFrota([FromBody] List<MotoFrotaDto> frota)
        {
            try
            {
                var resultados = new List<AnaliseMotoDto>();

                foreach (var moto in frota)
                {
                    var predicao = _motoMlService.PredizirManutencao(
                        moto.KmRodados,
                        moto.TempoUso,
                        moto.NumeroViagens,
                        moto.IdadeVeiculo);

                    var avaliacaoRisco = _motoMlService.AvaliarRiscoManutencao(predicao);

                    resultados.Add(new AnaliseMotoDto
                    {
                        Id = moto.Id,
                        Placa = moto.Placa,
                        PrecisaManutencao = predicao.PrecisaManutencao,
                        Probabilidade = Math.Round(predicao.Probabilidade * 100, 1),
                        Score = Math.Round(predicao.Score, 4),
                        AvaliacaoRisco = avaliacaoRisco,
                        Prioridade = CalcularPrioridade(predicao.Probabilidade, moto.KmRodados, moto.IdadeVeiculo)
                    });
                }

                // Ordenar por prioridade (maior prioridade primeiro)
                var ranking = resultados.OrderByDescending(r => r.Prioridade).ToList();

                var resumo = new PredicaoAnaliseFrotaResumoDto
                {
                    TotalMotos = resultados.Count,
                    MotosComManutencao = resultados.Count(r => r.PrecisaManutencao),
                    PercentualManutencao = resultados.Count == 0 ? 0 : Math.Round((double)resultados.Count(r => r.PrecisaManutencao) / resultados.Count * 100, 1),
                    DataAnalise = DateTime.UtcNow,
                    Ranking = ranking
                };

                var recurso = new Recurso<PredicaoAnaliseFrotaResumoDto>
                {
                    Dados = resumo,
                    Links = CriarLinksAnaliseFrota()
                };

                return Ok(recurso);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Erro ao analisar frota", detalhes = ex.Message });
            }
        }

        private static int CalcularPrioridade(float probabilidade, float kmRodados, float idadeVeiculo)
        {
            // Priorizacao simples ponderando probabilidade, quilometragem e idade do veiculo
            var score = (probabilidade * 0.6) + (Math.Min(kmRodados / 30000.0, 1.0) * 0.25) + (Math.Min(idadeVeiculo / 5.0, 1.0) * 0.15);
            return (int)Math.Round(score * 100, 0); // retorno como inteiro percentual 0..100
        }

        private List<Link> CriarLinksPredicao()
        {
            return new List<Link>
            {
                new Link { Rel = "self", Href = Url.Link(nameof(PredizirManutencao), null) ?? string.Empty, Method = "POST" },
                new Link { Rel = "analise-frota", Href = Url.Link(nameof(AnalisarFrota), null) ?? string.Empty, Method = "POST" }
            };
        }

        private List<Link> CriarLinksAnaliseFrota()
        {
            return new List<Link>
            {
                new Link { Rel = "self", Href = Url.Link(nameof(AnalisarFrota), null) ?? string.Empty, Method = "POST" },
                new Link { Rel = "predicao-moto", Href = Url.Link(nameof(PredizirManutencao), null) ?? string.Empty, Method = "POST" }
            };
        }
    }
}
