using System;
using System.Text.RegularExpressions;

namespace ToggleComment.Codes
{
    /// <summary>
    /// ブロックコメントのパターンを表すクラスです。
    /// </summary>
    public class BlockCommentPattern : ICodeCommentPattern
    {
        /// <summary>
        /// コメントかどうかを判定する正規表現のパターンです。
        /// </summary>
        private readonly Regex _regexPattern;

        /// <summary>
        /// コメントの接頭辞として付ける文字列を取得します。
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// コメントの接尾辞として付ける文字列を取得します。
        /// </summary>
        public string Suffix { get; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="prefix">コメントの接頭辞として付ける文字列</param>
        /// <param name="suffix">コメントの接尾辞として付ける文字列</param>
        public BlockCommentPattern(string prefix, string suffix)
        {
            Prefix = prefix;
            Suffix = suffix;
            _regexPattern = new Regex(@"^\s*" + Regex.Escape(Prefix) + ".*" + Regex.Escape(Suffix) + @"\s*$");
        }

        /// <inheritdoc />
        public bool IsComment(string text)
        {
            return _regexPattern.IsMatch(text.Replace(Environment.NewLine, string.Empty));
        }
    }
}
