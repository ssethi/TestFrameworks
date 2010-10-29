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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Infrastructure.Models;
using System.ComponentModel;

namespace StockTraderRI.Modules.News.Tests.Article
{
    [TestClass]
    public class NewsReaderViewModelFixture
    {
        [TestMethod]
        public void SetNewsArticleUpdatesProperty()
        {
            var target = new NewsReaderViewModel();

            bool propertyChangedRaised = false;
            target.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "NewsArticle")
                {
                    propertyChangedRaised = true;
                }
            };

            NewsArticle article = new NewsArticle() { Title = "My Title", Body = "My Body" };
            target.NewsArticle = article;

            Assert.AreSame(article, target.NewsArticle);
            Assert.IsTrue(propertyChangedRaised);
        }
    }
}
