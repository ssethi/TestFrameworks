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
using System.Windows;
using System.Windows.Controls;

namespace MVVM.Client.Tests.TestObjects
{
    [Export("TestChildWindow", typeof(ChildWindow))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class TestChildWindow : ChildWindow
    {
        public static TestChildWindow currentInstance;

        public TestChildWindow()
        {
            InitializeComponent();

            currentInstance = this;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        [Import]
        public TestChildWindowModel ViewModel
        {
            set { this.DataContext = value; }
        }

        internal static TestChildWindow GetAndResetCurrentInstance()
        {
            var instance = currentInstance;
            currentInstance = null;

            return instance;
        }
    }
}

