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
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.ViewModel;
using StockTraderRI.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Events;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Infrastructure;

namespace StockTraderRI.Modules.Market.TrendLine
{
    [Export(typeof(TrendLineViewModel))]
    public class TrendLineViewModel : NotificationObject
    {
        private readonly IMarketHistoryService marketHistoryService;

        private string tickerSymbol;

        private MarketHistoryCollection historyCollection;

        [ImportingConstructor]
        public TrendLineViewModel(IMarketHistoryService marketHistoryService, IEventAggregator eventAggregator)
        {            
            this.marketHistoryService = marketHistoryService;
            eventAggregator.GetEvent<TickerSymbolSelectedEvent>().Subscribe(this.TickerSymbolChanged);
        }

        public void TickerSymbolChanged(string newTickerSymbol)
        {
            MarketHistoryCollection newHistoryCollection = this.marketHistoryService.GetPriceHistory(newTickerSymbol);

            this.TickerSymbol = newTickerSymbol;
            this.HistoryCollection = newHistoryCollection;
        }

        public string TickerSymbol
        {
            get
            {
                return this.tickerSymbol;
            }
            set
            {
                if (this.tickerSymbol != value)
                {
                    this.tickerSymbol = value;
                    this.RaisePropertyChanged(() => this.TickerSymbol);                    
                }
            }
        }

        public MarketHistoryCollection HistoryCollection
        {
            get
            {
                return historyCollection;
            }
            private set
            {
                if (this.historyCollection != value)
                {
                    this.historyCollection = value;
                    this.RaisePropertyChanged(() => this.HistoryCollection);
                }
            }
        }
    }
}
