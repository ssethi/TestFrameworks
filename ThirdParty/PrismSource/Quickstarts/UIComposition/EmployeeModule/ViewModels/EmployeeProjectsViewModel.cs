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
using System.ComponentModel;
using System.Windows.Data;
using UIComposition.EmployeeModule.Models;
using UIComposition.EmployeeModule.Services;

namespace UIComposition.EmployeeModule.ViewModels
{
    /// <summary>
    /// View model to support the Employee Projects view.
    /// </summary>
    public class EmployeeProjectsViewModel : INotifyPropertyChanged
    {
        public EmployeeProjectsViewModel(IEmployeeDataService dataService)
        {
            if (dataService == null) throw new ArgumentNullException("dataService");

            // Initialize a CollectionView for the project list.
            this.Projects = new PagedCollectionView(dataService.GetProjects());
        }

        public string ViewName
        {
            get { return "Employee Projects"; }
        }

        private Employee currentEmployee;

        public Employee CurrentEmployee
        {
            get { return this.currentEmployee; }
            set
            {
                this.currentEmployee = value;

                // Filter the list of projects to those that are assigned to the current employee.
                if (this.CurrentEmployee != null)
                    this.Projects.Filter = obj => ((Project) obj).Id == this.CurrentEmployee.Id;
                this.Projects.Refresh();

                this.NotifyPropertyChanged("CurrentEmployee");
                this.NotifyPropertyChanged("Projects");
            }
        }

        public ICollectionView Projects { get; private set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}