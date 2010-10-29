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
using StockTraderRI.Modules.Watch.Services;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel.Composition;

namespace StockTraderRI.Modules.Watch.AddWatch
{
    [Export(typeof(AddWatchViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AddWatchViewModel : NotificationObject
    {
        private string stockSymbol;
        private IWatchListService watchListService;

        [ImportingConstructor]
        public AddWatchViewModel(IWatchListService watchListService)
        {
            if (watchListService == null)
            {
                throw new ArgumentNullException("service");
            }
            this.watchListService = watchListService;
        }

        public string StockSymbol
        {
            get { return stockSymbol; }
            set
            {
                stockSymbol = value;
                this.RaisePropertyChanged(() => StockSymbol);
            }
        }

        public ICommand AddWatchCommand { get { return this.watchListService.AddWatchCommand; } }
    }
}
