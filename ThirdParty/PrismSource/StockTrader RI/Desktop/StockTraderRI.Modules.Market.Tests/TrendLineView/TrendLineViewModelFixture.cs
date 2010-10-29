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
using Microsoft.Practices.Prism.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Market.Tests.Mocks;
using StockTraderRI.Modules.Market.TrendLine;

namespace StockTraderRI.Modules.Market.Tests.TrendLine
{
    /// <summary>
    /// Unit tests for TrendLineViewModel
    /// </summary>
    [TestClass]
    public class TrendLineViewModelFixture
    {
        [TestMethod]
        public void CanInitPresenter()
        {
            var historyService = new MockMarketHistoryService();
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(x => x.GetEvent<TickerSymbolSelectedEvent>()).Returns(
                new MockTickerSymbolSelectedEvent());
            TrendLineViewModel presentationModel = new TrendLineViewModel(historyService, eventAggregator.Object);

            Assert.IsNotNull(presentationModel);
        }

        [TestMethod]
        public void ShouldUpdateModelWithDataFromServiceOnTickerSymbolSelected()
        {
            var historyService = new MockMarketHistoryService();
            var tickerSymbolSelected = new MockTickerSymbolSelectedEvent();
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(x => x.GetEvent<TickerSymbolSelectedEvent>()).Returns(
                tickerSymbolSelected);

            TrendLineViewModel presentationModel = new TrendLineViewModel(historyService, eventAggregator.Object);

            tickerSymbolSelected.SubscribeArgumentAction("MyTickerSymbol");

            Assert.IsTrue(historyService.GetPriceHistoryCalled);
            Assert.AreEqual("MyTickerSymbol", historyService.GetPriceHistoryArgument);
            Assert.IsNotNull(presentationModel.HistoryCollection);
            Assert.AreEqual(historyService.Data.Count, presentationModel.HistoryCollection.Count);
            Assert.AreEqual(historyService.Data[0], presentationModel.HistoryCollection[0]);
            Assert.AreEqual("MyTickerSymbol", presentationModel.TickerSymbol);
        }
    }

    internal class MockTickerSymbolSelectedEvent : TickerSymbolSelectedEvent
    {
        public Action<string> SubscribeArgumentAction;
        public Predicate<string> SubscribeArgumentFilter;
        public override SubscriptionToken Subscribe(Action<string> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<string> filter)
        {
            SubscribeArgumentAction = action;
            SubscribeArgumentFilter = filter;
            return null;
        }
    }
}
