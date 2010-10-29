//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
namespace ModularityWithMef.Silverlight
{
    using System;
    using System.ComponentModel.Composition;
    using System.Globalization;
    using System.Windows.Controls;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using ModuleTracking;

    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : UserControl, IPartImportsSatisfiedNotification
    {
        // The shell imports IModuleTracker once to record updates as modules are downloaded.  
        // Due to SilverLight/MEF restrictions this must be public.
        [Import(AllowRecomposition = false)]
        public IModuleTracker moduleTracker;

        // The shell imports IModuleManager once to load modules on-demand.
        // Due to SilverLight/MEF restrictions this must be public.
        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        // The shell imports the logger once to output logs to the UI.
        // Due to SilverLight/MEF restrictions this must be public.
        [Import(AllowRecomposition = false)]
        public CallbackLogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shell"/> class.
        /// </summary>
        public Shell()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Logs the specified message.  Called by the CallbackLogger.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="priority">The priority.</param>
        public void Log(string message, Category category, Priority priority)
        {
            this.TraceTextBox.Text += string.Format(CultureInfo.CurrentUICulture, "[{0}][{1}] {2}\r\n", category, priority, message);
        }

        /// <summary>
        /// Handles the RequestModuleLoad event of the ModuleC control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ModuleC_RequestModuleLoad(object sender, EventArgs e)
        {
            // The ModuleManager uses the Async Events Pattern.
            this.ModuleManager.LoadModule(WellKnownModuleNames.ModuleC);
        }

        /// <summary>
        /// Handles the RequestModuleLoad event of the ModuleE control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ModuleE_RequestModuleLoad(object sender, EventArgs e)
        {
            // The ModuleManager uses the Async Events Pattern.
            this.ModuleManager.LoadModule(WellKnownModuleNames.ModuleE);
        }

        /// <summary>
        /// Handles the RequestModuleLoad event of the ModuleF control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ModuleF_RequestModuleLoad(object sender, EventArgs e)
        {
            // The ModuleManager uses the Async Events Pattern.
            this.ModuleManager.LoadModule(WellKnownModuleNames.ModuleF);
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use.
        /// </summary>
        public void OnImportsSatisfied()
        {
            // IPartImportsSatisfiedNotification is useful when you want to coordinate doing some work
            // with imported parts independent of when the UI is visible.

            // I use the IModuleTracker as the data-context for data-binding.
            // This quickstart only demonstrates modularity for Prism and does not use data-binding patterns such as MVVM.
            this.DataContext = this.moduleTracker;

            // I set this shell's Log method as the callback for receiving log messages.
            this.Logger.Callback = this.Log;
            this.Logger.ReplaySavedLogs();

            // I subscribe to events to help track module loading/loaded progress.
            // The ModuleManager uses the Async Events Pattern.
            this.ModuleManager.ModuleDownloadProgressChanged += this.ModuleManager_ModuleDownloadProgressChanged;
            this.ModuleManager.LoadModuleCompleted += this.ModuleManager_LoadModuleCompleted;
        }

        /// <summary>
        /// Handles the LoadModuleProgressChanged event of the ModuleManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.Practices.Composite.Modularity.LoadModuleProgressChangedEventArgs"/> instance containing the event data.</param>
        private void ModuleManager_ModuleDownloadProgressChanged(object sender, ModuleDownloadProgressChangedEventArgs e)
        {
            this.moduleTracker.RecordModuleDownloading(e.ModuleInfo.ModuleName, e.BytesReceived, e.TotalBytesToReceive);
        }

        /// <summary>
        /// Handles the LoadModuleCompleted event of the ModuleManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.Practices.Composite.Modularity.LoadModuleCompletedEventArgs"/> instance containing the event data.</param>
        private void ModuleManager_LoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            this.moduleTracker.RecordModuleLoaded(e.ModuleInfo.ModuleName);
        }        
    }
}
