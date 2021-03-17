using MarketSpot.Models.ValueObject;
using System;
using System.Linq;

namespace MarketSpot.Models.DB
{
    public partial class SQLite
    {
        /// <summary>24h分のデータを取得する</summary>
        /// <param name="symbol">シンボル</param>
        /// <returns>24h分</returns>
        public KLine[] GetNewest24h(string symbol)
        {
            try
            {
                return GetDbContext().KLine.AsQueryable().Where(x => x.Symbol == symbol).OrderByDescending(x => x.OpenTime).Take(4 * 24).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        /// <summary>最新のデータを取得する</summary>
        /// <param name="symbol">シンボル</param>
        /// <returns>最新</returns>
        public KLine GetNewest(string symbol)
        {
            try
            {
                return GetDbContext().KLine.AsQueryable().Where(x => x.Symbol == symbol).OrderByDescending(x => x.OpenTime).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        /// <summary>終値最低のデータを取得する</summary>
        /// <param name="symbol">シンボル</param>
        /// <returns>最低</returns>
        public KLine GetLowest(string symbol)
        {
            try
            {
                return GetDbContext().KLine.AsQueryable().Where(x => x.Symbol == symbol).OrderBy(x => x.Close).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        /// <summary>終値最高のデータを取得する</summary>
        /// <param name="symbol">シンボル</param>
        /// <returns>最高</returns>
        public KLine GetHighest(string symbol)
        {
            try
            {
                return GetDbContext().KLine.AsQueryable().Where(x => x.Symbol == symbol).OrderByDescending(x => x.Close).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        /// <summary>DB最新データの日付を取得する</summary>
        /// <param name="symbol">シンボル</param>
        /// <returns>最新の日付</returns>
        public DateTime GetNewestDate(string symbol)
        {
            try
            {
                var q = GetDbContext().KLine.AsQueryable().Where(x => x.Symbol == symbol).OrderByDescending(x => x.OpenTime).FirstOrDefault();
                return DateTime.TryParse(q?.OpenTime, out var dt) ? dt : new DateTime(2021, 1, 1);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        /// <summary>ローソク足を挿入する</summary>
        /// <param name="insert">挿入するデータ</param>
        public void InsertKLine(KLine[] insert)
        {
            try
            {
                var dbc = GetDbContext();
                dbc.KLine.AddRange(insert);
                dbc.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error($"ローソク足挿入失敗 {ex}");
            }
        }

        /// <summary>レポートを取得する</summary>
        /// <param name="symbollist">対象シンボル</param>
        /// <param name="yenrate">日本円米ドルレート</param>
        /// <returns>レポートサマリの配列</returns>
        public Summary[] GetReport(string[] symbollist, double yenrate)
        {
            return symbollist.Select(symbol => GetSummary(symbol, yenrate)).ToArray();
        }

        /// <summary>サマリを取得する</summary>
        /// <param name="symbol">対象シンボル</param>
        /// <param name="yrate">日本円米ドルレート</param>
        /// <returns>レポートサマリ</returns>
        public Summary GetSummary(string symbol, double yrate)
        {
            var newest = GetNewest(symbol);
            return new Summary()
            {
                Symbol = symbol,
                Newest = newest,
                Lowest = GetLowest(symbol),
                Highest = GetHighest(symbol),
                Day = GetNewest24h(symbol),
                CloseJPY = newest.Close.ToFloat() * yrate,
            };
        }
    }
}