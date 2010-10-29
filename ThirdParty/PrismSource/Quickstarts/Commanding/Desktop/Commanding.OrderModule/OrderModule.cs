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
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

using Commanding.Modules.Order.Views;
using Commanding.Modules.Order.Services;

namespace Commanding.Modules.Order
{
    public class OrderModule : IModule
    {
        private readonly IRegionManager  regionManager;
        private readonly IUnityContainer container;

        public OrderModule( IUnityContainer container, IRegionManager regionManager )
        {
            this.container     = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            this.container.RegisterType<IOrdersRepository, OrdersRepository>(new ContainerControlledLifetimeManager());

            // Show the Orders Editor view in the shell's main region.
            this.regionManager.RegisterViewWithRegion( "MainRegion",
                                                       () => this.container.Resolve<OrdersEditorView>() );

            // Show the Orders Toolbar view in the shell's toolbar region.
            this.regionManager.RegisterViewWithRegion( "GlobalCommandsRegion",
                                                       () => this.container.Resolve<OrdersToolBar>() );
        }
    }
}

