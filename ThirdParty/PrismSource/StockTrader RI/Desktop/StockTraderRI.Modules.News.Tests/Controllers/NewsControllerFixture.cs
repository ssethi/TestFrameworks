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
using System.ComponentModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Regions;

namespace StockTraderRI.Modules.News.Tests.Controllers
{
    [TestClass]
    public class NewsControllerFixture
    {
        [TestMethod]
        public void WhenArticleViewModelSelectedArticleChanged_NewsReaderViewModelNewsArticleUpdated()
        {
            // Prepare
            INewsFeedService newsFeedService = new Mock<INewsFeedService>().Object;
            IRegionManager regionManager = new Mock<IRegionManager>().Object;

            var tickerSymbolSelectedEvent = new Mock<TickerSymbolSelectedEvent>().Object;

            var mockEventAggregator = new Mock<IEventAggregator>();
            mockEventAggregator.Setup(x => x.GetEvent<TickerSymbolSelectedEvent>()).Returns(tickerSymbolSelectedEvent);
            IEventAggregator eventAggregator = mockEventAggregator.Object;


            ArticleViewModel articleViewModel = new ArticleViewModel(newsFeedService, regionManager, eventAggregator);
            NewsReaderViewModel newsReaderViewModel = new NewsReaderViewModel();

            var controller = new NewsController(articleViewModel, newsReaderViewModel);

            NewsArticle newsArticle = new NewsArticle() { Title = "SomeTitle", Body = "Newsbody" };

            // Act
            articleViewModel.SelectedArticle = newsArticle;

            // Verify
            Assert.AreSame(newsArticle, newsReaderViewModel.NewsArticle);
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
}
