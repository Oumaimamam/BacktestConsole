using BacktestConsole.ParsingTools;
using System;
using System.Collections.Generic;
using PricingLibrary.Computations;
using PricingLibrary.DataClasses;
using PricingLibrary.MarketDataFeed;
using PricingLibrary.RebalancingOracleDescriptions;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using CsvHelper;

namespace BacktestConsole
{
    class  Program
    {
        static void Main(string[] args)
        {
            //var BasketTestParameters = LoadTestParameters(@"C:\Users\localuser\Downloads\systematic-strategies-net-main\systematic-strategies-net-main\Resources\TestData\Test_5_2\params_5_1.json");
            //var lstDF = ConstructDataFeed(@"C:\Users\localuser\Downloads\systematic-strategies-net-main\systematic-strategies-net-main\Resources\TestData\Test_5_1\data_5_1.csv");
            var BasketTestParameters = LoadTestParameters(@"C:\Users\oumai\Downloads\TestData\Test_1_1\params_1_1.json");
            var lstDF = ConstructDataFeed(@"C:\Users\oumai\Downloads\TestData\Test_1_1\data_1_1.csv");

            List<OutputData> results = BackTest(BasketTestParameters, lstDF);

            ////affichage des résultats
            foreach (OutputData result in results)
            {
                Console.WriteLine($"{result.Date}, {result.Value}, {result.Price}"); 
            }

            // Sauvegarde des résultats dans un fichier JSON
            SaveResultsToFile(results, @"C:\Users\oumai\Downloads\output_file.json");
        }

        static void SaveResultsToFile(List<OutputData> results, string filePath)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true, // Cette option permet d'indenter le JSON
                Converters = { new JsonStringEnumConverter(), new RebalancingOracleDescriptionConverter() }
            };
            var jsonString = JsonSerializer.Serialize(results, options);
            File.WriteAllText(filePath, jsonString);
        }

        static BasketTestParameters LoadTestParameters(string JsonPath)
        {
            var json = File.ReadAllText(JsonPath);
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(), new RebalancingOracleDescriptionConverter() }
            };
            return JsonSerializer.Deserialize<BasketTestParameters>(json, options);
        }

        static List<DataFeed> ConstructDataFeed(string FileCsv)
        {
            //Csv parameters
            IEnumerable<ShareValue> shareEnum;
            using (var reader = new StreamReader(FileCsv))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                shareEnum = csv.GetRecords<ShareValue>().ToList();
            }
            var dataFeeds = shareEnum.GroupBy(d => d.DateOfPrice,
                         t => new { Symb = t.Id.Trim(), Val = t.Value },
                         (key, g) => new DataFeed(key, g.ToDictionary(e => e.Symb, e => e.Val))).ToList();
            return dataFeeds;
        }

        static List<OutputData> BackTest ( BasketTestParameters Testparameters, List<DataFeed> lstDF)
        {
            //Composition portefeuille en t0
            List<OutputData> results = new List<OutputData>();
            OutputData output = new OutputData();
            Dictionary<string, double> spotsDict = lstDF[0].PriceList;
            double[] spots = spotsDict.Values.ToArray();
            Pricer pricer = new Pricer(Testparameters);
            DateTime date_0 = lstDF[0].Date;
            PricingResults result = pricer.Price(date_0, spots);
            double p0 = result.Price;

            double[] deltas = result.Deltas;
            double cash = p0;

            for (int i = 0; i < spots.Length; i++)
            {
                cash -= deltas[i] * spots[i];
            }
            output.Date = date_0;
            output.Value = p0;
            output.Price = p0;
            results.Add(output);

            var deltas_prec = deltas;
            double val_curr = 0;
            double cash_prec = cash;
            DateTime date_prec = date_0;

            for (int i = 1; i < lstDF.Count; i++)
            {
                DataFeed datafeed = lstDF[i];
                spots = datafeed.PriceList.Values.ToArray();

                result = pricer.Price(datafeed.Date, spots);
                double price = result.Price;
                var deltas_curr = result.Deltas;

                for (int j = 0; j < spots.Length; j++)
                {
                    val_curr = deltas_prec[j] * spots[j];
                }
                double capitalisation = RiskFreeRateProvider.GetRiskFreeRateAccruedValue(date_prec, datafeed.Date);
                val_curr += cash_prec * capitalisation;
                var cash_curr = val_curr;

                for (int k = 0; k < spots.Length; k++)
                {
                    cash_curr -=  deltas_curr[k] * spots[k];
                }
                deltas_prec = deltas_curr;
                cash_prec = cash_curr;
                date_prec = datafeed.Date;

                var outputs = new OutputData();
                outputs.Date = datafeed.Date;
                outputs.Value = val_curr;
                outputs.Price = price;
                results.Add(outputs);

            }
            return results;
        }
    }
}
