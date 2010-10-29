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
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using UIComposition.Shell.Views;

namespace UIComposition.Shell
{
    public class UICompositionBootstrapper : UnityBootstrapper
    {
        // TODO: 02 - The Shell loads the EmployeeModule, as specified in the module catalog (ModuleCatalog.xaml).
        private const string ModuleCatalogUri = "/UIComposition.Shell;component/ModuleCatalog.xaml";

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(
                new Uri(ModuleCatalogUri, UriKind.Relative));
        }

        protected override DependencyObject CreateShell()
        {
            // Use the container to create an instance of the shell.
            ShellView view = this.Container.TryResolve<ShellView>();

            // Set it as the root visual for the application.
            Application.Current.RootVisual = view;

            return view;
        }
    }
}