using Binance.Net.Interfaces;
using MarketSpot.Models.ValueObject;
using System;

namespace MarketSpot.Models.Interface
{
    public interface IBinanceManipulator
    {
        void GetMarket(string[] symbollist);

        IBinanceBookPrice[] GetALLPrice();

        public KLine[] GetKLine(string symbol, DateTime start, int hour = 1);
    }
}