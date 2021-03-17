using CsvHelper;
using MarketSpot.Models.Interface;
using NLog;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MarketSpot.Models
{
    /// <summary>CSV操作クラス</summary>
    public class CSVManipulator : ICSVManipulator
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public CSVManipulator()
        {
        }

        /// <summary>シンボル一覧を読み込む</summary>
        /// <param name="symbolpath">csvファイルパス</param>
        /// <returns>シンボルの配列</returns>
        public string[] ReadSymbolList(string symbolpath)
        {
            try
            {
                using var sr = new StreamReader(symbolpath, Encoding.UTF8);
                using var csv = new CsvReader(sr, CultureInfo.CurrentCulture);
                return csv.GetRecords<SymbolData>().Where(x => !x.Flag.IsEmpty()).Select(x => x.Symbol).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }
    }
}