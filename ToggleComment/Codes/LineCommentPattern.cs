using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToggleComment.Codes
{
    /// <summary>
    /// 行コメントのパターンを表すクラスです。
    /// </summary>
    public class LineCommentPattern : ICodeCommentPattern
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
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="prefix">コメントの接頭辞として付ける文字列</param>

        public LineCommentPattern(string prefix)
        {
            Prefix = prefix;
            _regexPattern = new Regex(@"^\s*" + Regex.Escape(prefix));
        }

        /// <inheritdoc />
        public bool IsComment(string text)
        {
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => string.IsNullOrWhiteSpace(x) == false)
                .ToArray();

            return 0 < lines.Length ? lines.All(_regexPattern.IsMatch) : false;
        }
    }
}
