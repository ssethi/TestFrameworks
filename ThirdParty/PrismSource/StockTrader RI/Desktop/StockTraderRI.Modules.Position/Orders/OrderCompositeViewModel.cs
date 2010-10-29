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
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Models;

namespace StockTraderRI.Modules.Position.Orders
{
    [Export(typeof(IOrderCompositeViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class OrderCompositeViewModel : DependencyObject, IOrderCompositeViewModel, IHeaderInfoProvider<string>
    {
        private readonly IOrderDetailsViewModel orderDetailsViewModel;

        public static readonly DependencyProperty HeaderInfoProperty =
            DependencyProperty.Register("HeaderInfo", typeof(string), typeof(OrderCompositeViewModel), null);

        [ImportingConstructor]
        public OrderCompositeViewModel(IOrderDetailsViewModel orderDetailsViewModel)
        {
            this.orderDetailsViewModel = orderDetailsViewModel;
            this.orderDetailsViewModel.CloseViewRequested += _orderPresenter_CloseViewRequested;
        }

        void _orderPresenter_CloseViewRequested(object sender, EventArgs e)
        {
            OnCloseViewRequested(sender, e);
        }

        partial void SetTransactionInfo(TransactionInfo transactionInfo);

        private void OnCloseViewRequested(object sender, EventArgs e)
        {
            CloseViewRequested(sender, e);
        }

        public event EventHandler CloseViewRequested = delegate { };

        public TransactionInfo TransactionInfo
        {
            get { return this.orderDetailsViewModel.TransactionInfo; }
            set { SetTransactionInfo(value); }

        }

        public ICommand SubmitCommand
        {
            get { return this.orderDetailsViewModel.SubmitCommand; }
        }

        public ICommand CancelCommand
        {
            get { return this.orderDetailsViewModel.CancelCommand; }
        }

        public int Shares
        {
            get { return this.orderDetailsViewModel.Shares ?? 0; }
        }

        public object OrderDetails
        {
            get { return this.orderDetailsViewModel; }
        }
    }
}