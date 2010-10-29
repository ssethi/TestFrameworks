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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;

namespace ModuleA
{
    /// <summary>
    /// Interaction logic for AddFundView.xaml
    /// </summary>
    public partial class AddFundView : UserControl, IAddFundView
    {
        private AddFundPresenter _presenter;

        public AddFundView()
        {
            InitializeComponent();
            this.AddButton.Click += new RoutedEventHandler(AddButton_Click);
        }

        public AddFundView(AddFundPresenter presenter) : this()
        {
            _presenter = presenter;
            _presenter.View = this;
        }

        void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddFund(this, null);
        }

        #region IAddFundView Members

        public event EventHandler AddFund = delegate { };

        public string Customer
        {
            get
            {
                ComboBoxItem selectedItem = this.CustomerCbx.SelectedItem as ComboBoxItem;
                if (selectedItem == null)
                    return string.Empty;
                
                return selectedItem.Content.ToString();


            }
        }

        public string Fund
        {
            get
            {
                ComboBoxItem selectedItem = this.FundCbx.SelectedItem as ComboBoxItem;
                if (selectedItem == null)
                    return string.Empty;

                return selectedItem.Content.ToString();
            }
        }

        #endregion
    }
}
