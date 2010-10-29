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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuleB.Tests.Mocks;
using SubscriptionToken = Microsoft.Practices.Prism.Events.SubscriptionToken;

namespace ModuleB.Tests
{
    [TestClass]
    public class ActivityPresenterFixture
    {
        [TestMethod]
        public void PresenterAddsFundOnEvent()
        {
            var view = new MockActivityView();
            var mockEventAggregator = new MockEventAggregator();

            MockFundAddedEvent mockEvent = new MockFundAddedEvent();
            mockEventAggregator.AddMapping<FundAddedEvent>(mockEvent);

            ActivityPresenter presenter = new ActivityPresenter(mockEventAggregator);
            presenter.View = view;
            string customerId = "ALFKI";
            presenter.CustomerId = customerId;
            FundOrder payload = new FundOrder() { CustomerId = customerId, TickerSymbol = "MSFT" };
            mockEvent.SubscribeArgumentAction(payload);
            StringAssert.Contains(view.AddContentArgumentContent, "MSFT");
        }

        [TestMethod]
        public void PresenterSubscribesToFundAddedForCorrectCustomer()
        {
            var mockEventAggregator = new MockEventAggregator();

            MockFundAddedEvent mockEvent = new MockFundAddedEvent();
            mockEventAggregator.AddMapping<FundAddedEvent>(mockEvent);
            ActivityPresenter presenter = new ActivityPresenter(mockEventAggregator);
            presenter.View = new MockActivityView();

            presenter.CustomerId = "ALFKI";

            Assert.IsTrue(mockEvent.SubscribeArgumentFilter(new FundOrder { CustomerId = "ALFKI" }));
            Assert.AreEqual(ThreadOption.UIThread, mockEvent.ThreadOption);
            Assert.IsFalse(mockEvent.SubscribeArgumentFilter(new FundOrder { CustomerId = "FilteredOutCustomer" }));
        }

        [TestMethod]
        public void PresenterUnsubcribesFromFundAddedEventIfCustomerIDIsSetTwice()
        {
            var mockEventAggregator = new MockEventAggregator();

            MockFundAddedEvent mockEvent = new MockFundAddedEvent();
            mockEventAggregator.AddMapping<FundAddedEvent>(mockEvent);
            ActivityPresenter presenter = new ActivityPresenter(mockEventAggregator);
            presenter.View = new MockActivityView();

            presenter.CustomerId = "ALFKI";
            presenter.CustomerId = "ALFKI";

            Assert.AreEqual(1, mockEvent.SubscribeCount);
        }
    }

    internal class MockFundAddedEvent : FundAddedEvent
    {
        public Action<FundOrder> SubscribeArgumentAction;
        public Predicate<FundOrder> SubscribeArgumentFilter;
        public ThreadOption ThreadOption;
        public int SubscribeCount;

        public override SubscriptionToken Subscribe(Action<FundOrder> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<FundOrder> filter)
        {
            SubscribeArgumentAction = action;
            SubscribeArgumentFilter = filter;
            ThreadOption = threadOption;
            SubscribeCount++;
            return new SubscriptionToken();
        }

        public override void Unsubscribe(SubscriptionToken subscriptionToken)
        {
            SubscribeCount--;
        }
    }

    class MockActivityView : IActivityView
    {
        public string AddContentArgumentContent;

        public void SetTitle(string title)
        {

        }

        public void SetCustomerId(string customerId)
        {

        }

        public void AddContent(string content)
        {
            AddContentArgumentContent = content;
        }
    }
}
