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

namespace ModuleB
{
    /// <summary>
    /// Interaction logic for ActivityView.xaml
    /// </summary>
    public partial class ActivityView : UserControl, IActivityView
    {
        private ActivityPresenter presenter;

        public ActivityView()
        {
            InitializeComponent();
        }

        public ActivityView(ActivityPresenter presenter)
            : this()
        {
            this.presenter = presenter;
            presenter.View = this;
        }

        #region IActivityView Members

        public void AddContent(string content)
        {
            this.ContentPanel.Children.Add(new TextBlock() { Text = content });
        }

        public void SetTitle(string title)
        {
            this.ActivityLabel.Content = title;
        }

        public void SetCustomerId(string customerId)
        {
            this.presenter.CustomerId = customerId;
        }

        #endregion
    }
}
