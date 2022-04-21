namespace ToggleComment.Codes
{
    /// <summary>
    /// The interface representing the code comment pattern.
    /// </summary>
    public interface ICodeCommentPattern
    {
        /// <summary>
        /// Determines if the specified text is a comment.
        /// </summary>
        /// <param name="text">Text to be judged</param>
        /// <returns>If it is a comment <see langword = "true" /></returns>
        bool IsComment(string text);
    }
}
