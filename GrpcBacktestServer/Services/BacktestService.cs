using Grpc.Core;
using GrpcBacktestServer.Protos;

namespace GrpcBacktestServer.Services
{
    public class BacktestService : BacktestRunner.BacktestRunnerBase
    {
        public override Task<BacktestOutput> RunBacktest(BacktestRequest request, ServerCallContext context)
        {
            var testParams = request.TstParams;
            var dataParams = request.Data;

            if (request.RebParams.Regular != null)
            {
                // regular rebalancing
                var period = request.RebParams.Regular.Period;
                Console.WriteLine($"Regular rebalancing with period: {period}");

                // Perform your backtest logic for regular rebalancing here
                // You may need to adjust your strategy based on the period value
            }
            else if (request.RebParams.Weekly != null)
            {
                // Handle weekly rebalancing
                var dayOfWeek = request.RebParams.Weekly.Day;
                Console.WriteLine($"Weekly rebalancing on: {dayOfWeek}");

                // Perform your backtest logic for weekly rebalancing here
            }
            else
            {
                // Handle case where no valid rebalancing type is provided
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid rebalancing type provided"));
            }

            var output = new BacktestOutput
            {
                // Populate with the actual backtest result data
            };

            return Task.FromResult(output);
        }
    }
}
