using Binance.Net;
using Binance.Net.Interfaces;
using MarketSpot.Models;
using MarketSpot.Models.DB;
using MarketSpot.Models.Interface;
using MarketSpot.Models.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.IO;

[assembly: FunctionsStartup(typeof(MarketSpot.Startup))]

namespace MarketSpot
{
    /// <summary>スタートアップ</summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>DI設定</summary>
        /// <param name="builder">ホストビルダ</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var context = builder.GetContext();
            var configbuilder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: true);
            var configuration = configbuilder.Build();
            var settings = new Settings();
            configuration.GetSection("Config").Bind(settings);
            builder.Services.AddSingleton(settings);
            InitLog(builder);
            builder.Services.AddDbContext<SQLite>();
            builder.Services.BuildServiceProvider().GetRequiredService<SQLite>();
            builder.Services.AddScoped<IBinanceClient, BinanceClient>(x => new BinanceClient());
            builder.Services.AddScoped<IBinanceManipulator, BinanceManipulator>(x =>  new BinanceManipulator(x.GetService<IBinanceClient>(), x.GetService<SQLite>()));
            builder.Services.AddScoped<BinanceService>();
            builder.Services.AddScoped<ICSVManipulator, CSVManipulator>();
            builder.Services.AddScoped<IDiscordManipulator, DiscordManipulator>(x => new DiscordManipulator(settings.DiscordBotToken, settings.DiscordServerID, settings.DiscordChannelID));
            builder.Services.AddScoped<IExchengerates, Excheangerates>();
        }

        /// <summary>ログ初期化</summary>
        /// <param name="builder">ホストビルダ</param>
        private static void InitLog(IFunctionsHostBuilder builder)
        {
            var context = builder.GetContext();
            var rootPath = context.ApplicationRootPath;
            var logger = LogManager.Setup()
               .SetupExtensions(e => e.AutoLoadAssemblies(false))
               .LoadConfigurationFromFile(Path.Combine(rootPath, "NLog.config"), optional: false)
               .LoadConfiguration(builder => builder.LogFactory.AutoShutdown = false)
               .GetCurrentClassLogger();
            LogManager.Configuration.Variables["basedir"] = rootPath;

            builder.Services.AddLogging((loggingBuilder) =>
            {
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                loggingBuilder.AddNLog(new NLogProviderOptions() { ShutdownOnDispose = true });
            });
        }
    }
}