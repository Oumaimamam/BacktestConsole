using Grpc.Core;
using GrpcBacktestServer.Protos;

namespace GrpcBacktestServer.Services
{
    public class BacktestService : BacktestRunner.BacktestRunnerBase  // Votre service doit hériter de la classe générée à partir de votre proto
    {
        public override Task<BacktestOutput> RunBacktest(BacktestRequest request, ServerCallContext context)
        {
            // Implémentez la logique de traitement du backtest ici
            var response = new BacktestOutput
            {
                // Remplissez la réponse en fonction des résultats de votre backtest
            };
            return Task.FromResult(response);
        }
    }
}