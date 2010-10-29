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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.Prism.MefExtensions.Modularity
{
    /// <summary>
    /// Exports the ModuleInitializer using the Managed Extensibility Framework (MEF).
    /// </summary>
    /// <remarks>
    /// This allows the MefBootstrapper to provide this class as a default implementation.
    /// If another implementation is found, this export will not be used.
    /// </remarks>
    [Export(typeof(IModuleInitializer))]
    public class MefModuleInitializer : ModuleInitializer
    {
        // disable the warning that the field is never assigned to, and will always have its default value null
        // as it is imported by MEF
#pragma warning disable 0649
#if SILVERLIGHT
        /// <summary>
        /// Import the available modules from the MEF container
        /// </summary>
        /// <remarks>
        /// Due to Silverlight/MEF restrictions this must be public.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<Lazy<IModule, IModuleExport>> ImportedModules { get; set; }
#else
        /// <summary>
        /// Import the available modules from the MEF container
        /// </summary>
        [ImportMany(AllowRecomposition = true)] 
        private IEnumerable<Lazy<IModule, IModuleExport>> ImportedModules { get; set; }
#endif
#pragma warning restore 0649

        /// <summary>
        /// Initializes a new instance of the <see cref="MefModuleInitializer"/> class.
        /// </summary>
        /// <param name="serviceLocator">The container that will be used to resolve the modules by specifying its type.</param>
        /// <param name="loggerFacade">The logger to use.</param>
        [ImportingConstructor()]
        public MefModuleInitializer(IServiceLocator serviceLocator, ILoggerFacade loggerFacade)
            : base(serviceLocator, loggerFacade)
        {
        }

        /// <summary>
        /// Creates a module.
        /// </summary>
        /// <param name="typeName">The type to create.</param>
        /// <returns>An instance of the type.</returns>
        protected override IModule CreateModule(string typeName)
        {
            if (this.ImportedModules != null && this.ImportedModules.Count() != 0)
            {
                Lazy<IModule, IModuleExport> lazyModule =
                    this.ImportedModules.FirstOrDefault(x => (x.Metadata.ModuleType.AssemblyQualifiedName == typeName));
                if (lazyModule != null)
                {
                    return lazyModule.Value;
                }
            }

            // This does not fall back to the base implementation because the type must be in the MEF container and not just in the application domain.
            throw new ModuleInitializeException(
                string.Format(CultureInfo.CurrentCulture, Properties.Resources.FailedToGetType, typeName));
        }
    }
}