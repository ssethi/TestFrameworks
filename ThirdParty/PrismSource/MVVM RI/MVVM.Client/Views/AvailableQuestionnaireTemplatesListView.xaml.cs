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
using MVVM.Client.Infrastructure;
using MVVM.Client.ViewModels;

namespace MVVM.Client.Views
{
    /// <summary>
    /// View for showing the available templates.
    /// </summary>
    /// <remarks>
    /// This view is exported with a contract name so the UI service can resolve it.
    /// </remarks>
    [ExportView(ViewNames.QuestionnaireTemplatesList)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [SuppressMessage("Microsoft.Performance", "CA1823", Justification = "Members generated from xaml")]
    public partial class AvailableQuestionnaireTemplatesListView : UserControl
    {
        public AvailableQuestionnaireTemplatesListView()
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
        public AvailableQuestionnaireTemplatesListViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
    }
}
