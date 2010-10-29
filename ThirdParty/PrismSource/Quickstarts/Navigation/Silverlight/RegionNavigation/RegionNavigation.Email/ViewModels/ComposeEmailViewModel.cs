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
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.Prism.ViewModel;
using RegionNavigation.Email.Model;

namespace RegionNavigation.Email.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RegionMemberLifetime(KeepAlive = false)]
    public class ComposeEmailViewModel : NotificationObject, IConfirmNavigationRequest
    {
        private const string NormalStateKey = "Normal";
        private const string SendingStateKey = "Sending";
        private const string SentStateKey = "Sent";
        
        private const string ReplyToParameterKey = "ReplyTo";
        private const string ToParameterKey = "To";

        private readonly SynchronizationContext synchronizationContext;
        private readonly IEmailService emailService;
        private readonly DelegateCommand sendEmailCommand;
        private readonly DelegateCommand cancelEmailCommand;
        private readonly InteractionRequest<Confirmation> confirmExitInteractionRequest;
        private EmailDocument emailDocument;
        private string sendState;
        private IRegionNavigationJournal navigationJournal;

        [ImportingConstructor]
        public ComposeEmailViewModel(IEmailService emailService)
        {
            this.synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
            this.sendEmailCommand = new DelegateCommand(this.SendEmail);
            this.cancelEmailCommand = new DelegateCommand(this.CancelEmail);
            this.confirmExitInteractionRequest = new InteractionRequest<Confirmation>();
            this.sendState = NormalStateKey;

            this.emailService = emailService;
        }

        public ICommand SendEmailCommand
        {
            get { return this.sendEmailCommand; }
        }

        public ICommand CancelEmailCommand
        {
            get { return this.cancelEmailCommand; }
        }

        public IInteractionRequest ConfirmExitInteractionRequest
        {
            get { return this.confirmExitInteractionRequest; }
        }

        public EmailDocument EmailDocument
        {
            get
            {
                return this.emailDocument;
            }

            set
            {
                if (this.emailDocument != value)
                {
                    this.emailDocument = value;
                    this.RaisePropertyChanged(() => this.EmailDocument);
                }
            }
        }

        public string SendState
        {
            get
            {
                return this.sendState;
            }

            private set
            {
                if (this.sendState != value)
                {
                    this.sendState = value;
                    this.RaisePropertyChanged(() => this.SendState);
                }
            }
        }

        private void SendEmail()
        {
            this.SendState = SendingStateKey;
            this.emailService.BeginSendEmailDocument(
                this.emailDocument,
                r => this.synchronizationContext.Post(
                    s =>
                        {
                            this.SendState = SentStateKey;

                            // todo: 05 - Send Email: Navigating back
                            // After the email has been 'sent' (we're using a mock service in this application), the
                            // view model uses the navigation journal it captured when it
                            // was navigated to (see the OnNavigatedTo in this class) to
                            // navigate the region to the prior view.  
                            // 
                            if (this.navigationJournal != null)
                            {
                                this.navigationJournal.GoBack();
                            }
                        },
                    null),
                null);

        }

        private void CancelEmail()
        {
            // todo: 06 - Cancel Email : Navigating backwards
            // When the user elects to cancel the email, we navigate the region backwards.
            //
            // Because the view model implements the IConfirmNavigationRequest
            // it has the option to interrupt the navigation for the region if there
            // changes to the email.  See the ConfirmNavigationRequest implementation below
            // for more details on this.
            // 
            if (this.navigationJournal != null)
            {
                this.navigationJournal.GoBack();
            }
        }

        void IConfirmNavigationRequest.ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            // todo: 07 - Confirming Navigation Requests
            // There are times when a view (or view model) wish to be able to cancel a navigation request.
            // For this email, the user may have started but not sent an email.  We want to confirm with
            // the user that they want to discard their changes before completing the navigation.
            //
            // The view model uses the InteractionRequest to prompt the user (this is explained in more
            // detail in Prism's MVVM guidance) and, if the user confirms they want to navigate away,
            // then continues the navigation by using the continuationCallback passed in as a parameter.
            //
            // NOTE: You MUST invoke the continuationCallback action or you will halt this current
            // navigation request and no further processing of this request willl take place.  
            //            
            if (this.sendState == NormalStateKey)
            {
                this.confirmExitInteractionRequest.Raise(
                    new Confirmation { Content = Resources.ConfirmNavigateAwayFromEmailMessage, Title = Resources.ConfirmNavigateAwayFromEmailTitle },
                    c => { continuationCallback(c.Confirmed); });
            }
            else
            {
                continuationCallback(true);
            }
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            // We always want a new instance of a composed email, so we should return false to indicate
            // this doesn't handle the navigation request.
            return false;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Intentionally not implemented
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            // todo: 08 - Email OnNavigatedTo : Accessing navigation context.
            // When this view model is navigated to it gains access to the
            // NavigationContext to determine if we are composing a new email
            // or replying to an existing one.
            //
            // The navigation context offers the context information through
            // the Parameters property that is a string/value dictionairy
            // built from the navigation Uri.
            //
            // In this example, we look for the 'ReplyTo' value to 
            // determine if we are replying to an email and, if so, 
            // retrieving it's relevant information from the email service
            // to pre-populate response values.
            //
            var emailDocument = new EmailDocument();

            var parameters = navigationContext.Parameters;

            var replyTo = parameters[ReplyToParameterKey];
            Guid replyToId;
            if (replyTo != null && Guid.TryParse(replyTo, out replyToId))
            {
                var replyToEmail = this.emailService.GetEmailDocument(replyToId);
                if (replyToEmail != null)
                {
                    emailDocument.To = replyToEmail.From;
                    emailDocument.Subject = Resources.ResponseMessagePrefix + replyToEmail.Subject;

                    emailDocument.Text =
                        Environment.NewLine +
                        replyToEmail.Text
                            .Split(Environment.NewLine.ToCharArray())
                            .Select(l => l.Length > 0 ? Resources.ResponseLinePrefix + l : l)
                            .Aggregate((l1, l2) => l1 + Environment.NewLine + l2);
                }
            }
            else
            {
                var to = parameters[ToParameterKey];
                if (to != null)
                {
                    emailDocument.To = to;
                }
            }

            this.EmailDocument = emailDocument;

            // todo: 09 - Email OnNaviatedTo : capture the navigation service journal
            // You can capture the navigation service or navigation service journal
            // to navigate the region you're placed in without having to expressly 
            // know which region to navigate.
            this.navigationJournal = navigationContext.NavigationService.Journal;
        }
    }
}
