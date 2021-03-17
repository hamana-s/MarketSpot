using System.Text.RegularExpressions;

namespace MarketSpot.Models
{
    /// <summary>文字列拡張クラス</summary>
    public static class StringExtension
    {
        /// <summary>空の文字列チェック</summary>
        /// <param name="str">ターゲット</param>
        /// <returns>
        /// true: 空
        /// false: 空ではない
        /// </returns>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>正規表現検索を行う</summary>
        /// <param name="str">文字列</param>
        /// <param name="pat">正規表現パターン</param>
        /// <param name="m">マッチ結果</param>
        /// <returns>
        /// true: マッチした
        /// false: マッチしなかった
        /// </returns>
        public static bool Match(this string str, string pat, out Match m)
        {
            m = null!;
            if (str.IsEmpty()) return false;
            m = Regex.Match(str, pat);
            return m.Success;
        }

        /// <summary>
        /// 正規表現に一致するかチェック
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="pat">正規表現パターン</param>
        /// <returns>
        /// true: マッチした
        /// false: マッチしなかった
        /// </returns>
        public static bool IsMatch(this string str, string pat)
        {
            if (str.IsEmpty()) return false;
            return Regex.IsMatch(str, pat);
        }

        /// <summary>文字列から整数を返す</summary>
        /// <param name="str">文字列</param>
        /// <param name="def">変換できなかった際に返す値</param>
        /// <returns>文字列の示す整数</returns>
        public static int ToInt(this string str, int def = 0)
        {
            if (str == null) return def;
            return int.TryParse(str.Trim(), System.Globalization.NumberStyles.AllowThousands, null, out var t) ? t : def;
        }

        /// <summary>
        /// 文字列から浮動小数点数を返す
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>文字列の示す浮動小数点数</returns>
        public static double ToFloat(this string str)
        {
            return double.TryParse(str.Replace(",", ""), out var t) ? t : 0;
        }
    }
}