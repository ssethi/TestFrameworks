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
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Modules.News.Article
{
    [Export(typeof(NewsReaderViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NewsReaderViewModel : NotificationObject
    {
        private NewsArticle newsArticle;

        public NewsArticle NewsArticle        
        {
            get
            {
                return this.newsArticle;
            }
            set
            {
                if (this.newsArticle != value)
                {
                    this.newsArticle = value;
                    this.RaisePropertyChanged(() => this.NewsArticle);     
                }
            }
        }

    }
}
