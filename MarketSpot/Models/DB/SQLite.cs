using MarketSpot.Models.ValueObject;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace MarketSpot.Models.DB
{
    /// <summary>DBクラス</summary>
    public partial class SQLite : DbContext
    {
        /// <summary>接続文字列</summary>
        private static string ConnectionString;

        /// <summary>ロガー</summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>コンストラクタ</summary>
        public SQLite()
        {
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="configuration">設定</param>
        public SQLite(Settings configuration)
        {
            ConnectionString = configuration.SQLiteConnectionString;
        }

        /// <summary>設定値の変更</summary>
        /// <param name="optionsBuilder">オプション構成オブジェクト</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString, providerOptions => providerOptions.CommandTimeout(60));
        }

        /// <summary>ローソク足</summary>
        public DbSet<KLine> KLine { get; set; }

        /// <summary>SQLiteインスタンス生成</summary>
        /// <returns>DBコンテキスト</returns>
        public SQLite GetDbContext()
        {
            return new SQLite();
        }
    }
}