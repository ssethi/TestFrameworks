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
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows.Data;
using StockTraderRI.Infrastructure;

namespace StockTraderRI.Modules.Watch.WatchList
{
    [ViewExport(RegionName = RegionNames.MainRegion)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class WatchListView : UserControl
    {
        public WatchListView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the ViewModel.
        /// </summary>
        /// <remarks>
        /// This set-only property is annotated with the <see cref="ImportAttribute"/> so it is injected by MEF with
        /// the appropriate view model.
        /// </remarks>
        [Import]
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Justification = "Needs to be a property to be composed by MEF")]
        public WatchListViewModel ViewModel
        {        
            set
            {
                this.DataContext = value;
                this.OnDataContextChanged();
            }
        }

        private void OnDataContextChanged()
        {
            // Because in Silverlight you cannot bind to a RelativeSource, we are using Resources with an observable value,
            // in order to be able to bind to the Buy and Sell commands. 
            // The resources are declared in the XAML, because Silverlight has StaticResource markup only, so these
            // resources should be available when the control is initializing, even though the Value is yet not set.
            Binding binding = new Binding("RemoveWatchCommand");
            binding.Source = this.DataContext;
            ((ObservableCommand)this.Resources["RemoveWatchCommand"]).SetBinding(ObservableCommand.ValueProperty, binding);
        }
    }
}
