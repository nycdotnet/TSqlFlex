//------------------------------------------------------------------------------
// <copyright file="OpenTSqlFlex.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TSqlFlex.Core;

namespace TSqlFlex.VsixExtension
{
    internal sealed class OpenTSqlFlex
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("8f9af005-0c05-41d4-ade0-0ec20bd2eb39");
        private readonly Package package;
        public const int ToolbarId = 0x1000;

        private TSqlFlexToolWindow window;
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenTSqlFlex"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private OpenTSqlFlex(Package package)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        public static OpenTSqlFlex Instance { get; private set; }
        private IServiceProvider ServiceProvider { get { return package; } }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new OpenTSqlFlex(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            window = (TSqlFlexToolWindow)package.FindToolWindow(typeof(TSqlFlexToolWindow), 0, true);
            if (null == window?.Frame)
            {
                throw new NotSupportedException("Cannot create T-SQL Flex tool window.");
            }

            var windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
