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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using RegionNavigation.Contacts.Model;
using RegionNavigation.Infrastructure;

namespace RegionNavigation.Contacts.ViewModels
{
    [Export]
    public class ContactsViewModel
    {
        private const string ComposeEmailViewName = "ComposeEmailView";
        private const string ToQueryItemName = "To";

        private readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
        private readonly IRegionManager regionManager;
        private readonly ObservableCollection<Contact> contactsCollection;
        private readonly PagedCollectionView contactsView;
        private readonly DelegateCommand<object> emailContactCommand;

        [ImportingConstructor]
        public ContactsViewModel(IContactsService contactsService, IRegionManager regionManager)
        {
            this.emailContactCommand = new DelegateCommand<object>(this.EmailContact, this.CanEmailContact);

            this.contactsCollection = new ObservableCollection<Contact>();
            this.contactsView = new PagedCollectionView(this.contactsCollection);
            this.contactsView.CurrentChanged += (s, e) => this.emailContactCommand.RaiseCanExecuteChanged();

            this.regionManager = regionManager;

            contactsService.BeginGetContacts((ar) =>
            {
                IEnumerable<Contact> newContacts = contactsService.EndGetContacts(ar);

                this.synchronizationContext.Post((state) =>
                {
                    foreach (var newContact in newContacts)
                    {
                        this.contactsCollection.Add(newContact);
                    }
                }, null);

            }, null);
        }

        public ICollectionView Contacts
        {
            get { return this.contactsView; }
        }

        public ICommand EmailContactCommand
        {
            get { return this.emailContactCommand; }
        }

        private void EmailContact(object ignored)
        {
            var uriQuery = new UriQuery();
            var contact = this.contactsView.CurrentItem as Contact;
            if (contact != null && !string.IsNullOrEmpty(contact.EmailAddress))
            {
                uriQuery.Add(ToQueryItemName, contact.EmailAddress);
            }

            var uri = new Uri(ComposeEmailViewName + uriQuery.ToString(), UriKind.Relative);
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, uri, nr => { });
        }

        private bool CanEmailContact(object ignored)
        {
            return this.contactsView.CurrentItem != null;
        }
    }
}
