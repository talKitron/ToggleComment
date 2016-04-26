namespace ToggleComment.Codes
{
    /// <summary>
    /// コードコメントのパターンを表すインターフェースです。
    /// </summary>
    public interface ICodeCommentPattern
    {
        /// <summary>
        /// 指定のテキストがコメントかどうかを判定します。
        /// </summary>
        /// <param name="text">判定対象のテキスト</param>
        /// <returns>コメントの場合は<see langword = "true" /></returns>
        bool IsComment(string text);
    }
}