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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using StockTraderRI.Infrastructure.Interfaces;
using System.ComponentModel.Composition;

namespace StockTraderRI.Modules.Watch.Services
{
    [Export(typeof(IWatchListService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class WatchListService : IWatchListService
    {
        private readonly IMarketFeedService marketFeedService;

        private ObservableCollection<string> WatchItems { get; set; }

        [ImportingConstructor]
        public WatchListService(IMarketFeedService marketFeedService)
        {
            this.marketFeedService = marketFeedService;
            WatchItems = new ObservableCollection<string>();

            AddWatchCommand = new DelegateCommand<string>(AddWatch);
        }

        public ObservableCollection<string> RetrieveWatchList()
        {
            return WatchItems;
        }

        private void AddWatch(string tickerSymbol)
        {
            if (!String.IsNullOrEmpty(tickerSymbol))
            {
                string upperCasedTrimmedSymbol = tickerSymbol.ToUpper(CultureInfo.InvariantCulture).Trim();
                if (!WatchItems.Contains(upperCasedTrimmedSymbol))
                {
                    if (marketFeedService.SymbolExists(upperCasedTrimmedSymbol))
                    {
                        WatchItems.Add(upperCasedTrimmedSymbol);
                    }
                }
            }
        }

        public ICommand AddWatchCommand { get; set; }
    }
}
