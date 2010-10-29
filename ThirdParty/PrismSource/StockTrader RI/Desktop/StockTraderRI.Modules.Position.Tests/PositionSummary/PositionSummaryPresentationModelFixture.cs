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
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Tests.Mocks;

namespace StockTraderRI.Modules.Position.Tests.PositionSummary
{
    [TestClass]
    public class PositionSummaryPresentationModelFixture
    {
        MockOrdersController ordersController;
        Mock<IEventAggregator> eventAggregator;
        MockObservablePosition observablePosition;

        [TestInitialize]
        public void SetUp()
        {
            ordersController = new MockOrdersController();
            observablePosition = new MockObservablePosition();
            eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(x => x.GetEvent<TickerSymbolSelectedEvent>()).Returns(
                new MockTickerSymbolSelectedEvent());
        }

        [TestMethod]
        public void ShouldTriggerPropertyChangedEventOnCurrentPositionSummaryItemChange()
        {
            PositionSummaryViewModel presentationModel = CreatePresentationModel();
            string changedPropertyName = string.Empty;

            presentationModel.PropertyChanged += (o, e) =>
                                                     {
                                                         changedPropertyName = e.PropertyName;
                                                     };

            presentationModel.CurrentPositionSummaryItem = new PositionSummaryItem("NewTickerSymbol", 0, 0, 0);

            Assert.AreEqual("CurrentPositionSummaryItem", changedPropertyName);
        }

        [TestMethod]
        public void TickerSymbolSelectedPublishesEvent()
        {
            var tickerSymbolSelectedEvent = new MockTickerSymbolSelectedEvent();
            eventAggregator.Setup(x => x.GetEvent<TickerSymbolSelectedEvent>()).Returns(tickerSymbolSelectedEvent);
            var presentationModel = CreatePresentationModel();

            presentationModel.CurrentPositionSummaryItem = new PositionSummaryItem("FUND0", 0, 0, 0);

            Assert.IsTrue(tickerSymbolSelectedEvent.PublishCalled);
            Assert.AreEqual("FUND0", tickerSymbolSelectedEvent.PublishArgumentPayload);
        }

        [TestMethod]
        public void ControllerCommandsSetIntoPresentationModel()
        {
            var presentationModel = CreatePresentationModel();

            Assert.AreSame(presentationModel.BuyCommand, ordersController.BuyCommand);
            Assert.AreSame(presentationModel.SellCommand, ordersController.SellCommand);
        }

        private PositionSummaryViewModel CreatePresentationModel()
        {
            return new PositionSummaryViewModel(ordersController, eventAggregator.Object, observablePosition);

        }

        [TestMethod]
        public void CurrentItemNullDoesNotThrow()
        {
            var model = CreatePresentationModel();

            model.CurrentPositionSummaryItem = null;
        }
    }

    internal class MockTickerSymbolSelectedEvent : TickerSymbolSelectedEvent
    {
        public bool PublishCalled;
        public string PublishArgumentPayload;


        public override void Publish(string payload)
        {
            PublishCalled = true;
            PublishArgumentPayload = payload;
        }
    }

    internal class MockObservablePosition : IObservablePosition
    {
        public ObservableCollection<PositionSummaryItem> Items { get; set; }
    }
}

/*
 * 
 * market update with volume change should be reflected in presentation model
 */
