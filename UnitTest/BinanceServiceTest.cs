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

            $"{string.Join(",", symbol)}�̏����擾���邱��"
            .x(() =>
            {
                binance.Verify(d => d.GetMarket(symbol), Times.Once);
            });

            "Discord�փ|�X�g���邱��"
            .x(() =>
            {
                discord.Verify(d => d.PostTable(It.IsAny<IEnumerable<Summary>>()), Times.Exactly(2));
            });

            "�|�X�g����f�[�^�̃`�F�b�N"
            .x(() =>
            {
                assert.Should().HaveCount(2);
                assert[0].Should().HaveCount(1);
                assert[1].Should().HaveCount(0);
            });
        }
    }
}