using Binance.Net.Enums;
using Binance.Net.Interfaces;
using MarketSpot.Models.DB;
using MarketSpot.Models.Interface;
using MarketSpot.Models.ValueObject;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketSpot.Models
{
    /// <summary>バイナンス操作クラス</summary>
    public class BinanceManipulator : IBinanceManipulator
    {
        /// <summary>バイナンスクライアント</summary>
        private readonly IBinanceClient Client;

        /// <summary>DBコンテキスト</summary>
        private readonly SQLite DBC;

        /// <summary>ロガー</summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>コンストラクタ</summary>
        /// <param name="client">バイナンスクライアント(DI)</param>
        /// <param name="dbc">DBコンテキスト(DI)</param>
        public BinanceManipulator(IBinanceClient client, SQLite dbc)
        {
            Client = client;
            DBC = dbc;
        }

        /// <summary>マーケット情報を取得する</summary>
        /// <param name="symbollist">対象シンボル</param>
        public void GetMarket(string[] symbollist)
        {
            try
            {
                foreach (var symbol in symbollist)
                {
                    var dt = DBC.GetNewestDate(symbol).AddMinutes(15);
                    var list = new List<KLine>();
                    while (true)
                    {
                        var k = GetKLine(symbol, dt, 24);
                        list.AddRange(k);
                        dt = dt.AddDays(1);
                        if (dt > DateTime.Now) break;
                    }
                    DBC.InsertKLine(list.ToArray());
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        /// <summary>全シンボルを取得する</summary>
        public IBinanceBookPrice[] GetALLPrice()
        {
            try
            {
                var p = Client.Spot.Market.GetAllBookPrices().Data;
                return p.Where(x => x.Symbol.IsMatch(@"USDT$") && !x.Symbol.Contains("UP") && !x.Symbol.Contains("DOWN")).OrderBy(x => x.BestBidPrice).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        /// <summary>ローソク足情報を取得する</summary>
        /// <param name="symbol">対象シンボル</param>
        /// <param name="start">開始時刻</param>
        /// <param name="hour">ローソク足の対象期間</param>
        /// <returns>ローソク足情報</returns>
        public KLine[] GetKLine(string symbol, DateTime start, int hour = 1)
        {
            try
            {
                var startoffset = new DateTimeOffset(start, new TimeSpan(9, 0, 0));
                var onehour = Client.Spot.Market.GetKlines(symbol, KlineInterval.FifteenMinutes, startoffset.DateTime, startoffset.DateTime.AddHours(hour).AddMinutes(-1)).Data;
                return onehour.Select(x => new KLine()
                {
                    Symbol = symbol,
                    BaseVolume = $"{x.BaseVolume}",
                    Close = $"{x.Close}",
                    CloseTime = $"{x.CloseTime:yyyy-MM-dd HH:mm:ss}",
                    High = $"{x.High}",
                    Low = $"{x.Low}",
                    Open = $"{x.Open}",
                    OpenTime = $"{x.OpenTime:yyyy-MM-dd HH:mm:ss}",
                    QuoteVolume = $"{x.QuoteVolume}",
                    TakerBuyBaseVolume = $"{x.TakerBuyBaseVolume}",
                    TakerBuyQuoteVolume = $"{x.TakerBuyQuoteVolume}",
                    TradeCount = x.TradeCount
                }).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }
    }
}