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
using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using ModuleTracking;

namespace ModularityWithMef.Desktop
{
    /// <summary>
    /// A module for the quickstart.
    /// </summary>
    [ModuleExport(typeof(ModuleB), InitializationMode = InitializationMode.OnDemand)]
    public class ModuleB : IModule
    {
        private readonly IModuleTracker moduleTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleB"/> class.
        /// </summary>
        /// <param name="moduleTracker">The module tracker.</param>
        [ImportingConstructor]
        public ModuleB(IModuleTracker moduleTracker)
        {
            if (moduleTracker == null)
            {
                throw new ArgumentNullException("moduleTracker");
            }

            this.moduleTracker = moduleTracker;
            this.moduleTracker.RecordModuleConstructed(WellKnownModuleNames.ModuleB);
        }

        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            this.moduleTracker.RecordModuleInitialized(WellKnownModuleNames.ModuleB);
        }
    }
}
