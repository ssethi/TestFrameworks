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
using System.Windows.Controls;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RegionNavigation.Infrastructure;

namespace RegionNavigation
{
    [Export]
    public partial class Shell : UserControl, IPartImportsSatisfiedNotification
    {
        private const string EmailModuleName = "RegionNavigation.Email";
        private static Uri InboxViewUri = new Uri("/InboxView", UriKind.Relative);
        public Shell()
        {
            InitializeComponent();
        }

        [Import(AllowRecomposition = false)]
        public IModuleManager ModuleManager;

        [Import(AllowRecomposition = false)]
        public IRegionManager RegionManager;

        public void OnImportsSatisfied()
        {
            this.ModuleManager.LoadModuleCompleted +=
                (s, e) =>
                {
                    // todo: 01 - Navigation on when modules are loaded.
                    // When using region navigation, be sure to use it consistently
                    // to ensure you get proper journal behavior.  If we mixed
                    // usage of adding views directly to regions, such as through
                    // RegionManager.AddToRegion, and then use RegionManager.RequestNavigate,
                    // we may not be able to navigate back correctly.
                    // 
                    // Here, we wait until the module we want to start with is
                    // loaded and then navigate to the view we want to display
                    // initially.
                    //     
                    if (e.ModuleInfo.ModuleName == EmailModuleName)
                    {
                        this.RegionManager.RequestNavigate(
                            RegionNames.MainContentRegion,
                            InboxViewUri,
                            nr => { });
                    }
                };
        }
    }
}
