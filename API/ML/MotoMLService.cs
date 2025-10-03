using Microsoft.ML;
using Microsoft.ML.Data;

namespace API.ML
{
    public class MotoManutencaoData
    {
        [LoadColumn(0)]
        public float KmRodados { get; set; }
        
        [LoadColumn(1)]
        public float TempoUso { get; set; } // em meses
        
        [LoadColumn(2)]
        public float NumeroViagens { get; set; }
        
        [LoadColumn(3)]
        public float IdadeVeiculo { get; set; } // em anos
        
        [LoadColumn(4)]
        public bool PrecisaManutencao { get; set; }
    }

    public class MotoManutencaoPredicao
    {
        [ColumnName("PredictedLabel")]
        public bool PrecisaManutencao { get; set; }
        
        [ColumnName("Probability")]
        public float Probabilidade { get; set; }
        
        [ColumnName("Score")]
        public float Score { get; set; }
    }

    public class MotoMLService
    {
        private readonly MLContext _mlContext;
        private ITransformer? _model;

        public MotoMLService()
        {
            _mlContext = new MLContext(seed: 1);
            TreinarModelo();
        }

        private void TreinarModelo()
        {
            // Dados de exemplo para treinamento do modelo
            var dadosTreinamento = new List<MotoManutencaoData>
            {
                new() { KmRodados = 15000, TempoUso = 12, NumeroViagens = 300, IdadeVeiculo = 2, PrecisaManutencao = true },
                new() { KmRodados = 8000, TempoUso = 6, NumeroViagens = 150, IdadeVeiculo = 1, PrecisaManutencao = false },
                new() { KmRodados = 25000, TempoUso = 18, NumeroViagens = 500, IdadeVeiculo = 3, PrecisaManutencao = true },
                new() { KmRodados = 5000, TempoUso = 3, NumeroViagens = 80, IdadeVeiculo = 1, PrecisaManutencao = false },
                new() { KmRodados = 18000, TempoUso = 15, NumeroViagens = 400, IdadeVeiculo = 2.5f, PrecisaManutencao = true },
                new() { KmRodados = 12000, TempoUso = 8, NumeroViagens = 200, IdadeVeiculo = 1.5f, PrecisaManutencao = false },
                new() { KmRodados = 30000, TempoUso = 24, NumeroViagens = 600, IdadeVeiculo = 4, PrecisaManutencao = true },
                new() { KmRodados = 7000, TempoUso = 5, NumeroViagens = 120, IdadeVeiculo = 1, PrecisaManutencao = false },
                new() { KmRodados = 22000, TempoUso = 20, NumeroViagens = 450, IdadeVeiculo = 3.5f, PrecisaManutencao = true },
                new() { KmRodados = 10000, TempoUso = 7, NumeroViagens = 180, IdadeVeiculo = 1.2f, PrecisaManutencao = false }
            };

            var dataView = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);

            // Pipeline de processamento de dados
            var pipeline = _mlContext.Transforms.Concatenate("Features", 
                    nameof(MotoManutencaoData.KmRodados),
                    nameof(MotoManutencaoData.TempoUso),
                    nameof(MotoManutencaoData.NumeroViagens),
                    nameof(MotoManutencaoData.IdadeVeiculo))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: nameof(MotoManutencaoData.PrecisaManutencao),
                    featureColumnName: "Features"));

            // Treinar o modelo
            _model = pipeline.Fit(dataView);
        }

        public MotoManutencaoPredicao PredizirManutencao(float kmRodados, float tempoUso, float numeroViagens, float idadeVeiculo)
        {
            if (_model == null)
                throw new InvalidOperationException("Modelo não foi treinado");

            var engine = _mlContext.Model.CreatePredictionEngine<MotoManutencaoData, MotoManutencaoPredicao>(_model);
            
            var input = new MotoManutencaoData
            {
                KmRodados = kmRodados,
                TempoUso = tempoUso,
                NumeroViagens = numeroViagens,
                IdadeVeiculo = idadeVeiculo
            };

            var predicted = engine.Predict(input);

            // Heurísticas de domínio para melhorar previsibilidade nos extremos
            var baixaUtilizacao = kmRodados <= 5000f && tempoUso <= 6f && numeroViagens <= 200f && idadeVeiculo <= 1.5f;
            var altaUtilizacao  = kmRodados >= 25000f || tempoUso >= 24f || numeroViagens >= 800f || idadeVeiculo >= 5f;

            // Limite mais conservador para indicar manutenção
            var threshold = 0.75f;
            var precisa = predicted.Probabilidade >= threshold;

            if (baixaUtilizacao)
                precisa = false;
            else if (altaUtilizacao)
                precisa = true;

            return new MotoManutencaoPredicao
            {
                PrecisaManutencao = precisa,
                Probabilidade = predicted.Probabilidade,
                Score = predicted.Score
            };
        }

        public string AvaliarRiscoManutencao(MotoManutencaoPredicao predicao)
        {
            return predicao.Probabilidade switch
            {
                >= 0.8f => "Alto risco - Manutenção urgente recomendada",
                >= 0.6f => "Médio risco - Agendar manutenção em breve",
                >= 0.4f => "Baixo risco - Monitorar periodicamente",
                _ => "Muito baixo risco - Veículo em bom estado"
            };
        }
    }
}
