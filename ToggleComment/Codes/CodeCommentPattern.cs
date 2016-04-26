using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToggleComment.Codes
{
    /// <summary>
    /// コードのコメントを表すパターンです。
    /// </summary>
    public class CodeCommentPattern
    {
        /// <summary>
        /// コメントかどうかを判定する正規表現のパターンです。
        /// </summary>
        private readonly Regex _regexPattern;

        /// <summary>
        /// ブロックコメントかどうかを取得します。
        /// </summary>
        public bool IsBlockComment { get; }

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
        public CodeCommentPattern(string prefix, string suffix = "")
        {
            Prefix = prefix;

            if (string.IsNullOrEmpty(suffix))
            {
                IsBlockComment = false;
                Suffix = string.Empty;
                _regexPattern = new Regex(@"^\s*" + Regex.Escape(prefix));
            }
            else
            {
                IsBlockComment = true;
                Suffix = suffix;
                _regexPattern = new Regex(@"^\s*" + Regex.Escape(Prefix) + ".*" + Regex.Escape(Suffix) + @"\s*$");
            }
        }

        /// <summary>
        /// 指定のテキストがコメントかどうかを判定します。
        /// </summary>
        /// <param name="text">判定対象のテキスト</param>
        /// <returns>コメントの場合は<see langword = "true" /></returns>
        public bool IsComment(string text)
        {
            if (IsBlockComment)
            {
                return _regexPattern.IsMatch(text.Replace(Environment.NewLine, string.Empty));
            }
            else
            {
                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => string.IsNullOrWhiteSpace(x) == false)
                    .ToArray();

                return 0 < lines.Length ? lines.All(_regexPattern.IsMatch) : false;
            }
        }
    }
}
