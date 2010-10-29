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
using Microsoft.Practices.Prism.ViewModel;
using StockTraderRI.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StockTraderRI.Modules.Watch.Services;
using System.ComponentModel.Composition;
using StockTraderRI.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using StockTraderRI.Modules.Watch.Properties;
using System.Collections.Specialized;
using Microsoft.Practices.Prism.Regions;

namespace StockTraderRI.Modules.Watch.WatchList
{
    [Export(typeof(WatchListViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class WatchListViewModel : NotificationObject
    {
        private readonly IMarketFeedService marketFeedService;
        private readonly IEventAggregator eventAggregator;
        private readonly IRegionManager regionManager;
        private readonly ObservableCollection<string> watchList;
        private ICommand removeWatchCommand;
        private ObservableCollection<WatchItem> watchListItems;
        private WatchItem currentWatchItem;

        [ImportingConstructor]
        public WatchListViewModel(IWatchListService watchListService, IMarketFeedService marketFeedService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.HeaderInfo = Resources.WatchListTitle;
            this.WatchListItems = new ObservableCollection<WatchItem>();

            this.marketFeedService = marketFeedService;
            this.regionManager = regionManager;

            this.watchList = watchListService.RetrieveWatchList();
            this.watchList.CollectionChanged += delegate { this.PopulateWatchItemsList(this.watchList); };
            this.PopulateWatchItemsList(this.watchList);

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<MarketPricesUpdatedEvent>().Subscribe(this.MarketPricesUpdated, ThreadOption.UIThread);

            this.removeWatchCommand = new DelegateCommand<string>(this.RemoveWatch);

            this.watchListItems.CollectionChanged += this.WatchListItems_CollectionChanged;
        }       

        public ObservableCollection<WatchItem> WatchListItems
        {
            get
            {
                return this.watchListItems;
            }

            private set
            {
                if (this.watchListItems != value)
                {
                    this.watchListItems = value;
                    this.RaisePropertyChanged(() => this.WatchListItems);
                }
            }
        }

        public WatchItem CurrentWatchItem
        {
            get
            {
                return this.currentWatchItem;
            }

            set
            {
                if (value != null && this.currentWatchItem != value)
                {
                    this.currentWatchItem = value;
                    this.RaisePropertyChanged(() => CurrentWatchItem);
                    this.eventAggregator.GetEvent<TickerSymbolSelectedEvent>().Publish(this.currentWatchItem.TickerSymbol);
                }
            }
        }

        public string HeaderInfo { get; set; }

        public ICommand RemoveWatchCommand { get { return this.removeWatchCommand; } }

#if SILVERLIGHT
        public void MarketPricesUpdated(IDictionary<string, decimal> updatedPrices)
        {
            foreach (WatchItem watchItem in this.WatchListItems)
            {
                if (updatedPrices.ContainsKey(watchItem.TickerSymbol))
                {
                    watchItem.CurrentPrice = updatedPrices[watchItem.TickerSymbol];
                }
            }
        }
#else
        private void MarketPricesUpdated(IDictionary<string, decimal> updatedPrices)
        {
            foreach (WatchItem watchItem in this.WatchListItems)
            {
                if (updatedPrices.ContainsKey(watchItem.TickerSymbol))
                {
                    watchItem.CurrentPrice = updatedPrices[watchItem.TickerSymbol];
                }
            }
        }
#endif

        private void RemoveWatch(string tickerSymbol)
        {
            this.watchList.Remove(tickerSymbol);
        }

        private void PopulateWatchItemsList(IEnumerable<string> watchItemsList)
        {
            this.WatchListItems.Clear();
            foreach (string tickerSymbol in watchItemsList)
            {
                decimal? currentPrice;
                try
                {
                    currentPrice = this.marketFeedService.GetPrice(tickerSymbol);
                }
                catch (ArgumentException)
                {
                    currentPrice = null;
                }

                this.WatchListItems.Add(new WatchItem(tickerSymbol, currentPrice));
            }
        }

        private void WatchListItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                regionManager.Regions[RegionNames.MainRegion].RequestNavigate("/WatchListView", nr => { });
            }
        }
    }
}
