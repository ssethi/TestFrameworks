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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using StockTraderRI.Resources;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;

namespace StockTraderRI
{
    [Export]
    public partial class Shell : UserControl
    {
        private const int VisibleGridDefaultHeight = 5;

        public Shell()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Shell_Loaded);
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
        public ShellViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        } 

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            var story = (Storyboard)this.Resources[ResourceNames.EntryStoryboardName];
            story.Begin();
        }      

        private void ActionControl_LayoutUpdated(object sender, System.EventArgs e)
        {
            Visibility newVisibility = (this.ActionControl.Content != null) ? Visibility.Visible : Visibility.Collapsed;
            if (this.ActionControl.Visibility != newVisibility)
            {
                this.ActionControl.Visibility = newVisibility;
                this.ActionRow.Height = new GridLength(newVisibility == Visibility.Visible ? VisibleGridDefaultHeight : 0, GridUnitType.Star);
            }
        }
    }
}
