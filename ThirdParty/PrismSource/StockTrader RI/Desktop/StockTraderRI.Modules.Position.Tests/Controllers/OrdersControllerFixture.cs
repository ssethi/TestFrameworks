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
using System.Windows.Input;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Tests.Mocks;

namespace StockTraderRI.Modules.Position.Tests.Controllers
{
    [TestClass]
    public class OrdersControllerFixture
    {
        private MockRegionManager regionManager;
        private MockRegion ordersRegion;

        [TestInitialize]
        public void SetUp()
        {
            regionManager = new MockRegionManager();
            regionManager.Regions.Add("ActionRegion", new MockRegion());
            ordersRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", ordersRegion);
        }

        [TestMethod]
        public void BuyAndSellCommandsInvokeController()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var buyOrderCompositePresenter = new MockOrderCompositePresentationModel();
                var sellOrderCompositePresenter = new MockOrderCompositePresentationModel();

                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, null);

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(buyOrderCompositePresenter);
                controller.BuyCommand.Execute("STOCK01");

                Assert.AreEqual("STOCK01", controller.StartOrderArgumentTickerSymbol);
                Assert.AreEqual(TransactionType.Buy, controller.StartOrderArgumentTransactionType);

                // Set new CompositePresentationModel to simulate resolution of new instance.
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(sellOrderCompositePresenter);
                controller.SellCommand.Execute("STOCK02");

