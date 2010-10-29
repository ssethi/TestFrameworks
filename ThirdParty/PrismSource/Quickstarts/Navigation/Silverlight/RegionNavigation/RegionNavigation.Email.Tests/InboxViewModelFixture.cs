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
using Microsoft.Practices.Prism.Regions;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionNavigation.Email.Model;
using RegionNavigation.Email.ViewModels;
using RegionNavigation.Infrastructure;

namespace RegionNavigation.Email.Tests
{
    [TestClass]
    public class InboxViewModelFixture : SilverlightTest
    {
        [TestMethod]
        public void WhenCreated_ThenRequestsEmailsToService()
        {
            var emailServiceMock = new Mock<IEmailService>();
            var requested = false;
            emailServiceMock
                .Setup(svc => svc.BeginGetEmailDocuments(It.IsAny<AsyncCallback>(), null))
                .Callback(() => requested = true);

            var viewModel = new InboxViewModel(emailServiceMock.Object, new Mock<IRegionManager>().Object);

            Assert.IsTrue(requested);
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenEmailsAreReturned_ThenViewModelIsPopulated()
        {
            var asyncResultMock = new Mock<IAsyncResult>();

            var emailServiceMock = new Mock<IEmailService>(MockBehavior.Strict);
            AsyncCallback callback = null;
            emailServiceMock
                .Setup(svc => svc.BeginGetEmailDocuments(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((ac, o) => callback = ac)
                .Returns(asyncResultMock.Object);
            var email = new EmailDocument { };
            emailServiceMock
                .Setup(svc => svc.EndGetEmailDocuments(asyncResultMock.Object))
                .Returns(new[] { email });


            var viewModel = new InboxViewModel(emailServiceMock.Object, new Mock<IRegionManager>().Object);

            this.EnqueueConditional(() => callback != null);

            this.EnqueueCallback(
                () =>
                {
                    callback(asyncResultMock.Object);
                });

            this.EnqueueCallback(
                () =>
                {
                    CollectionAssert.AreEqual(new[] { email }, viewModel.Messages.Cast<EmailDocument>().ToArray());
                    emailServiceMock.VerifyAll();
                });

            this.EnqueueTestComplete();
        }

        [TestMethod]
        public void WhenEmailIsNotSelected_ThenCannotExecuteTheReplyCommand()
        {
        }

        [TestMethod]
        public void WhenEmailIsSelected_ThenCanExecuteTheReplyCommand()
        {
        }

        [TestMethod]
        public void WhenExecutingTheNewCommand_ThenNavigatesToComposeEmailView()
        {
            var emailServiceMock = new Mock<IEmailService>();


            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri("ComposeEmailView", UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            var viewModel = new InboxViewModel(emailServiceMock.Object, regionManagerMock.Object);

            viewModel.ComposeMessageCommand.Execute(null);

            regionMock.VerifyAll();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenExecutingTheReplyCommand_ThenNavigatesToComposeEmailViewWithId()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();
            AsyncCallback callback = null;
            var asyncResultMock = new Mock<IAsyncResult>();
            emailServiceMock
                .Setup(svc => svc.BeginGetEmailDocuments(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((ac, o) => callback = ac)
                .Returns(asyncResultMock.Object);
            emailServiceMock
                .Setup(svc => svc.EndGetEmailDocuments(asyncResultMock.Object))
                .Returns(new[] { email });

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri(@"ComposeEmailView?ReplyTo=" + email.Id.ToString("N"), UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            var viewModel = new InboxViewModel(emailServiceMock.Object, regionManagerMock.Object);

            EnqueueConditional(() => callback != null);

            EnqueueCallback(
                () =>
                {
                    callback(asyncResultMock.Object);
                });

            EnqueueConditional(
                () =>
                {
                    return !viewModel.Messages.IsEmpty;
                });

            EnqueueCallback(
                () =>
                {
                    viewModel.Messages.MoveCurrentToFirst();

                    viewModel.ReplyMessageCommand.Execute(null);

                    regionMock.VerifyAll();
                });

            EnqueueTestComplete();
        }


        [TestMethod]
        public void WhenExecutingTheOpenCommand_ThenNavigatesToEmailView()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri(@"EmailView?EmailId=" + email.Id.ToString("N"), UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            var viewModel = new InboxViewModel(emailServiceMock.Object, regionManagerMock.Object);

            viewModel.OpenMessageCommand.Execute(email);

            regionMock.VerifyAll();
        }


        public static Func<bool> SetupGetEmails(Mock<IEmailService> serviceMock, IEnumerable<EmailDocument> result)
        {
            var asyncResultMock = new Mock<IAsyncResult>();
            AsyncCallback callback = null;
            serviceMock
                .Setup(svc => svc.BeginGetEmailDocuments(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((ac, o) => callback = ac)
                .Returns(asyncResultMock.Object);
            var email = new EmailDocument { };
            serviceMock
                .Setup(svc => svc.EndGetEmailDocuments(asyncResultMock.Object))
                .Returns(result);

            return () =>
                {
                    if (callback == null)
                    {
                        return false;
                    }

                    callback(asyncResultMock.Object);
                    return true;
                };
        }
    }
}
