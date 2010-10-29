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
// using Microsoft.Practices.Prism.Modularity;
// using Microsoft.Practices.Prism.MefExtensions.Modularity;

namespace StockTraderRI.Modules.Watch
{
    /// <summary>
    /// A placeholder to initialize this module.
    /// </summary>
    /// <remarks>
    /// This module is intentionally left empty because views, services, and other types are discovered through declarative attributes.
    /// View registration for this module is done through the <see cref="StockTraderRI.Infrastructure.ViewExportAttribute"/>.
    /// If you extend this reference implementation and need to initialization when this module is loaded, 
    /// uncomment the module export attribute, IModule interface, Initialize method.
    /// </remarks>
    //[ModuleExport(typeof(WatchModule)]
    public class WatchModule
    {
        // public void Initialize() { }
    }
}
