using PricingLibrary.DataClasses;
using BacktestConsole.ParsingTools;
using BacktestConsole.HedgingStrategyBacktest;
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

            JsonHandler JsonHandler = new JsonHandler();
            ParserCsv ParserCsv = new ParserCsv();
            RebalacingStrategy RebalancingStrategy = new RebalacingStrategy();
            var BasketTestParameters = JsonHandler.LoadTestParameters(TestParamsFile);
            var LstDF = ParserCsv.ConstructDataFeedFromCsv(MktDataFile);
            List<OutputData> Results = RebalancingStrategy.RegularRebalacingStrategy(BasketTestParameters, LstDF);

            /*affichage des résultats*/
            foreach (OutputData result in Results)
            {
                Console.WriteLine($"{result.Date}, {result.Value}, {result.Price}"); 
            }

            /*Sauvegarde des résultats dans un fichier JSON*/
            JsonHandler.SaveResultsToFile(Results, OutputFile);
        }


    }
}
