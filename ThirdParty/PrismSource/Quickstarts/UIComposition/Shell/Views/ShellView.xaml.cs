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
using System.Windows.Controls;
using UIComposition.Shell.ViewModels;

namespace UIComposition.Shell.Views
{
    public partial class ShellView : UserControl
    {
        // TODO: 01 - The Shell defines two regions: LeftRegion and MainRegion (ShellView.xaml).
        public ShellView(ShellViewModel viewModel)
        {
            this.InitializeComponent();

            // Set the ViewModel as this View's data context.
            this.DataContext = viewModel;
        }
    }
}