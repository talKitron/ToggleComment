using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace ToggleComment
{
    /// <summary>
    /// Base class for commands to be registered as extensions.
    /// </summary>
    internal abstract class CommandBase
    {
        /// <summary>
        /// Package providing commands.
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// Get the service provider.
        /// </summary>
        protected IServiceProvider ServiceProvider
        {
            get { return _package; }
        }

        /// <summary>
        /// Instance initialisation.
        /// </summary>
        /// <remarks>
        /// The command must be defined in the .vsct file.
        /// </remarks>
        /// <param name="package">Package providing the command</param>
        /// <param name="commandId">ID of the command</param>
        /// <param name="commandSetId">ID of the command menu group</param>.
        protected CommandBase(Package package, int commandId, Guid commandSetId)
        {
            _package = package ?? throw new ArgumentNullException("package");

            if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                var menuCommandID = new CommandID(commandSetId, commandId);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Command.
        /// </summary>
        protected abstract void Execute(object sender, EventArgs e);

        /// <summary>
        /// Display a message box.
        /// </summary>
        /// <param name="title">Title of the message</param>
        /// <param name="message">Message to display</param>
        /// <param name="icon">Icon to display</param>
        protected void ShowMessageBox(string title, string message, OLEMSGICON icon)
        {
            VsShellUtilities.ShowMessageBox(
                ServiceProvider,
                message,
                title,
                icon,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        /// <summary>
        /// Callback when the command is executed.
        /// </summary>
        /// <param name="sender">Event issuer</param>
        /// <param name="e">event arguments</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                Execute(sender, e);
            }
            catch (Exception ex)
            {
                ShowMessageBox(
                    $"{GetType().Name} is not executable.",
                    $"{ex.GetType().FullName}: {ex.Message}.",
                    OLEMSGICON.OLEMSGICON_WARNING);
            }
        }
    }
}
