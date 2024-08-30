        static List<OutputData> RunBacktest(BasketTestParameters testParams, IEnumerable<ShareValue> marketData)
        {
            var results = new List<OutputData>();

            // Initialize the pricer and rebalancing oracle
            var pricer = new Pricer(testParams);
            var rebalancingOracle = testParams.RebalancingOracleDescription;

            double portfolioValue = 1000;

            var basket = testParams.BasketOption;
            var basketWeights = basket.Weights.ToArray();

            var initialShareValues = new Dictionary<string, double>();

            foreach (var data in marketData)
            {
                DateTime date = data.DateOfPrice;

                if (initialShareValues.Count == 0)
                {
                    foreach (var share in basket.UnderlyingShareIds)
                    {
                        initialShareValues[share] = marketData.First(md => md.Id == share && md.DateOfPrice == date).Value;
                    }
                }

                // Rebalance the portfolio if necessary (based on the oracle)
                if (ShouldRebalance(date, rebalancingOracle))
                {
                    // Here you would calculate the new weights, if rebalancing is required
                    // For simplicity, we assume weights stay the same in this example
                }

                // Calculate the current portfolio value based on the latest share prices
                double currentPortfolioValue = 0;
                for (int i = 0; i < basket.UnderlyingShareIds.Count; i++)
                {
                    string shareId = basket.UnderlyingShareIds[i];
                    double currentShareValue = marketData.First(md => md.Id == shareId && md.Date == date).Value;
                    currentPortfolioValue += basketWeights[i] * (currentShareValue / initialShareValues[shareId]) * portfolioValue;
                }

                // Update portfolio value
                portfolioValue = currentPortfolioValue;

                // Record the output data for this date
                results.Add(new OutputData
                {
                    Date = date,
                    PortfolioValue = portfolioValue
                });
            }

            return results;
        }
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacktestConsole
{
    internal class draft
    {
    }
}
