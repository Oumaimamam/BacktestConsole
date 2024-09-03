using PricingLibrary.MarketDataFeed;
using System;
using System.Collections.Generic;

namespace BacktestConsole.HedgingStrategyBacktest
{
    internal class Portfolio
    {
        private Dictionary<string, double> Composition;
        private double Cash;
        private DateTime PreviousRebalancingDate;

        public Dictionary<string, double> GetComposition()
        {
            return Composition;
        }
        public Portfolio(Dictionary<string, double> Composition, DataFeed DataFeed, double Cash)
        {

            this.Composition = Composition;
            this.Cash = Cash;
            this.PreviousRebalancingDate = DataFeed.Date;
        }

        public double ComputeValue(DataFeed DataFeed)
        {
            Dictionary<string, double> Spots = DataFeed.PriceList;
            double capitalisation = RiskFreeRateProvider.GetRiskFreeRateAccruedValue( PreviousRebalancingDate, DataFeed.Date);
            double Value = Cash * capitalisation;
            Value += DictionaryUtils.ScalarProduct(Composition, Spots);
            return Value;
        }

        public void UpdatePortfolio(Dictionary<string, double> NewComposition, DataFeed DataFeed)
        {
            Cash = ComputeValue(DataFeed) - DictionaryUtils.ScalarProduct(NewComposition, DataFeed.PriceList);
            Composition = NewComposition;
            PreviousRebalancingDate = DataFeed.Date;
        } 
    }
}
