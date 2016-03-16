using System;
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
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new ToggleCommentCommand(package);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
        }
    }
}
