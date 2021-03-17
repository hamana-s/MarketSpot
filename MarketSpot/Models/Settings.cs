using System;
using System.IO;
using Utf8Json;

namespace MarketSpot.Models
{
    /// <summary>設定値クラス</summary>
    public class Settings
    {
        /// <summary>SQLite接続文字列</summary>
        public string SQLiteConnectionString { get; set; } = "";
        /// <summary>csvパス</summary>
        public string CsvPath { get; set; } = "";

        /// <summary>discordボットトークン</summary>
        public string DiscordBotToken { get; set; } = "";
        /// <summary>discordサーバーID</summary>
        public ulong DiscordServerID { get; set; }
        /// <summarydiscordチャンネルID</summary>
        public ulong DiscordChannelID { get; set; }

        /// <summary>設定ファイル保存</summary>
        /// <param name="path">保存先</param>
        public void SaveJson(string path)
        {
            try
            {
                var j = JsonSerializer.ToJsonString(this);
                using var strm = new StreamWriter(path);
                strm.Write(j);
                strm.Flush();
            }
            catch (Exception)
            {
            }
        }
    }
}
