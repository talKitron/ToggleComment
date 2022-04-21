using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToggleComment.Codes
{
    /// <summary>
    /// A class representing a line comment pattern.
    /// </summary>
    public class LineCommentPattern : ICodeCommentPattern
    {
        /// <summary>
        /// Regular expression pattern to determine if it is a comment or not.
        /// </summary>
        private readonly Regex _regexPattern;

        /// <summary>
        /// Gets the string to prefix the comment with.
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// Instance initialisation.
        /// </summary>
        /// <param name="prefix">String to prefix the comment with</param>.

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