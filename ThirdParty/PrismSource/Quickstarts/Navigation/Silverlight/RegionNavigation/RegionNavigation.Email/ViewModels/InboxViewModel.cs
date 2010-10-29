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
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using RegionNavigation.Email.Model;
using RegionNavigation.Infrastructure;

namespace RegionNavigation.Email.ViewModels
{
    [Export]
    public class InboxViewModel : NotificationObject
    {
        private const string ComposeEmailViewKey = "ComposeEmailView";
        private const string ReplyToKey = "ReplyTo";
        private const string EmailViewKey = "EmailView";
        private const string EmailIdKey = "EmailId";

        private readonly SynchronizationContext synchronizationContext;
        private readonly IEmailService emailService;
        private readonly IRegionManager regionManager;
        private readonly DelegateCommand<object> composeMessageCommand;
        private readonly DelegateCommand<object> replyMessageCommand;
        private readonly DelegateCommand<EmailDocument> openMessageCommand;
        private readonly ObservableCollection<EmailDocument> messagesCollection;

        private static Uri ComposeEmailViewUri = new Uri(ComposeEmailViewKey, UriKind.Relative);

        [ImportingConstructor]
        public InboxViewModel(IEmailService emailService, IRegionManager regionManager)
        {
            this.synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();

            this.composeMessageCommand = new DelegateCommand<object>(this.ComposeMessage);
            this.replyMessageCommand = new DelegateCommand<object>(this.ReplyMessage, this.CanReplyMessage);
            this.openMessageCommand = new DelegateCommand<EmailDocument>(this.OpenMessage);

            this.messagesCollection = new ObservableCollection<EmailDocument>();
            this.Messages = new PagedCollectionView(this.messagesCollection);
            this.Messages.CurrentChanged += (s, e) =>
                this.replyMessageCommand.RaiseCanExecuteChanged();

            this.emailService = emailService;
            this.regionManager = regionManager;

            this.emailService.BeginGetEmailDocuments(
                r =>
                {
                    var messages = this.emailService.EndGetEmailDocuments(r);

                    this.synchronizationContext.Post(
                        s =>
                        {
                            foreach (var message in messages)
                            {
                                this.messagesCollection.Add(message);
                            }
                        },
                        null);
                },
                null);
        }

        public ICollectionView Messages { get; private set; }

        public ICommand ComposeMessageCommand
        {
            get { return this.composeMessageCommand; }
        }

        public ICommand ReplyMessageCommand
        {
            get { return this.replyMessageCommand; }
        }

        public ICommand OpenMessageCommand
        {
            get { return this.openMessageCommand; }
        }

        private void ComposeMessage(object ignored)
        {
            // todo: 02 - New Email: Navigating to a view in a region
            // Any region can be navigated by passing in a relative Uri for the email view name.
            // By the default, the navigation service will look for an item already in the region
            // with a type name that matches the Uri.
            //
            // If an item is not found in the region, the navigation services uses the 
            // ServiceLocator to find an item that matches the Uri, in the example below it would
            // be ComposeEmailView.
            //
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, ComposeEmailViewUri, nr => { });
        }

        private void ReplyMessage(object ignored)
        {
            // todo: 03 - Reply Email: Navigating to a view in a region with context
            // When navigating, you can also supply context so the target view or
            // viewmodel can orient their data to something appropriate.  In this case,
            // we've chosen to pass the email id in a name/value pair as part of the Uri.
            //
            // The recipient of the context can get access to this information by
            // implementing the INavigationAware or IConfirmNavigationRequest interface, as shown by the 
            // the ComposeEmailViewModel.
            //
            var currentEmail = this.Messages.CurrentItem as EmailDocument;
            var builder = new StringBuilder();
            builder.Append(ComposeEmailViewKey);
            if (currentEmail != null)
            {
                var query = new UriQuery();
                query.Add(ReplyToKey, currentEmail.Id.ToString("N"));
                builder.Append(query);
            }
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, new Uri(builder.ToString(), UriKind.Relative), nr => { });
        }

        private bool CanReplyMessage(object ignored)
        {
            return this.Messages.CurrentItem != null;
        }

        private void OpenMessage(EmailDocument document)
        {
            // todo: 04 - Open Email: Navigating to a view in a region with context
            // When navigating, you can also supply context so the target view or
            // viewmodel can orient their data to something appropriate.  In this case,
            // we've chosen to pass the email id in a name/value pair as part of the Uri.
            //
            // The EmailViewModel retrieves this context by implementing the INavigationAware
            // interface.
            //
            var builder = new StringBuilder();
            builder.Append(EmailViewKey);
            var query = new UriQuery();
            query.Add(EmailIdKey, document.Id.ToString("N"));
            builder.Append(query);
            this.regionManager.RequestNavigate(RegionNames.MainContentRegion, new Uri(builder.ToString(), UriKind.Relative), nr => { });
        }
    }
}
