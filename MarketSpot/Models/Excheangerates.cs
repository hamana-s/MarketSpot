using MarketSpot.Models.Interface;
using RestSharp;
using Utf8Json;

namespace MarketSpot.Models
{
    /// <summary>為替レートクラス</summary>
    public class Excheangerates : IExchengerates
    {
        /// <summary>米ドル/円レートを取得する</summary>
        /// <returns>USDJPY為替レート</returns>
        public double GetJPYRate()
        {
            var url = "https://api.exchangeratesapi.io/latest?symbols=USD,JPY&base=USD";
            var client = new RestClient(url);
            var req = new RestRequest(Method.GET);
            var res = client.Execute(req);
            if (!res.IsSuccessful) return 0;

            var rate = JsonSerializer.Deserialize<Root>(res.Content);
            return rate.rates.JPY;
        }

        /// <summary>レート</summary>
        public class Rates
        {
            /// <summary>日本円</summary>
            public double JPY { get; set; }

            /// <summary>米ドル</summary>
            public double USD { get; set; }
        }

        /// <summary>JsonRoot</summary>
        public class Root
        {
            /// <summary>レート</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "Jsonデータ構造のため")]
            public Rates rates { get; set; }

            /// <summary>ベース</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "Jsonデータ構造のため")]
            public string @base { get; set; }

            /// <summary>日付</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "Jsonデータ構造のため")]
            public string date { get; set; }
        }
    }
}