                Assert.AreEqual("STOCK02", controller.StartOrderArgumentTickerSymbol);
                Assert.AreEqual(TransactionType.Sell, controller.StartOrderArgumentTransactionType);
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void ControllerAddsViewIfNotPresent()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, null);

                var collapsibleRegion = (MockRegion)regionManager.Regions["ActionRegion"];

                Assert.AreEqual<int>(0, collapsibleRegion.AddedViews.Count);
                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
                Assert.AreEqual<int>(1, collapsibleRegion.AddedViews.Count);

            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void ControllerAddsANewOrderOnStartOrder()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, null);

                Assert.AreEqual<int>(0, ordersRegion.AddedViews.Count);
                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
                Assert.AreEqual<int>(1, ordersRegion.AddedViews.Count);
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void NewOrderIsShownOrder()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, null);

                Assert.AreEqual<int>(0, ordersRegion.AddedViews.Count);
                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
                Assert.AreSame(ordersRegion.SelectedItem, ordersRegion.AddedViews[0]);
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void StartOrderHooksInstanceCommandsToGlobalSaveAllAndCancelAllCommands()
        {
            try
            {

                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, null);
                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

                Assert.IsFalse(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);
                commandProxy.SubmitAllOrdersCommand.Execute(null);
                Assert.IsTrue(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);

                Assert.IsFalse(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
                commandProxy.CancelAllOrdersCommand.Execute(null);
                Assert.IsTrue(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void StartOrderHooksInstanceCommandsToGlobalSaveAndCancelCommands()
        {
            try
            {

                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, null);
                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

                Assert.IsFalse(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);
                commandProxy.SubmitOrderCommand.Execute(null);
                Assert.IsTrue(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);

                Assert.IsFalse(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
                commandProxy.CancelOrderCommand.Execute(null);
                Assert.IsTrue(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void OnCloseViewRequestedTheControllerUnhooksGlobalCommands()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, null);
                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

                Assert.AreEqual(1, ordersRegion.AddedViews.Count);

                // Act
                orderCompositePresenter.RaiseCloseViewRequested();

                // Verify
                Assert.AreEqual(0, ordersRegion.AddedViews.Count);

                commandProxy.SubmitAllOrdersCommand.Execute(null);
                Assert.IsFalse(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);

                commandProxy.CancelAllOrdersCommand.Execute(null);
                Assert.IsFalse(orderCompositePresenter.MockCancelCommand.ExecuteCalled);

                commandProxy.SubmitOrderCommand.Execute(null);
                Assert.IsFalse(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);

                commandProxy.CancelOrderCommand.Execute(null);
                Assert.IsFalse(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void StartOrderCreatesCompositePMAndPassesCorrectInitInfo()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, null);

                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

                Assert.AreEqual("STOCK01", orderCompositePresenter.TransactionInfo.TickerSymbol);
                Assert.AreEqual(TransactionType.Buy, orderCompositePresenter.TransactionInfo.TransactionType);

            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void SubmitAllInstanceCommandHookedToGlobalSubmitAllCommands()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var accountPositionService = new MockAccountPositionService();
                accountPositionService.AddPosition("STOCK01", 10.0M, 100);

                var controller = new TestableOrdersController(regionManager, commandProxy, accountPositionService);
                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

                Assert.IsFalse(controller.SubmitAllCommandCalled);
                commandProxy.SubmitAllOrdersCommand.CanExecute(null);
                Assert.IsTrue(controller.SubmitAllCommandCalled);

            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }

        }

        [TestMethod]
        public void CannotSellMoreSharesThanAreOwned()
        {
            try
            {

                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var accountPositionService = new MockAccountPositionService();
                accountPositionService.AddPosition("STOCK01", 10.0M, 100);

                var controller = new TestableOrdersController(regionManager, commandProxy, accountPositionService);

                // Act
                var buyOrder = new MockOrderCompositePresentationModel() { Shares = 100, };
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(buyOrder);
                controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
                Assert.IsTrue(controller.SubmitAllVoteOnlyCommand.CanExecute());

                var sellOrder = new MockOrderCompositePresentationModel() { Shares = 200 };
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(sellOrder);
                controller.InvokeStartOrder(TransactionType.Sell, "STOCK01");

                //Should not be able to sell even though owned shares==100, buy==100 and sell==200
                Assert.IsFalse(controller.SubmitAllVoteOnlyCommand.CanExecute());
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void CannotSellMoreSharesThanAreOwnedInDifferentOrders()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var accountPositionService = new MockAccountPositionService();
                accountPositionService.AddPosition("STOCK01", 10.0M, 100);

                var controller = new TestableOrdersController(regionManager, commandProxy, accountPositionService);
                var sellOrder1 = new MockOrderCompositePresentationModel() { Shares = 100 };
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(sellOrder1);

                controller.InvokeStartOrder(TransactionType.Sell, "STOCK01");

                Assert.IsTrue(controller.SubmitAllVoteOnlyCommand.CanExecute());

                var sellOrder2 = new MockOrderCompositePresentationModel() { Shares = 100 };
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(sellOrder2);

                controller.InvokeStartOrder(TransactionType.Sell, "stock01");

                Assert.IsFalse(controller.SubmitAllVoteOnlyCommand.CanExecute());
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void CannotSellMoreSharesThatAreNotOwned()
        {
            try
            {

                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel() { Shares = 1 };
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, new MockStockTraderRICommandProxy(), new MockAccountPositionService());
                controller.InvokeStartOrder(TransactionType.Sell, "NOTOWNED");

                Assert.IsFalse(controller.SubmitAllVoteOnlyCommand.CanExecute());
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void CannotSubmitAllWhenNothingToSubmit()
        {
            var controller = new TestableOrdersController(new MockRegionManager(), new MockStockTraderRICommandProxy(), new MockAccountPositionService());

            Assert.IsFalse(controller.SubmitAllVoteOnlyCommand.CanExecute());
        }

        [TestMethod]
        public void AfterAllOrdersSubmittedSubmitAllCommandShouldBeDisabled()
        {
            try
            {
                Mock<IOrdersView> mockOrdersView = new Mock<IOrdersView>();
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();
                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel();
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersView orderCompositeView = mockOrdersView.Object;
                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(orderCompositeView);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, new MockAccountPositionService());

                var buyOrder = new MockOrderCompositePresentationModel() { Shares = 100 };
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(buyOrder);

                controller.InvokeStartOrder(TransactionType.Buy, "STOCK1");

                bool canExecuteChangedCalled = false;
                bool canExecuteResult = false;

                commandProxy.SubmitAllOrdersCommand.CanExecuteChanged += delegate
                {
                    canExecuteChangedCalled = true;
                    canExecuteResult =
                        controller.SubmitAllVoteOnlyCommand.CanExecute();
                };
                buyOrder.RaiseCloseViewRequested();

                Assert.IsTrue(canExecuteChangedCalled);
                Assert.IsFalse(canExecuteResult);
            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }

        [TestMethod]
        public void ShouldRemoveOrdersViewWhenClosingLastOrder()
        {
            try
            {
                Mock<IOrdersViewModel> mockOrdersViewModel = new Mock<IOrdersViewModel>();

                Mock<ServiceLocatorImplBase> mockServiceLocator = new Mock<ServiceLocatorImplBase>();

                var orderCompositePresenter = new MockOrderCompositePresentationModel() { Shares = 100 };
                var commandProxy = new MockStockTraderRICommandProxy();

                IOrdersViewModel orderCompositePresentationModel = mockOrdersViewModel.Object;

                mockServiceLocator.Setup(x => x.GetInstance<IOrdersView>()).Returns(new Mock<IOrdersView>().Object);
                mockServiceLocator.Setup(x => x.GetInstance<IOrdersViewModel>()).Returns(orderCompositePresentationModel);
                mockServiceLocator.Setup(x => x.GetInstance<IOrderCompositeViewModel>()).Returns(orderCompositePresenter);
                ServiceLocator.SetLocatorProvider(() => mockServiceLocator.Object);

                var controller = new TestableOrdersController(regionManager, commandProxy, new MockAccountPositionService());

                var region = (MockRegion)regionManager.Regions["ActionRegion"];

                controller.InvokeStartOrder(TransactionType.Buy, "STOCK1");

                Assert.AreEqual<int>(1, region.AddedViews.Count);

                orderCompositePresenter.RaiseCloseViewRequested();

                Assert.AreEqual<int>(0, region.AddedViews.Count);

            }
            finally
            {
                ServiceLocator.SetLocatorProvider(() => null);
            }
        }
    }

    internal class TestableOrdersController : OrdersController
    {

        public TestableOrdersController(IRegionManager regionManager, MockStockTraderRICommandProxy commandProxy, IAccountPositionService accountPositionService)
            : base(regionManager, commandProxy, accountPositionService)
        {
        }

        public string StartOrderArgumentTickerSymbol { get; set; }
        public TransactionType StartOrderArgumentTransactionType { get; set; }



        protected override void StartOrder(string tickerSymbol, TransactionType transactionType)
        {
            base.StartOrder(tickerSymbol, transactionType);

            StartOrderArgumentTickerSymbol = tickerSymbol;
            StartOrderArgumentTransactionType = transactionType;
        }

        public void InvokeStartOrder(TransactionType transactionType, string symbol)
        {
            StartOrder(symbol, transactionType);
        }

        public bool SubmitAllCommandCalled = false;

        protected override bool SubmitAllCanExecute()
        {
            SubmitAllCommandCalled = true;
            return base.SubmitAllCanExecute();
        }
    }

    public class MockOrdersViewModel : IOrdersViewModel
    {
        public string HeaderInfo
        {
            get { throw new NotImplementedException(); }
        }
    }

    class MockOrderCompositePresentationModel : IOrderCompositeViewModel
    {
        public MockCommand MockSubmitCommand = new MockCommand();
        public MockCommand MockCancelCommand = new MockCommand();

        public event EventHandler CloseViewRequested;

        public ICommand SubmitCommand
        {
            get { return MockSubmitCommand; }
        }

        public ICommand CancelCommand
        {
            get { return MockCancelCommand; }
        }

        public TransactionInfo TransactionInfo { get; set; }

        public int Shares { get; set; }

        internal void RaiseCloseViewRequested()
        {
            CloseViewRequested(this, EventArgs.Empty);
        }
    }

    internal class MockCommand : ICommand
    {
        public bool ExecuteCalled;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteCalled = true;
        }
    }
}

