namespace MarketSpot.Models
{
    /// <summary>CSVシンボルリストクラス</summary>
    /// <remarks>GetAllBookPricesで取得する</remarks>
    public class SymbolData
    {
        /// <summary>価格</summary>
        public double Price { get; set; }
        /// <summary>シンボル</summary>
        public string Symbol { get; set; }
        /// <summary>取得対象</summary>
        public string Flag { get; set; }
    }
}