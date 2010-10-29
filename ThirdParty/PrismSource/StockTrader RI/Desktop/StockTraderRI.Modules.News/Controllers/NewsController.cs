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
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;
using System.ComponentModel.Composition;
using System.ComponentModel;

namespace StockTraderRI.Modules.News.Controllers
{
    [Export(typeof(INewsController))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NewsController : INewsController
    {
        private readonly ArticleViewModel articleViewModel;
        private readonly NewsReaderViewModel newsReaderViewModel;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "newsReader")]
        [ImportingConstructor]
        public NewsController(ArticleViewModel articleViewModel, NewsReaderViewModel newsReaderViewModel)
        {            
            this.articleViewModel = articleViewModel;         
            this.newsReaderViewModel = newsReaderViewModel;
            this.articleViewModel.PropertyChanged += this.ArticleViewModel_PropertyChanged;
        }

        private void ArticleViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedArticle":
                    this.newsReaderViewModel.NewsArticle = this.articleViewModel.SelectedArticle;
                    break;
            }
        }
    }
}
