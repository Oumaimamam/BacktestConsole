﻿//using PricingLibrary.DataClasses;
//using BacktestConsole.DataFilesHandlers;
//using BacktestConsole.HedgingStrategyBacktest;
using BacktestConsoleLibrary.HedgingStrategyBacktest;
using BacktestConsoleLibrary.DataFilesHandlers;
using PricingLibrary.DataClasses;
using PricingLibrary.RebalancingOracleDescriptions;

namespace BacktestConsole
{
    static class  Program
    {
        /*Méthode Main*/
        static void Main(string[] args)
        {
            string TestParamsFile = args[0];
            string MktDataFile = args[1];
            string OutputFile = args[2];

            JsonHandler jsonHandler = new JsonHandler();
            CsvParser csvParser = new CsvParser();
            
            var BasketTestParameters = jsonHandler.LoadTestParameters(TestParamsFile);
            var LstDF = csvParser.ConstructDataFeedFromCsv(MktDataFile);
            RegularOracleDescription RegularOracle = (RegularOracleDescription)BasketTestParameters.RebalancingOracleDescription;
            int period = RegularOracle.Period;
            RebalancingStrategy rebalancingStrategy = new RebalancingStrategy(BasketTestParameters, LstDF);
            List<OutputData> Results = rebalancingStrategy.RegularRebalancingStrategy(period);

            /*Sauvegarde des résultats dans un fichier JSON*/
            jsonHandler.SaveResultsToFile(Results, OutputFile);
        }
    }
}
