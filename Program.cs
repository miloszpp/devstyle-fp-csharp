using System;
using System.Net.Http;
using Newtonsoft.Json;

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

            var max = getChartItems("AAPL");
            Console.WriteLine(max.Close);
        }
    }

    class ChartItem {
        public DateTime Date { get; set; }
        public double Close { get; set; }
    }
}
