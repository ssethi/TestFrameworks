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
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionNavigation.Email.Model;
using RegionNavigation.Email.ViewModels;

namespace RegionNavigation.Email.Tests
{
    [TestClass]
    public class ComposeEmailViewModelFixture : SilverlightTest
    {
        [TestMethod]
        public void WhenSendMessageCommandIsExecuted_ThenSendsMessageThroughService()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);
            ((INavigationAware)viewModel).OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("", UriKind.Relative)));

            viewModel.SendEmailCommand.Execute(null);

            Assert.AreEqual("Sending", viewModel.SendState);

            emailServiceMock.Verify(svc => svc.BeginSendEmailDocument(viewModel.EmailDocument, It.IsAny<AsyncCallback>(), null));
        }

        [TestMethod]
        public void WhenNavigatedToWithAReplyToQueryParameter_ThenRepliesToTheAppropriateMessage()
        {
            var replyToEmail = new EmailDocument { From = "somebody@contoso.com", To = "", Subject = "", Text = "" };

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock
                .Setup(svc => svc.GetEmailDocument(replyToEmail.Id))
                .Returns(replyToEmail);

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);
            var uriQuery = new UriQuery();
            uriQuery.Add("ReplyTo", replyToEmail.Id.ToString("N"));
            ((INavigationAware)viewModel).OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("" + uriQuery.ToString(), UriKind.Relative)));

            Assert.AreEqual("somebody@contoso.com", viewModel.EmailDocument.To);
        }

        [TestMethod]
        public void WhenNavigatedToWithAToQueryParameter_ThenInitializesToField()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);
            var uriQuery = new UriQuery();
            uriQuery.Add("To", "somebody@contoso.com");
            ((INavigationAware)viewModel).OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("" + uriQuery.ToString(), UriKind.Relative)));

            Assert.AreEqual("somebody@contoso.com", viewModel.EmailDocument.To);
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenFinishedSendingMessage_ThenNavigatesBack()
        {
            var sendEmailResultMock = new Mock<IAsyncResult>();

            var emailServiceMock = new Mock<IEmailService>();
            AsyncCallback callback = null;
            emailServiceMock
                .Setup(svc => svc.BeginSendEmailDocument(It.IsAny<EmailDocument>(), It.IsAny<AsyncCallback>(), null))
                .Callback<EmailDocument, AsyncCallback, object>((e, c, o) => { callback = c; })
                .Returns(sendEmailResultMock.Object);
            emailServiceMock
                .Setup(svc => svc.EndSendEmailDocument(sendEmailResultMock.Object))
                .Verifiable();

            var journalMock = new Mock<IRegionNavigationJournal>();
            journalMock.Setup(j => j.GoBack()).Verifiable();

            var navigationServiceMock = new Mock<IRegionNavigationService>();
            navigationServiceMock.SetupGet(svc => svc.Journal).Returns(journalMock.Object);

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);
            ((INavigationAware)viewModel).OnNavigatedTo(new NavigationContext(navigationServiceMock.Object, new Uri("", UriKind.Relative)));

            viewModel.SendEmailCommand.Execute(null);

            this.EnqueueConditional(() => callback != null);

            this.EnqueueCallback(
                () =>
                {
                    callback(sendEmailResultMock.Object);
                });

            this.EnqueueCallback(
                () =>
                {
                    Assert.AreEqual("Sent", viewModel.SendState);
                    journalMock.VerifyAll();
                });

            this.EnqueueTestComplete();
        }

        [TestMethod]
        public void WhenRequestedForVetoOnNavigationBeforeSubmitting_ThenRaisesInteractionRequest()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);
            var uriQuery = new UriQuery();
            uriQuery.Add("To", "somebody@contoso.com");
            ((INavigationAware)viewModel).OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("" + uriQuery.ToString(), UriKind.Relative)));

            InteractionRequestedEventArgs args = null;
            viewModel.ConfirmExitInteractionRequest.Raised += (s, e) => args = e;

            bool? callbackResult = null;
            ((IConfirmNavigationRequest)viewModel).ConfirmNavigationRequest(
                new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("some uri", UriKind.Relative)),
                b => callbackResult = b);

            Assert.IsNotNull(args);
            Assert.IsNull(callbackResult);
        }

        [TestMethod]
        public void WhenRequestedForVetoOnNavigationAfterSubmitting_ThenDoesNotRaiseInteractionRequest()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);
            var uriQuery = new UriQuery();
            uriQuery.Add("To", "somebody@contoso.com");
            ((INavigationAware)viewModel).OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("" + uriQuery.ToString(), UriKind.Relative)));

            InteractionRequestedEventArgs args = null;
            viewModel.ConfirmExitInteractionRequest.Raised += (s, e) => args = e;

            viewModel.SendEmailCommand.Execute(null);

            bool? callbackResult = null;
            ((IConfirmNavigationRequest)viewModel).ConfirmNavigationRequest(
                new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("some uri", UriKind.Relative)),
                b => callbackResult = b);

            Assert.IsNull(args);
            Assert.IsTrue(callbackResult.Value);
        }

        [TestMethod]
        public void WhenRejectingConfirmationToNavigateAway_ThenInvokesRequestCallbackWithFalse()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);
            var uriQuery = new UriQuery();
            uriQuery.Add("To", "somebody@contoso.com");
            ((INavigationAware)viewModel).OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("" + uriQuery.ToString(), UriKind.Relative)));

            InteractionRequestedEventArgs args = null;
            viewModel.ConfirmExitInteractionRequest.Raised += (s, e) => args = e;

            bool? callbackResult = null;
            ((IConfirmNavigationRequest)viewModel).ConfirmNavigationRequest(
                new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("some uri", UriKind.Relative)),
                b => callbackResult = b);

            var confirmation = args.Context as Confirmation;

            confirmation.Confirmed = false;

            args.Callback();

            Assert.IsFalse(callbackResult.Value);
        }

        [TestMethod]
        public void WhenAcceptingConfirmationToNavigateAway_ThenInvokesRequestCallbackWithTrue()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);
            var uriQuery = new UriQuery();
            uriQuery.Add("To", "somebody@contoso.com");
            ((INavigationAware)viewModel).OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("" + uriQuery.ToString(), UriKind.Relative)));

            InteractionRequestedEventArgs args = null;
            viewModel.ConfirmExitInteractionRequest.Raised += (s, e) => args = e;

            bool? callbackResult = null;
            ((IConfirmNavigationRequest)viewModel).ConfirmNavigationRequest(
                new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("some uri", UriKind.Relative)),
                b => callbackResult = b);

            var confirmation = args.Context as Confirmation;

            confirmation.Confirmed = true;

            args.Callback();

            Assert.IsTrue(callbackResult.Value);
        }

        [TestMethod]
        public void WhenCancelling_ThenNavigatesBack()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object);

            var journalMock = new Mock<IRegionNavigationJournal>();

            var navigationServiceMock = new Mock<IRegionNavigationService>();
            navigationServiceMock.SetupGet(svc => svc.Journal).Returns(journalMock.Object);

            ((INavigationAware)viewModel).OnNavigatedTo(
                new NavigationContext(
                    navigationServiceMock.Object,
                    new Uri("location", UriKind.Relative)));

            viewModel.CancelEmailCommand.Execute(null);

            journalMock.Verify(j => j.GoBack());
        }
    }
}
