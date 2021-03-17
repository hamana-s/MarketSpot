using MarketSpot.Models.ValueObject;
using System.Collections.Generic;

namespace MarketSpot.Models.Interface
{
    public interface IDiscordManipulator
    {
        void Post(string message, ulong serverid = 0, ulong channelid = 0);

        void PostTable(IEnumerable<Summary> tabledata);
    }
}