using FluentAssertions;
using MarketSpot.Models;
using MarketSpot.Models.DB;
using MarketSpot.Models.Interface;
using MarketSpot.Models.Services;
using MarketSpot.Models.ValueObject;
using Moq;
using System.Collections.Generic;
using Xbehave;

namespace UnitTest
{
    public class BinanceServiceTest
    {
        [Scenario]
        public void ReportTest()
        {
            var binance = new Mock<IBinanceManipulator>();
            var discord = new Mock<IDiscordManipulator>();
            var assert = new List<IEnumerable<Summary>>();
            discord.Setup(x => x.PostTable(It.IsAny<IEnumerable<Summary>>())).Callback<IEnumerable<Summary>>(x => assert.Add(x));
            var target = new BinanceService(binance.Object, new SQLite(new Settings() { SQLiteConnectionString = "Data Source=..\\..\\..\\TestData\\Report.sqlite" }), discord.Object);
            var symbol = new[] { "BTCUSDT" };
            target.Report(symbol, 100);

            $"{string.Join(",", symbol)}の情報を取得すること"
            .x(() =>
            {
                binance.Verify(d => d.GetMarket(symbol), Times.Once);
            });

            "Discordへポストすること"
            .x(() =>
            {
                discord.Verify(d => d.PostTable(It.IsAny<IEnumerable<Summary>>()), Times.Exactly(2));
            });

            "ポストするデータのチェック"
            .x(() =>
            {
                assert.Should().HaveCount(2);
                assert[0].Should().HaveCount(1);
                assert[1].Should().HaveCount(0);
            });
        }
    }
}