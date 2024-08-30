using BacktestConsole.ParsingTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;
using PricingLibrary.TimeHandler;
using PricingLibrary.Utilities;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization.Json;

namespace BacktestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var testParams = SampleTestParameters.Sample();
            var marketData = SampleMarketData.Sample();
            var results = RunBacktest(testParams, marketData);
            //affichage des résultats
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
            SaveResultsToFile(results, "output_file.json");
        }
        static List<OutputData> RunBacktest(BasketTestParameters testParams, IEnumerable<ShareValue> marketData)
        {
            var results = new List<OutputData>();
            double portfolioValue = 1000;
            var weights = testParams.BasketOption.Weights;
            foreach (var data in marketData)
            {
                double newPortfolioValue = 0;
                for (int i=0; i < data.Count; i++) 
                {
                    newPortfolioValue += portfolioValue * weights[i] * portfolioValue;
                }
                portfolioValue = newPortfolioValue;
                results.Add(new OutputData
                {
                    Date = data.Date,
                    PortfolioValue = portfolioValue
                });
            }
            return results;
        }
        static void SaveResultsToFile(List<OutputData> results, string filePath)
        {
            var jsonOptions = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(results, jsonOptions);
            File.WriteAllText(filePath, json);
        }
    }

}
