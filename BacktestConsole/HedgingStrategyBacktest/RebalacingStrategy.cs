using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;
namespace BacktestConsole.HedgingStrategyBacktest
{
    /*Méthode pour effectuer le backtest de la stratégie de Delta-Hedging*/
    public class RebalacingStrategy
    { 
        static double ComputePortfolioValue(double[] deltas, double[] spots, double cash)
        {
            double value = cash;
            for (int i = 0; i < deltas.Length; i++)
            {
                value += deltas[i] * spots[i];
            }
            return value;
        }
        static double ComputeCash(double[] deltas_prec, double[] deltas_curr, double cash_prec, double[] spots)
        {
            double cash_curr = cash_prec;
            for (int i = 0; i < deltas_prec.Length; i++)
            {
                cash_curr += (deltas_prec[i] - deltas_curr[i]) * spots[i];
            }
            return cash_curr;
        }

        public List<OutputData> RegularRebalacingStrategy(BasketTestParameters Testparameters, List<DataFeed> LstDF)
        {
            /*Composition portefeuille en t0*/
            List<OutputData> Results = new List<OutputData>();
            Dictionary<string, double> spotsDict = LstDF[0].PriceList;
            double[] spots = spotsDict.Values.ToArray();
            Pricer pricer = new Pricer(Testparameters);
            DateTime date_0 = LstDF[0].Date;
            PricingResults result = pricer.Price(date_0, spots);
            double p0 = result.Price;
            double[] deltas = result.Deltas;
            double CashZero = p0;

            for (int i = 0; i < spots.Length; i++)
            {
                CashZero -= deltas[i] * spots[i];
            }

            OutputData output = new OutputData
            {
                Date = date_0,
                Value = p0,
                Price = p0,
                Deltas = deltas,
                DeltasStdDev = result.DeltaStdDev,
                PriceStdDev = result.PriceStdDev
            };
            Results.Add(output);

            /////////////////////////////////////////////////////////////////////////////////////////////////////
            var deltas_prec = deltas;
            double cash_prec = CashZero;
            DateTime date_prec = date_0;
            IRebalancingOracleDescription rebalancingOracle = Testparameters.RebalancingOracleDescription;
            // regular rebalancing

            RegularOracleDescription RegularOracle = (RegularOracleDescription)rebalancingOracle;
            int period = RegularOracle.Period;


            for (int i = period; i < LstDF.Count; i += period)
            {
                DataFeed datafeed = LstDF[i];
                spots = datafeed.PriceList.Values.ToArray();
                result = pricer.Price(datafeed.Date, spots);
                double price = result.Price;
                var deltas_curr = result.Deltas;
                double val_curr = 0;

                for (int j = 0; j < spots.Length; j++)
                {
                    val_curr += deltas_prec[j] * spots[j];
                }

                double capitalisation = RiskFreeRateProvider.GetRiskFreeRateAccruedValue(date_prec, datafeed.Date);
                val_curr += cash_prec * capitalisation;
                var cash_curr = cash_prec * capitalisation;

                for (int k = 0; k < spots.Length; k++)
                {
                    cash_curr += (deltas_prec[k] - deltas_curr[k]) * spots[k];
                }

                deltas_prec = deltas_curr;
                cash_prec = cash_curr;
                date_prec = datafeed.Date;

                var outputs = new OutputData { Date = datafeed.Date, Value = val_curr, Price = price };
                Results.Add(outputs);
            }
            return Results;
        }
    }
}
