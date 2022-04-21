using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ToggleComment.Codes;
using ToggleComment.Utils;

namespace ToggleComment
{
    /// <summary>
    /// This command is used to comment out/uncomment selected lines.
    /// </summary>
    internal sealed class ToggleCommentCommand : CommandBase
    {
        /// <summary>
        /// Instances to which the execution of commands is delegated.
        /// </summary>
        private readonly IOleCommandTarget _commandTarget;

        /// <summary>
        /// ID of the command.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Comment Patterns.
        /// </summary>
        private readonly IDictionary<string, ICodeCommentPattern[]> _patterns = new Dictionary<string, ICodeCommentPattern[]>();

        /// <summary>
        /// Command menu group ID.
        /// </summary>
        public static readonly Guid CommandSet = new Guid("85542055-97d7-4219-a793-8c077b81b25b");

        /// <summary>
        /// Get a singleton instance.
        /// </summary>
        public static ToggleCommentCommand Instance { get; private set; }

        /// <summary>
        /// Instance initialisation.
        /// </summary>
        /// <param name="package">The package providing the command</param>.
        private ToggleCommentCommand(Package package) : base(package, CommandId, CommandSet)
        {
            _commandTarget = (IOleCommandTarget)ServiceProvider.GetService(typeof(SUIHostCommandDispatcher));
        }

        /// <summary>
        /// Initialise a singleton instance of this command.
        /// </summary>
        /// <param name="package">Package providing the command</param>.
        public static void Initialize(Package package)
        {
            Instance = new ToggleCommentCommand(package);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));
            if (dte?.ActiveDocument.Object("TextDocument") is TextDocument textDocument)
            {
                var patterns = _patterns.GetOrAdd(textDocument.Language, CreateCommentPatterns);
                if (0 < patterns.Length)
                {
                    var selection = textDocument.Selection;
                    SelectLines(selection);
                    var text = selection.Text;

                    var isComment = patterns.Any(x => x.IsComment(text));
                    var commandId = isComment ? VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK : VSConstants.VSStd2KCmdID.COMMENT_BLOCK;

                    ExecuteCommand(commandId);
                }
                else if (ExecuteCommand(VSConstants.VSStd2KCmdID.COMMENT_BLOCK) == false)
                {
                    ShowMessageBox(
                        "Toggle Comment is not executable.",
                        $"{textDocument.Language} files is not supported.",
                        OLEMSGICON.OLEMSGICON_INFO);
                }
            }
        }

        /// <summary>
        /// Create patterns to represent code comments.
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
                        // MEMO : Support for CSS, JavaScript embedded in HTML
                        return new ICodeCommentPattern[] {
                            new BlockCommentPattern("<!--", "-->"),
                            new BlockCommentPattern("@*", "*@"),
                            new BlockCommentPattern("/*", "*/"),
                            new LineCommentPattern("//")};
                    }
                case "HTML":
                    {
                        // MEMO : VS UncommentSelection command does not support block comments <%/* */%>.
                        return new ICodeCommentPattern[] {
                            new BlockCommentPattern("<!--", "-->"),
                            new BlockCommentPattern("<%--", "--%>"),
                            new BlockCommentPattern("/*", "*/"),
                            new LineCommentPattern("//")};
                    }
                case "JavaScript":
                case "F#":
                    {
                        // MEMO : VS UncommentSelection command does not support JavaScript, F# block comments
                        return new[] { new LineCommentPattern("//") };
                    }
                case "CSS":
                    {
                        return new[] { new BlockCommentPattern("/*", "*/") };
                    }
                case "PowerShell":
                    {
                        // MEMO : VS UncommentSelection command does not support PowerShell block comments
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
                case "Python":
                    {
                        return new[] { new LineCommentPattern("#") };
                    }
                default:
                    {
                        return new ICodeCommentPattern[0];
                    }
            }
        }

        /// <summary>
        /// Executes the specified command.
        /// Returns <see langword="false"/> if the command could not be executed.
        /// </summary>
        private bool ExecuteCommand(VSConstants.VSStd2KCmdID commandId)
        {
            var grooupId = VSConstants.VSStd2K;
            var result = _commandTarget.Exec(ref grooupId, (uint)commandId, 0, IntPtr.Zero, IntPtr.Zero);

            return result == VSConstants.S_OK;
        }

        /// <summary>
        /// Makes the currently selected row into a row selection.
        /// </summary>
        private static void SelectLines(TextSelection selection)
        {
            var startPoint = selection.TopPoint.CreateEditPoint();
            startPoint.StartOfLine();

            var endPoint = selection.BottomPoint.CreateEditPoint();
            if (endPoint.AtStartOfLine == false || startPoint.Line == endPoint.Line)
            {
                endPoint.EndOfLine();
            }

            if (selection.Mode == vsSelectionMode.vsSelectionModeBox)
            {
                selection.Mode = vsSelectionMode.vsSelectionModeStream;
            }

            selection.MoveToPoint(startPoint);
            selection.MoveToPoint(endPoint, true);
        }
    }
}
