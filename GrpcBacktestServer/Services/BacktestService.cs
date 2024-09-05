using GrpcBacktestServer.Protos;
using BacktestConsoleLibrary.HedgingStrategyBacktest;
using BacktestConsoleLibrary.DataFilesHandlers;
using Grpc.Core;
using System.Threading.Tasks;

namespace GrpcBacktestServer.Services
{
    public class BacktestService : BacktestRunner.BacktestRunnerBase
    {
        private readonly RebalancingStrategy _rebalancingStrategy;
        private readonly JsonHandler _jsonHandler;
        private readonly CsvParser _csvParser;

        // Constructor where we initialize the objects from the library
        public BacktestService()
        {
            // Assuming you have some default parameters for JsonHandler and CsvParser
            _jsonHandler = new JsonHandler();
            _csvParser = new CsvParser();
        }

        public override async Task<BacktestOutput> RunBacktest(BacktestRequest request, ServerCallContext context)
        {
            // Load the test parameters from the request
            var testParams = _jsonHandler.LoadTestParameters(request.TstParams);
            var marketData = _csvParser.ConstructDataFeedFromCsv(request.Data.CsvFilePath);

            // Initialize the strategy with test parameters and market data
            var rebalancingStrategy = new RebalancingStrategy(testParams, marketData);

            // Run the backtest (you might need to modify this depending on your library’s structure)
            var results = rebalancingStrategy.RegularRebalancingStrategy();

            // Prepare the response
            var response = new BacktestOutput();
            foreach (var result in results)
            {
                response.BacktestInfo.Add(new BacktestInfo
                {
                    Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(result.Date.ToUniversalTime()),
                    PortfolioValue = result.Value,
                    Price = result.Price,
                    DeltaStdDev = result.DeltasStdDev,
                    PriceStdDev = result.PriceStdDev,
                    Delta = { result.Deltas }
                });
            }

            return await Task.FromResult(response);
        }
    }
}
