using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketSpot.Models.ValueObject
{
    /// <summary>サマリクラス</summary>
    public class Summary
    {
        /// <summary>シンボル</summary>
        public string Symbol { get; set; }

        /// <summary>最新</summary>
        public KLine Newest { get; set; }
        /// <summary>最低</summary>
        public KLine Lowest { get; set; }
        /// <summary>最高</summary>
        public KLine Highest { get; set; }
        /// <summary>直近24時間</summary>
        public KLine[] Day { get; set; }

        /// <summary>直近24時間の最低</summary>
        public KLine DayLowest { get { return Day.OrderBy(x => x.Close).First(); } }
        /// <summary>直近24時間の最高</summary>
        public KLine DayHighest { get { return Day.OrderByDescending(x => x.Close).First(); } }

        /// <summary>終値</summary>
        public double Close { get { return Newest.Close.ToFloat(); } }
        /// <summary>終値(JPY)</summary>
        public double CloseJPY { get; set; }
        /// <summary>最高-最低間の位置</summary>
        public double Position { get { return GetPosition(Newest.Close.ToFloat(), Highest.Close.ToFloat(), Lowest.Close.ToFloat()); } }
        /// <summary>24時間の変動率</summary>
        public double _24h { get { return 1 - DayLowest.Close.ToFloat() / DayHighest.Close.ToFloat(); } }
        /// <summary>1時間の変動率</summary>
        public double _1h { get { return 1 - Day.Take(4).Min(x => x.Close).ToFloat() / Day.Take(4).Max(x => x.Close).ToFloat(); } }
        /// <summary>直近の変動率</summary>
        public double Near { get { return 1 - Day.Take(2).Last().Close.ToFloat() / Day.Take(2).First().Close.ToFloat(); } }

        /// <summary>文字列化</summary>
        /// <returns>現在のオブジェクトを表す文字列。</returns>
        public override string ToString()
        {
            var ret = new StringBuilder();
            ret.Append($"{Symbol} : {Close:g7} / {CloseJPY:c3}\t");
            ret.Append($"pos: {Position:p1}\t");
            ret.Append($"24h: {_24h:p1}\t");
            ret.Append($"1h: {_1h:p1}\t");
            ret.Append($"near: {Near:p1}\t");
            return ret.ToString();
        }

        /// <summary>KLineを示すテーブル</summary>
        /// <returns>テーブル</returns>
        public List<object> ToTable()
        {
            return new List<object>
            {
                $"{Symbol}",
                $"{Close:g7}",
                //$"{CloseJPY:c3}",
                $"{Position:p1}",
                $"{_24h:p1}",
                $"{_1h:p1}",
                $"{Near:p1}"
            };
        }

        /// <summary>テーブルヘッダを取得する</summary>
        /// <returns>ヘッダ</returns>
        public static List<object> GetHeader()
        {
            return new List<object>
            {
                "symbol",
                "USDT",
                //"JPY",
                "pos",
                "24h",
                "1h",
                "near"
            };
        }

        /// <summary>最高-最低間の位置を取得</summary>
        /// <param name="current">現在値</param>
        /// <param name="high">最高</param>
        /// <param name="low">最低</param>
        /// <returns>パーセント</returns>
        private double GetPosition(double current, double high, double low)
        {
            var a = Math.Abs(high - low);
            var b = Math.Abs(current - low);
            return b / a;
        }
    }
}