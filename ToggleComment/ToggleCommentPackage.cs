using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace ToggleComment
{
    /// <summary>
    /// The package is deployed as an extension.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "2.0", IconResourceID = 400)] // Information displayed in Visual Studio Help/About.
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class ToggleCommentPackage : Package
    {
        /// <summary>
        /// The package ID.
        /// </summary>
        public const string PackageGuidString = "9fb26121-a4a7-4f27-81c3-c713a2464345";

        /// <summary>
        /// Initialise the package.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            ToggleCommentCommand.Initialize(this);
        }
    }
}
