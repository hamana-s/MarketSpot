namespace MarketSpot.Models.ValueObject
{

    /// <summary>ローソク足クラス</summary>
    public class KLine
    {
        /// <summary>ID</summary>
        public long ID { get; set; }

        /// <summary>シンボル</summary>
        public string Symbol { get; set; }

        /// <summary>Gets or sets the base volume.</summary>
        public string BaseVolume { get; set; }

        /// <summary>終値</summary>
        public string Close { get; set; }

        /// <summary>終了時刻</summary>
        public string CloseTime { get; set; }

        /// <summary>高値</summary>
        public string High { get; set; }

        /// <summary>Gets or sets the ignore.</summary>
        public long Ignore { get; set; }

        /// <summary>安値</summary>
        public string Low { get; set; }

        /// <summary>始値</summary>
        public string Open { get; set; }

        /// <summary>開始時刻</summary>
        public string OpenTime { get; set; }

        /// <summary>売買量</summary>
        public string QuoteVolume { get; set; }

        /// <summary>売量</summary>
        public string TakerBuyBaseVolume { get; set; }

        /// <summary>買量</summary>
        public string TakerBuyQuoteVolume { get; set; }

        /// <summary>取引数</summary>
        public long TradeCount { get; set; }
    }
}