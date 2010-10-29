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
using System.Diagnostics;
using System.Globalization;
using EventAggregation.Infrastructure;
using Microsoft.Practices.Prism.Events;
using ModuleB.Properties;

namespace ModuleB
{
    public class ActivityPresenter
    {
        private string _customerId;
        private IEventAggregator eventAggregator;
        private SubscriptionToken subscriptionToken;

        public ActivityPresenter(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void FundAddedEventHandler(FundOrder fundOrder)
        {
            Debug.Assert(View != null);
            View.AddContent(fundOrder.TickerSymbol);
        }

        public bool FundOrderFilter(FundOrder fundOrder)
        {
            return fundOrder.CustomerId == _customerId;
        }

        public IActivityView View { get; set; }

        public string CustomerId
        {
            get
            {
                return _customerId;
            }

            set
            {
                _customerId = value;

                FundAddedEvent fundAddedEvent = eventAggregator.GetEvent<FundAddedEvent>();

                if (subscriptionToken != null)
                {
                    fundAddedEvent.Unsubscribe(subscriptionToken);
                }

                subscriptionToken = fundAddedEvent.Subscribe(FundAddedEventHandler, ThreadOption.UIThread, false, FundOrderFilter);

                View.SetTitle(string.Format(CultureInfo.CurrentCulture, Resources.ActivityTitle, CustomerId));
            }
        }
    }
}
