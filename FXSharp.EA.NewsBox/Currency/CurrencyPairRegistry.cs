﻿
using FXSharp.TradingPlatform.Exts;
using System.Collections.Generic;
using System.Linq;
namespace FXSharp.EA.NewsBox
{
    public class CurrencyPairRegistry
    {
        private static Dictionary<string, string> currencyToPair = new Dictionary<string, string> 
        {
            {"NZD", "NZDUSD"}, 
            {"AUD", "AUDUSD"}, 
            {"CNY", "AUDUSD"}, 
            {"JPY", "USDJPY"}, 
            {"CHF", "USDCHF"}, 
            {"EUR", "EURUSD"}, 
            {"GBP", "GBPUSD"}, 
            {"CAD", "USDCAD"}, 
            {"USD", "EURUSD"}
        };

        private static IList<string> majorPairs = new List<string> { "AUDUSD", "NZDUSD", "USDJPY", "USDCHF", "EURUSD", "GBPUSD", "USDCAD" };

        private static IList<string> currenciesPriorities = new List<string>
                                                                {
                                                                    "EUR", "GBP", "AUD", "NZD", "USD", "CAD", "CHF", "JPY"
                                                                };

        private static IList<string> currencyPairs = new List<string>();

        static CurrencyPairRegistry()
        {
            for (int i = 0; i < currenciesPriorities.Count - 1; i++)
            {
                for (int j = i+1; j < currenciesPriorities.Count; j++)
                {
                    currencyPairs.Add(string.Format("{0}{1}", currenciesPriorities[i], currenciesPriorities[j]));
                }
            }

            RemoveUnregisteredCurrencies();
        }

        public static void FilterCurrencyForMinimalSpread(EExpertAdvisor ea)
        {
            currencyPairs = GetLowSpreadsCurrencies(ea).ToList();
        }

        private static IEnumerable<string> GetLowSpreadsCurrencies(EExpertAdvisor ea)
        {
            var spreadInfos = new List<SpreadInfo>();

            foreach (var pair in CurrencyPairs)
            {
                var spread = ea.MarketInfo(pair, TradePlatform.MT4.SDK.API.MARKER_INFO_MODE.MODE_SPREAD);
                spreadInfos.Add(new SpreadInfo { Symbol = pair, Spread = spread });
            }

            var results = from s in spreadInfos
                          where s.Spread <= 20
                          orderby s.Spread ascending
                          select s.Symbol;

            return results;
        }

        private static void RemoveUnregisteredCurrencies()
        {
            currencyPairs.Remove("NZDCAD");
            currencyPairs.Remove("NZDCHF");
        }

        public static IList<string> CurrencyPairs
        {
            get { return currencyPairs; }
        }

        public string RelatedCurrencyPair(string currency)
        {
            return currencyToPair[currency];
        }

        public static IEnumerable<string> RelatedCurrencyPairs(string currency)
        {
            if (currency == "CNY")
                currency = "AUD";

            var result = from s in currencyPairs
                         where s.Contains(currency)
                         select s;

            return result;
        }

        public static IEnumerable<string> RelatedCurrencyPairsForMinimalSpread(EExpertAdvisor ea, string currency)
        {
            var spreadInfos = new List<SpreadInfo>();

            foreach (var pair in RelatedCurrencyPairs(currency))
            {
                var spread = ea.MarketInfo(pair, TradePlatform.MT4.SDK.API.MARKER_INFO_MODE.MODE_SPREAD);
                spreadInfos.Add(new SpreadInfo { Symbol = pair, Spread = spread });
            }

            var results = from s in spreadInfos
                          //where s.Spread <= 20
                          orderby s.Spread ascending
                          select s.Symbol;

            return results;
        }

        public static IEnumerable<string> RelatedMajorCurrencyPairs(string currency)
        {
            if (currency == "CNY")
                currency = "AUD";

            var result = from s in majorPairs
                         where s.Contains(currency)
                         select s;

            return result;
        }

        public static IEnumerable<string> RelatedMajorCurrencyPairsForMinimalSpread(EExpertAdvisor ea, string currency)
        {
            var spreadInfos = new List<SpreadInfo>();

            foreach (var pair in RelatedMajorCurrencyPairs(currency))
            {
                var spread = ea.MarketInfo(pair, TradePlatform.MT4.SDK.API.MARKER_INFO_MODE.MODE_SPREAD);
                spreadInfos.Add(new SpreadInfo { Symbol = pair, Spread = spread });
            }

            var results = from s in spreadInfos
                          orderby s.Spread ascending
                          select s.Symbol;

            return results;
        }
    }
}
