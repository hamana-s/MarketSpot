using MarketSpot.Models;
using MarketSpot.Models.Interface;
using MarketSpot.Models.Services;
using Microsoft.Azure.WebJobs;

namespace MarketSpot
{
    /// <summary>マーケットファンクション</summary>
    public class MarketFunction
    {
        /// <summary>設定</summary>
        private readonly Settings Config;

        /// <summary>バイナンスサービス</summary>
        private readonly BinanceService Binance;

        /// <summary>CSV</summary>
        private readonly ICSVManipulator CSV;

        /// <summary>為替レート</summary>
        private readonly IExchengerates Excheangerates;

        /// <summary>コンストラクタ</summary>
        /// <param name="config">設定(DI)</param>
        /// <param name="binance">バイナンスサービス(DI)</param>
        /// <param name="csv">CSV(DI)</param>
        public MarketFunction(Settings config, BinanceService binance, ICSVManipulator csv, IExchengerates exchengerates)
        {
            Config = config;
            Binance = binance;
            CSV = csv;
            Excheangerates = exchengerates;
        }

        /// <summary>サマリレポート投稿</summary>
        /// <param name="myTimer">myTimer</param>
        [FunctionName("ReportBinance")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:未使用のパラメーターを削除します", Justification = "タイマー設定のため")]
        public void Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer)
        {
            Binance.Report(CSV.ReadSymbolList(Config.CsvPath), Excheangerates.GetJPYRate());
        }
    }
}