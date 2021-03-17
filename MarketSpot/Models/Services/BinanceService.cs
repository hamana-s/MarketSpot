using MarketSpot.Models.DB;
using MarketSpot.Models.Interface;
using NLog;
using System.Diagnostics;
using System.Linq;

namespace MarketSpot.Models.Services
{
    public class BinanceService
    {
        /// <summary>バイナンス</summary>
        private readonly IBinanceManipulator Binance;

        /// <summary>DBコンテキスト</summary>
        private readonly SQLite DBC;

        /// <summary>ディスコード</summary>
        private readonly IDiscordManipulator Discord;

        /// <summary>ロガー</summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>コンストラクタ</summary>
        /// <param name="config">設定(DI)</param>
        /// <param name="binance">バイナンス(DI)</param>
        /// <param name="dbc">DBコンテキスト(DI)</param>
        public BinanceService(IBinanceManipulator binance, SQLite dbc, IDiscordManipulator discord)
        {
            Binance = binance;
            DBC = dbc;
            Discord = discord;
        }

        /// <summary>サマリーレポートをDiscordに投稿する</summary>
        /// <param name="symbollist">シンボルリスト</param>
        public void Report(string[] symbollist, double yenrate)
        {
            Log.Info("Report実行");

            Log.Info("GetMarket開始");
            var sw = Stopwatch.StartNew();
            Binance.GetMarket(symbollist);
            sw.Stop();
            Log.Info($"GetMarket終了 所要時間: {sw.ElapsedMilliseconds}ms");

            var repo = DBC.GetReport(symbollist, yenrate)
                .OrderByDescending(x => x._24h)
                .ThenByDescending(x => x._1h)
                .ThenByDescending(x => x.Position);

            Log.Info("Post開始");
            Discord.PostTable(repo.Where(x => x.Position > 0.8 || x._24h > 0.10 || x._1h > 0.05 || x.Near > 0.03));
            Discord.PostTable(repo.Where(x => x.Position < 0.3 || x._24h < -0.15 || x._1h < -0.10 || x.Near < -0.04));
            Log.Info("Post終了");
        }
    }
}