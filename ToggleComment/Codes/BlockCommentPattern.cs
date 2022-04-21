using System;
using System.Text.RegularExpressions;

namespace ToggleComment.Codes
{
    /// <summary>
    /// Class representing the block comment pattern.
    /// </summary>
    public class BlockCommentPattern : ICodeCommentPattern
    {
        /// <summary>
        /// Regular expression pattern to determine if it is a comment.
        /// </summary>
        private readonly Regex _regexPattern;

        /// <summary>
        /// Gets the string to prefix the comment with.
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// Gets the string to attach as a suffix to the comment.
        /// </summary>
        public string Suffix { get; }

        /// <summary>
        /// Instance initialisation.
        /// </summary>
        /// <param name="prefix">String to prefix the comment</param>
        /// <param name="suffix">String to attach as comment suffix</param>.
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
