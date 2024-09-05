using Grpc.Core;
using GrpcBacktestServer.Protos;
using PricingLibrary.DataClasses;
using PricingLibrary.Utilities;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.DataClasses;
using PricingLibrary.Computations;
using BacktestConsoleLibrary.HedgingStrategyBacktest;

namespace GrpcBacktestServer.Services
{
    public class BacktestService : BacktestRunner.BacktestRunnerBase
    {
        public override Task<BacktestOutput> RunBacktest(BacktestRequest request, ServerCallContext context)
        {
            var testParams = request.TstParams;
            var dataParams = request.Data;

            if (testParams.RebParams.Regular != null)
            {
                // regular rebalancing
                var period = testParams.RebParams.Regular.Period;
                Console.WriteLine($"Regular rebalancing with period: {period}");

                // Perform your backtest logic for regular rebalancing here
                // You may need to adjust your strategy based on the period value
                RebalancingStrategy rebalancingStrategy = new RebalancingStrategy(testParams, LstDF);
                List<OutputData> Results = rebalancingStrategy.RegularRebalancingStrategy(period);

            }

            var output = new BacktestOutput
            {
                output.Date  = DateTime.Now.ToString("yyyy-MM-dd"),
            };

            return Task.FromResult(output);
        }
    }
}
