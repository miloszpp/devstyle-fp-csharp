using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using System.Xml.Linq;

namespace devstylefpcsharp
{
    using static FP;

    class Program
    {
        static void Main(string[] args)
        {
            Func<string, string> normalizeSymbol = symbol => symbol.ToLower();

            var fetchChart = Curry<string, string, string>((period, symbol) =>
            {
                using (var httpClient = new HttpClient())
                {
                    return httpClient.GetStringAsync($"https://api.iextrading.com/1.0/stock/{symbol}/chart/{period}").Result;
                }
            });

            Func<string, ChartItem> getChartItems = Compose(
                normalizeSymbol, 
                fetchChart("1m"),
                JsonConvert.DeserializeObject<ChartItem[]>,
                Reduce<ChartItem>((candidate, item) => candidate.Close > item.Close ? candidate : item)
            );

            //Func<string, ChartItem> getChartItems = Compose(
            //    (Func<string, string>)Normalize, 
            //    Curry<string, string, string>(Fetch)("1m"),
            //    Parse,
            //    Reduce<ChartItem>((candidate, item) => candidate.Close > item.Close ? candidate : item)
            //);

            var max = getChartItems("AAPL");
        }

        static string Normalize(string symbol)
            => symbol.ToLower();

        static string Fetch(string period, string symbol) 
        {
            using (var httpClient = new HttpClient())
            {
                return httpClient.GetStringAsync($"https://api.iextrading.com/1.0/stock/{symbol}/chart/{period}").Result;
            }    
        }

        static ChartItem[] Parse(string json) 
            => JsonConvert.DeserializeObject<ChartItem[]>(json);


    }

    class ChartItem {
        public DateTime Date { get; set; }
        public double Close { get; set; }
    }
}
