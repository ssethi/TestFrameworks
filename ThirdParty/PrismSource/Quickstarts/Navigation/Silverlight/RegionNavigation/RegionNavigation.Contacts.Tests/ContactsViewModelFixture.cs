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
using RegionNavigation.Contacts.Model;
using RegionNavigation.Contacts.ViewModels;
using RegionNavigation.Infrastructure;

namespace RegionNavigation.Contacts.Tests
{
    [TestClass]
    public class ContactsViewModelFixture : SilverlightTest
    {
        [TestMethod]
        public void WhenCreated_ThenRequestsContacts()
        {
            var contactsServiceMock = new Mock<IContactsService>();
            var regionManager = new RegionManager();

            var viewModel = new ContactsViewModel(contactsServiceMock.Object, regionManager);

            contactsServiceMock.Verify(svc => svc.BeginGetContacts(It.IsAny<AsyncCallback>(), null));
            Assert.IsTrue(viewModel.Contacts.IsEmpty);
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenReceivesContacts_ThenAddsContactsToTheCollection()
        {
            var contactsServiceMock = new Mock<IContactsService>();
            AsyncCallback callback = null;
            var resultMock = new Mock<IAsyncResult>();
            contactsServiceMock
                .Setup(svc => svc.BeginGetContacts(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((cb, s) => callback = cb)
                .Returns(resultMock.Object);
            var contacts = new[] { new Contact { }, new Contact { } };
            contactsServiceMock
                .Setup(svc => svc.EndGetContacts(resultMock.Object))
                .Returns(contacts);

            var regionManager = new RegionManager();

            var viewModel = new ContactsViewModel(contactsServiceMock.Object, regionManager);

            this.EnqueueConditional(() => callback != null);

            this.EnqueueCallback(() => callback(resultMock.Object));

            this.EnqueueCallback(
                () =>
                {
                    CollectionAssert.AreEqual(contacts, new List<Contact>(viewModel.Contacts.Cast<Contact>()));
                });

            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenContactIsSelected_ThenEmailContactCommandIsEnabledAndNotifiesChange()
        {
            var contactsServiceMock = new Mock<IContactsService>();
            AsyncCallback callback = null;
            var resultMock = new Mock<IAsyncResult>();
            contactsServiceMock
                .Setup(svc => svc.BeginGetContacts(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((cb, s) => callback = cb)
                .Returns(resultMock.Object);
            var contacts = new[] { new Contact { }, new Contact { } };
            contactsServiceMock
                .Setup(svc => svc.EndGetContacts(resultMock.Object))
                .Returns(contacts);

            var regionManager = new RegionManager();

            var viewModel = new ContactsViewModel(contactsServiceMock.Object, regionManager);

            this.EnqueueConditional(() => callback != null);

            this.EnqueueCallback(() => callback(resultMock.Object));

            this.EnqueueCallback(
                () =>
                {
                    var notified = false;
                    viewModel.EmailContactCommand.CanExecuteChanged += (s, o) => notified = true;
                    Assert.IsFalse(viewModel.EmailContactCommand.CanExecute(null));
                    viewModel.Contacts.MoveCurrentToFirst();
                    Assert.IsTrue(viewModel.EmailContactCommand.CanExecute(null));
                    Assert.IsTrue(notified);
                });

            this.EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenSendingEmail_ThenNavigatesWithAToQueryParameter()
        {
            var contactsServiceMock = new Mock<IContactsService>();
            AsyncCallback callback = null;
            var resultMock = new Mock<IAsyncResult>();
            contactsServiceMock
                .Setup(svc => svc.BeginGetContacts(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((cb, s) => callback = cb)
                .Returns(resultMock.Object);
            var contacts = new[] { new Contact { EmailAddress = "email" }, new Contact { } };
            contactsServiceMock
                .Setup(svc => svc.EndGetContacts(resultMock.Object))
                .Returns(contacts);

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri("ComposeEmailView?To=email", UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            IRegionManager regionManager = regionManagerMock.Object;

            var viewModel = new ContactsViewModel(contactsServiceMock.Object, regionManager);

            this.EnqueueConditional(() => callback != null);

            this.EnqueueCallback(() => callback(resultMock.Object));

            this.EnqueueCallback(
                () =>
                {
                    viewModel.Contacts.MoveCurrentToFirst();

                    viewModel.EmailContactCommand.Execute(null);

                    regionMock.VerifyAll();
                });

            this.EnqueueTestComplete();
        }
    }
}
