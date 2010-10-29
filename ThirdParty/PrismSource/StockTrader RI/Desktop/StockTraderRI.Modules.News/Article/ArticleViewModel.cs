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
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Modules.News.Article
{
    [Export(typeof(ArticleViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ArticleViewModel : NotificationObject
    {
        private string companySymbol;
        private IList<NewsArticle> articles;
        private NewsArticle selectedArticle;
        private readonly INewsFeedService newsFeedService;
        private readonly IRegionManager regionManager;
        private readonly ICommand showArticleListCommand;
        private readonly ICommand showNewsReaderViewCommand;

        [ImportingConstructor]
        public ArticleViewModel(INewsFeedService newsFeedService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            if (newsFeedService == null)
            {
                throw new ArgumentNullException("newsFeedService");
            }

            if (regionManager == null)
            {
                throw new ArgumentNullException("regionManager");
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            this.newsFeedService = newsFeedService;
            this.regionManager = regionManager;

            this.showArticleListCommand = new DelegateCommand(this.ShowArticleList);
            this.showNewsReaderViewCommand = new DelegateCommand(this.ShowNewsReaderView);

            eventAggregator.GetEvent<TickerSymbolSelectedEvent>().Subscribe(OnTickerSymbolSelected, ThreadOption.UIThread);
        }

        public string CompanySymbol
        {
            get
            {
                return this.companySymbol;
            }
            set
            {
                if (this.companySymbol != value)
                {
                    this.companySymbol = value;
                    this.RaisePropertyChanged(() => this.CompanySymbol);
                    this.OnCompanySymbolChanged();
                }
            }
        }

        public NewsArticle SelectedArticle
        {
            get { return this.selectedArticle; }
            set
            {
                if (this.selectedArticle != value)
                {
                    this.selectedArticle = value;
                    this.RaisePropertyChanged(() => this.SelectedArticle);
                }
            }
        }

        public IList<NewsArticle> Articles
        {
            get { return this.articles; }
            private set
            {
                if (this.articles != value)
                {
                    this.articles = value;
                    this.RaisePropertyChanged(() => this.Articles);
                }
            }
        }

        public ICommand ShowNewsReaderCommand { get { return this.showNewsReaderViewCommand; } }

        public ICommand ShowArticleListCommand { get { return this.showArticleListCommand; } }

#if SILVERLIGHT
        public void OnTickerSymbolSelected(string companySymbol)
        {
            this.CompanySymbol = companySymbol;
        }
#else
        private void OnTickerSymbolSelected(string companySymbol)
        {
            this.CompanySymbol = companySymbol;
        }
#endif


        private void OnCompanySymbolChanged()
        {
            this.Articles = newsFeedService.GetNews(companySymbol);
        }

        private void ShowArticleList()
        {
            this.SelectedArticle = null;
        }

        private void ShowNewsReaderView()
        {
            this.regionManager.RequestNavigate(RegionNames.SecondaryRegion, new Uri("/NewsReaderView", UriKind.Relative));
        }
    }
}
