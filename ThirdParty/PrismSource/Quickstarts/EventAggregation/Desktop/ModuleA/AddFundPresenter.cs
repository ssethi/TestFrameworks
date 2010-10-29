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
using EventAggregation.Infrastructure;
using Microsoft.Practices.Prism.Events;

namespace ModuleA
{
    public class AddFundPresenter
    {
        private IAddFundView _view;
        private IEventAggregator eventAggregator;

        public AddFundPresenter(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        void AddFund(object sender, EventArgs e)
        {
            FundOrder fundOrder = new FundOrder();
            fundOrder.CustomerId = View.Customer;
            fundOrder.TickerSymbol = View.Fund;

            if (!string.IsNullOrEmpty(fundOrder.CustomerId) && !string.IsNullOrEmpty(fundOrder.TickerSymbol))
                eventAggregator.GetEvent<FundAddedEvent>().Publish(fundOrder);
        }

        public IAddFundView View
        {
            get { return _view; }
            set
            {
                _view = value;
                _view.AddFund += AddFund;
            }
        }

    }
}
