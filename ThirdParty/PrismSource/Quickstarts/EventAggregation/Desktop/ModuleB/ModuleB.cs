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

namespace ModuleB
{
    public class ModuleB : IModule
    {
        public ModuleB(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
            
            RegisterViewsAndServices();
        }

        private void RegisterViewsAndServices()
        {
            this.Container.RegisterType<IActivityView, ActivityView>();
        }

        public void Initialize()
        {
            ActivityView activityView1 = Container.Resolve<ActivityView>();
            ActivityView activityView2 = Container.Resolve<ActivityView>();

            activityView1.SetCustomerId("Customer1");
            activityView2.SetCustomerId("Customer2");

            IRegion rightRegion = RegionManager.Regions["RightRegion"];
            rightRegion.Add(activityView1);
            rightRegion.Add(activityView2);
        }

        public IUnityContainer Container { get; private set; }

        public IRegionManager RegionManager { get; private set; }
    }
}
