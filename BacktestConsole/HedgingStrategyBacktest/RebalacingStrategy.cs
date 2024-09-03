using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using BacktestConsole.Utils;
using PricingLibrary.RebalancingOracleDescriptions;
namespace BacktestConsole.HedgingStrategyBacktest

{
    /*Méthode pour effectuer le backtest de la stratégie de Delta-Hedging*/
    public class RebalacingStrategy
    { 
        public List<OutputData> RegularRebalacingStrategy(BasketTestParameters Testparameters, List<DataFeed> LstDF)
        {
            List<OutputData> Results = new List<OutputData>(); 
            PricingResults Result = new Pricer(Testparameters).Price(LstDF[0].Date, LstDF[0].PriceList.Values.ToArray());
            double Cash = Result.Price - DictionaryUtils.ScalarProduct(new DictDeltaMaker().Make(Result.Deltas, Testparameters), LstDF[0].PriceList);
            Portfolio Portfolio = new Portfolio(new DictDeltaMaker().Make(Result.Deltas, Testparameters),LstDF[0], Cash);
            OutputData output = new OutputData
            {
                Date = LstDF[0].Date,
                Value = Result.Price,
                Price = Result.Price,
                Deltas = Result.Deltas,
                DeltasStdDev = Result.DeltaStdDev,
                PriceStdDev = Result.PriceStdDev
            };
            Results.Add(output);
            RegularOracleDescription RegularOracle = (RegularOracleDescription)Testparameters.RebalancingOracleDescription;
            int period = RegularOracle.Period;


            for (int i = period; i < LstDF.Count; i += period)
            {
                Portfolio.ComputeValue(LstDF[i]);
                Result = new Pricer(Testparameters).Price(LstDF[i].Date, LstDF[i].PriceList.Values.ToArray());
                Dictionary<string, double> NewComposition = new DictDeltaMaker().Make(Result.Deltas, Testparameters);
                Portfolio.UpdatePortfolio(NewComposition, LstDF[i]);
                OutputData OutputData = new OutputData
                {
                    Date = LstDF[i].Date,
                    Value = Portfolio.ComputeValue(LstDF[i]),
                    Price = Result.Price,
                    Deltas = Result.Deltas,
                    DeltasStdDev = Result.DeltaStdDev,
                    PriceStdDev = Result.PriceStdDev
                };
                Results.Add(OutputData);
            }
            return Results;
        }
    }
}
