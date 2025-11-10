using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.ML;
using Asp.Versioning;
using API.ML.DTOs;

namespace API.Controladores
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/predicao")]
    [Authorize]
    public class PredicaoControlador : ControllerBase
    {
        private const int MAX_FLEET_SIZE = 1000;
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
        [HttpPost("manutencao-moto")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult PredizirManutencao([FromBody] DadosPredicaoDto dadosMoto)
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

                return Ok(new PredicaoResultadoDto
                {
                    PrecisaManutencao = predicao.PrecisaManutencao,
                    Probabilidade = Math.Round(predicao.Probabilidade * 100, 1),
                    Score = Math.Round(predicao.Score, 4),
                    AvaliacaoRisco = avaliacaoRisco,
                    Recomendacao = recomendacao,
                    DataAnalise = DateTime.UtcNow
                });
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
        [HttpPost("analise-frota")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public IActionResult AnalisarFrota([FromBody] List<MotoFrotaDto> frota)
        {
            if (frota == null || frota.Count == 0)
                return BadRequest(new { erro = "A lista de motos não pode ser vazia" });

            if (frota.Count > MAX_FLEET_SIZE)
                return BadRequest(new { erro = $"O tamanho máximo da frota é {MAX_FLEET_SIZE}" });

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

                return Ok(resumo);
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
    }
}
