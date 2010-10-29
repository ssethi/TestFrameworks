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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuleA.Tests.Mocks;

namespace ModuleA.Tests
{
    [TestClass]
    public class AddFundPresenterFixture
    {
        [TestMethod]
        public void PresenterPublishesFundAddedOnViewAddClick()
        {
            var view = new MockAddFundView();
            var EventAggregator = new MockEventAggregator();
            var mockFundAddedEvent = new MockFundAddedEvent();
            EventAggregator.AddMapping<FundAddedEvent>(mockFundAddedEvent);
            var presenter = new AddFundPresenter(EventAggregator);
            presenter.View = view;
            view.Customer = "99";
            view.Fund = "TestFund";

            view.PublishAddClick();

            Assert.IsTrue(mockFundAddedEvent.PublishCalled);
            Assert.AreEqual("99", mockFundAddedEvent.PublishArgumentPayload.CustomerId);
            Assert.AreEqual("TestFund", mockFundAddedEvent.PublishArgumentPayload.TickerSymbol);
        }


    }

    class MockAddFundView : IAddFundView
    {
        public void PublishAddClick()
        {
            AddFund(this, EventArgs.Empty);
        }

        public event EventHandler AddFund;

        public string Customer { get; set; }

        public string Fund { get; set; }
    }
}
