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
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.Controllers;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    [Export(typeof(IPositionSummaryViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PositionSummaryViewModel : NotificationObject, IPositionSummaryViewModel
    {
        private PositionSummaryItem currentPositionSummaryItem;

        private readonly IEventAggregator eventAggregator;

        public IObservablePosition Position { get; private set; }

        [ImportingConstructor]
        public PositionSummaryViewModel(IOrdersController ordersController, IEventAggregator eventAggregator, IObservablePosition observablePosition)
        {
            this.eventAggregator = eventAggregator;
            this.Position = observablePosition;

            BuyCommand = ordersController.BuyCommand;
            SellCommand = ordersController.SellCommand;

            this.CurrentPositionSummaryItem = new PositionSummaryItem("FAKEINDEX", 0, 0, 0);
        }

        public ICommand BuyCommand { get; private set; }

        public ICommand SellCommand { get; private set; }

        public string HeaderInfo
        {
            get { return "POSITION"; }
        }

        public PositionSummaryItem CurrentPositionSummaryItem
        {
            get { return currentPositionSummaryItem; }
            set
            {
                if (currentPositionSummaryItem != value)
                {
                    currentPositionSummaryItem = value;
                    this.RaisePropertyChanged(() => this.CurrentPositionSummaryItem);
                    if (currentPositionSummaryItem != null)
                    {
                        eventAggregator.GetEvent<TickerSymbolSelectedEvent>().Publish(
                            CurrentPositionSummaryItem.TickerSymbol);
                    }
                }
            }
        }
    }
}
