using System;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace ToggleComment
{
    /// <summary>
    /// 選択された行のコメントアウト・解除を行うコマンドです。
    /// </summary>
    internal sealed class ToggleCommentCommand : CommandBase
    {
        /// <summary>
        /// コマンドのIDです。
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// 選択された行をコメントアウトするコマンドです。
        /// </summary>
        private const string COMMENT_SELECTION_COMMAND = "Edit.CommentSelection";

        /// <summary>
        /// 選択された行のコメントを解除するコマンドです。
        /// </summary>
        private const string UNCOMMENT_SELECTION_COMMAND = "Edit.UncommentSelection";

        /// <summary>
        /// C# のコメントのパターンです。
        /// </summary>
        private readonly Lazy<CodeCommentPattern[]> commentPatterns =
            new Lazy<CodeCommentPattern[]>(() => new[] { new CodeCommentPattern("//"), new CodeCommentPattern("/*", "*/") });

        /// <summary>
        /// コマンドメニューグループのIDです。
        /// </summary>
        public static readonly Guid CommandSet = new Guid("85542055-97d7-4219-a793-8c077b81b25b");

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static ToggleCommentCommand Instance { get; private set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        private ToggleCommentCommand(Package package) : base(package, CommandId, CommandSet)
        {
        }

        /// <summary>
        /// このコマンドのシングルトンのインスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        public static void Initialize(Package package)
        {
            Instance = new ToggleCommentCommand(package);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
            var textDocument = dte.ActiveDocument.Object("TextDocument") as TextDocument;
            if (textDocument != null)
            {
                var selection = textDocument.Selection;
                SelectLines(selection);

                var command = IsComment(selection.Text) ? UNCOMMENT_SELECTION_COMMAND : COMMENT_SELECTION_COMMAND;
                dte.ExecuteCommand(command);
            }
        }

        /// <summary>
        /// 指定の文字列がコメントかどうかを判定します。
        /// </summary>
        private bool IsComment(string text)
        {
            return commentPatterns.Value.Any(x => x.IsComment(text));
        }

        /// <summary>
        /// 選択中の行を行選択状態にします。
        /// </summary>
        private static void SelectLines(TextSelection selection)
        {
            var bottom = selection.BottomLine;

            selection.MoveToDisplayColumn(selection.TopLine, 1, false);
            selection.MoveToDisplayColumn(bottom, 0, true);
            selection.EndOfLine(true);
        }
    }
}
