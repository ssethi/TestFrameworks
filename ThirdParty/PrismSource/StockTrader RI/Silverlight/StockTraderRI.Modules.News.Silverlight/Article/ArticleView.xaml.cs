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
using System.Windows;
using System.Windows.Controls;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.News.Controllers;

namespace StockTraderRI.Modules.News.Article
{
    [ViewExport(RegionName = RegionNames.ResearchRegion)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ArticleView : UserControl
    {
        // Note - this import is here so that the controller is created and gets wired to the article and news reader
        // view models, which are shared instances
        [Import]
        public INewsController newsController;

        public ArticleView()
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
        public ArticleViewModel ViewModel
        {          
            set
            {
                this.DataContext = value;
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.NewsList.SelectedItem != null)
            {
                VisualStateManager.GoToState(this, "Details", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "List", true);
            }
        }
    }
}
