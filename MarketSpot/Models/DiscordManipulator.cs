using ConsoleTableExt;
using Discord;
using Discord.WebSocket;
using MarketSpot.Models.Interface;
using MarketSpot.Models.ValueObject;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MarketSpot.Models
{
    /// <summary>
    /// discordd操作クラス
    /// </summary>
    public class DiscordManipulator : IDisposable, IDiscordManipulator
    {
        /// <summary>ロガー</summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>クライアント</summary>
        private DiscordSocketClient Client = new DiscordSocketClient();

        /// <summary>サーバーID</summary>
        public ulong ServerID { get; }

        /// <summary>チャンネルID</summary>
        public ulong ChannelID { get; }

        /// <summary>コンストラクタ</summary>
        /// <param name="bottoken">ボットトークン</param>
        /// <param name="serverid">サーバーID</param>
        /// <param name="channelid">チャンネルID</param>
        public DiscordManipulator(string bottoken, ulong serverid, ulong channelid)
        {
            ServerID = serverid;
            ChannelID = channelid;
            Start(bottoken);
        }

        /// <summary>クライアント起動</summary>
        /// <param name="bottoken">ボットトークン</param>
        private void Start(string bottoken)
        {
            Client = new DiscordSocketClient();
            Client.LoginAsync(TokenType.Bot, bottoken);
            Client.StartAsync();
            do
            {
                Thread.Sleep(100);
            } while (Client.ConnectionState != ConnectionState.Connected);
        }

        /// <summary>アンマネージ リソースの解放またはリセットに関連付けられているアプリケーション定義のタスクを実行します。</summary>
        public void Dispose()
        {
            Client.LogoutAsync();
            Client.Dispose();
        }

        /// <summary>メッセージを投稿する</summary>
        /// <param name="message">メッセージ</param>
        /// <param name="serverid">サーバーID</param>
        /// <param name="channelid">チャンネルID</param>
        public void Post(string message, ulong serverid = 0, ulong channelid = 0)
        {
            try
            {
                var sid = serverid == 0 ? ServerID : serverid;
                var cid = channelid == 0 ? ChannelID : channelid;
                if (message.IsEmpty()) return;
                Client.GetGuild(sid).GetTextChannel(cid).SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        /// <summary>テーブルを投稿する</summary>
        /// <param name="tabledata">テーブルデータ</param>
        public void PostTable(IEnumerable<Summary> tabledata)
        {
            if (!tabledata.Any()) return;
            var table = new List<List<object>> { Summary.GetHeader() };
            table.AddRange(tabledata.Select(x => x.ToTable()));
            var s = ConsoleTableBuilder.From(table).WithFormat(ConsoleTableBuilderFormat.Minimal).Export();
            Post($"```\r\n{s}\r\n```");
        }
    }
}