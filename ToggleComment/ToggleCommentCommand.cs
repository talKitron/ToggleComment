using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ToggleComment.Codes;
using ToggleComment.Utils;

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
        /// コメントのパターンです。
        /// </summary>
        private readonly IDictionary<string, ICodeCommentPattern[]> _patterns = new Dictionary<string, ICodeCommentPattern[]>();

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
                var patterns = _patterns.GetOrAdd(textDocument.Language, CreateCommentPatterns);
                if (0 < patterns.Length)
                {
                    var text = GetTextOfSelectedLines(textDocument.Selection);
                    var isComment = patterns.Any(x => x.IsComment(text));

                    var command = isComment ? UNCOMMENT_SELECTION_COMMAND : COMMENT_SELECTION_COMMAND;
                    dte.ExecuteCommand(command);
                }
                else
                {
                    try
                    {
                        dte.ExecuteCommand(COMMENT_SELECTION_COMMAND);
                    }
                    catch (COMException)
                    {
                        ShowMessageBox(
                            "Toggle Comment is not executable.",
                            $"{textDocument.Language} files is not supported.",
                            OLEMSGICON.OLEMSGICON_INFO);
                    }
                }
            }
        }

        /// <summary>
        /// コードのコメントを表すパターンを作成します。
        /// </summary>
        private static ICodeCommentPattern[] CreateCommentPatterns(string language)
        {
            switch (language)
            {
                case "CSharp":
                case "C/C++":
                case "TypeScript":
                    {
                        return new ICodeCommentPattern[] { new LineCommentPattern("//"), new BlockCommentPattern("/*", "*/") };
                    }
                case "XML":
                case "XAML":
                    {
                        return new[] { new BlockCommentPattern("<!--", "-->") };
                    }
                case "HTMLX":
                    {
                        // MEMO : HTML に埋め込まれたCSS, JavaScriptをサポートする
                        return new ICodeCommentPattern[] {
                            new BlockCommentPattern("<!--", "-->"),
                            new BlockCommentPattern("/*", "*/"),
                            new LineCommentPattern("//")};
                    }
                case "HTML":
                    {
                        // MEMO : VS の UncommentSelection コマンドがブロックコメント <%/* */%> に対応していない
                        return new ICodeCommentPattern[] {
                            new BlockCommentPattern("<!--", "-->"),
                            new BlockCommentPattern("<%--", "--%>"),
                            new BlockCommentPattern("/*", "*/"),
                            new LineCommentPattern("//")};
                    }
                case "JavaScript":
                case "F#":
                    {
                        // MEMO : VS の UncommentSelection コマンドが JavaScript, F# のブロックコメントに対応していない
                        return new[] { new LineCommentPattern("//") };
                    }
                case "CSS":
                    {
                        return new[] { new BlockCommentPattern("/*", "*/") };
                    }
                case "PowerShell":
                    {
                        // MEMO : VS の UncommentSelection コマンドが PowerShell のブロックコメントに対応していない
                        return new[] { new LineCommentPattern("#") };
                    }
                case "SQL Server Tools":
                    {
                        return new[] { new LineCommentPattern("--") };
                    }
                case "Basic":
                    {
                        return new[] { new LineCommentPattern("'") };
                    }
                default:
                    {
                        return new ICodeCommentPattern[0];
                    }
            }
        }

        /// <summary>
        /// 選択中の全ての行のテキストを取得します。
        /// </summary>
        private static string GetTextOfSelectedLines(TextSelection selection)
        {
            var startPoint = selection.TopPoint.CreateEditPoint();
            startPoint.StartOfLine();
            var endPoint = selection.BottomPoint.CreateEditPoint();
            endPoint.EndOfLine();

            return startPoint.GetText(endPoint);
        }
    }
}
