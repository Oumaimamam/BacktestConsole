using PricingLibrary.DataClasses;

namespace BacktestConsole.Utils
{
    public class DictDeltaMaker
    {
        public  Dictionary<string, double> Make(double[] Deltas, BasketTestParameters Testparameters)
        {
            string[] SharesId = Testparameters.BasketOption.UnderlyingShareIds;
            Dictionary<string, double> Composition = new Dictionary<string, double>();
            for (int p = 0; p < SharesId.Length; p++)
            {
                Composition[SharesId[p]] = Deltas[p];
            }
            return Composition;
        }
    }
}
