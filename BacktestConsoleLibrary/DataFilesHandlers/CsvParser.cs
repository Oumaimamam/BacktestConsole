﻿using CsvHelper;
using PricingLibrary.MarketDataFeed;
using System.Globalization;

namespace BacktestConsoleLibrary.DataFilesHandlers
{
    public class CsvParser
    {
        public List<DataFeed> ConstructDataFeedFromCsv(string FileCsv)
        {
            /*Csv parameters*/
            IEnumerable<ShareValue> ShareEnum;
            using (var Reader = new StreamReader(FileCsv))
            using (var CsvFileContent = new CsvReader(Reader, CultureInfo.InvariantCulture))
            {
                ShareEnum = CsvFileContent.GetRecords<ShareValue>().ToList();
            }
            /*Création de DataFeed pour chaque date*/
            var DataFeeds = ShareEnum.GroupBy(d => d.DateOfPrice,
                         t => new { Symb = t.Id.Trim(), Val = t.Value },
                         (key, g) => new DataFeed(key, g.ToDictionary(e => e.Symb, e => e.Val))).OrderBy(df => df.Date).ToList();
            return DataFeeds;
        }
    }
}

