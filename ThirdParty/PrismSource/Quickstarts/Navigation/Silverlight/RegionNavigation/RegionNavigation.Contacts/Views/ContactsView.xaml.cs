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
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using RegionNavigation.Contacts.ViewModels;

namespace RegionNavigation.Contacts.Views
{
    [Export("ContactsView")]
    public partial class ContactsView : UserControl, INavigationAware
    {
        private const string ContactAvatarViewName = "ContactAvatarView";
        private const string ContactDetailViewName = "ContactDetailView";
        private const string ShowParameterName = "Show";
        private const string AvatarsValue = "Avatars";

        public ContactsView()
        {
            InitializeComponent();
        }

        [Import]
        public IRegionManager regionManager;

        [Import]
        public ContactsViewModel ViewModel
        {
            get
            {
                return (ContactsViewModel)this.DataContext;
            }
            set
            {
                this.DataContext = value;
            }
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            // This view should orient its inner region to show either the avatar or contact details view based
            // on the query parameter "Show"

            if (navigationContext.Parameters[ShowParameterName] == AvatarsValue)
            {
                regionManager.RequestNavigate(ContactsRegionNames.ContactDetailsRegion, new Uri(ContactAvatarViewName, UriKind.Relative), nr => { });
            }
            else
            {
                regionManager.RequestNavigate(ContactsRegionNames.ContactDetailsRegion, new Uri(ContactDetailViewName, UriKind.Relative), nr => { });
            }
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            // This view will handle all navigation view requests for ContactsView, so we always return true.
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Intentionally not implemented
        }
    }
}
