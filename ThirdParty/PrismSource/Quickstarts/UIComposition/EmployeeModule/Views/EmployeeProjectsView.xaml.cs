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
using Microsoft.Practices.Prism.Regions;
using UIComposition.EmployeeModule.Models;
using UIComposition.EmployeeModule.ViewModels;

namespace UIComposition.EmployeeModule.Views
{
    public partial class EmployeeProjectsView : UserControl
    {
        public EmployeeProjectsView(EmployeeProjectsViewModel employeeProjectsViewModel)
        {
            this.InitializeComponent();

            // Set the ViewModel as this View's data context.
            this.DataContext = employeeProjectsViewModel;

            // TODO: 09 - Whenever the RegionContext value is changed, the newly selected employee is set on the view model for each child window.
            // This view is displayed in a region with a region context.
            // The region context is defined as the currently selected employee
            // When the region context is changed, we need to propogate the
            // change to this view's view model.
            RegionContext.GetObservableContext(this).PropertyChanged += (s, e)
                                                                        =>
                                                                        employeeProjectsViewModel.CurrentEmployee =
                                                                        RegionContext.GetObservableContext(this).Value
                                                                        as Employee;
        }
    }
}