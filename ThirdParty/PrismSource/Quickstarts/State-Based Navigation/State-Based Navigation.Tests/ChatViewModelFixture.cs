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
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StateBasedNavigation.Infrastructure;
using StateBasedNavigation.Model;
using StateBasedNavigation.ViewModels;

namespace StateBasedNavigation.Tests
{
    [TestClass]
    public class ChatViewModelFixture
    {
        [TestMethod]
        public void WhenCreated_ThenConnectsServiceAndIsAvailable()
        {
            var serviceMock = new Mock<IChatService>(MockBehavior.Strict);
            serviceMock.SetupSet(svc => svc.Connected = true);
            serviceMock.SetupGet(svc => svc.Connected).Returns(true);
            serviceMock.Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()));

            var viewModel = new ChatViewModel(serviceMock.Object);

            Assert.AreEqual("Available", viewModel.ConnectionStatus);

            serviceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenCreated_ThenGetsContactsFromService()
        {
            Action<IOperationResult<IEnumerable<Contact>>> getContactsCallback = null;
            var serviceMock = new Mock<IChatService>();
            serviceMock
                .SetupGet(svc => svc.Connected)
                .Returns(true);
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()))
                .Callback<Action<IOperationResult<IEnumerable<Contact>>>>(c => getContactsCallback = c);

            var viewModel = new ChatViewModel(serviceMock.Object);

            Assert.IsNotNull(getContactsCallback);
        }

        [TestMethod]
        public void WhenReceivingContacts_ThenUpdatesContactsCollection()
        {
            Action<IOperationResult<IEnumerable<Contact>>> getContactsCallback = null;
            var serviceMock = new Mock<IChatService>();
            serviceMock
                .SetupGet(svc => svc.Connected)
                .Returns(true);
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()))
                .Callback<Action<IOperationResult<IEnumerable<Contact>>>>(c => getContactsCallback = c);

            var contacts = new[] { new Contact(), new Contact() };
            var resultMock = new Mock<IOperationResult<IEnumerable<Contact>>>();
            resultMock.SetupGet(r => r.Result).Returns(contacts);

            var viewModel = new ChatViewModel(serviceMock.Object);

            getContactsCallback(resultMock.Object);

            CollectionAssert.AreEqual(contacts, viewModel.Contacts);
        }

        [TestMethod]
        public void WhenChangingConnectionStatus_ThenUpdatesServiceAndNotifiesPropertyChange()
        {
            var serviceMock = new Mock<IChatService>(MockBehavior.Strict);
            serviceMock
                .SetupSet(svc => svc.Connected = true);
            serviceMock
                .SetupSet(svc => svc.Connected = false)
                .Raises(svc => svc.ConnectionStatusChanged += null, EventArgs.Empty);
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()));
            var viewModel = new ChatViewModel(serviceMock.Object);

            var tracker = new PropertyChangeTracker(viewModel);

            viewModel.ConnectionStatus = "Unavailable";

            CollectionAssert.AreEqual(new[] { "ConnectionStatus" }, tracker.ChangedProperties);

            serviceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenChangingShowDetails_ThenNotifiesPropertyChange()
        {
            var serviceMock = new Mock<IChatService>(MockBehavior.Strict);
            serviceMock
                .SetupSet(svc => svc.Connected = true);
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()));
            var viewModel = new ChatViewModel(serviceMock.Object);

            var tracker = new PropertyChangeTracker(viewModel);

            viewModel.ShowDetails = true;

            CollectionAssert.AreEqual(new[] { "ShowDetails" }, tracker.ChangedProperties);

            serviceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenThereIsNoContactSelected_ThenShowDetailsCommandCannotExecute()
        {
            var serviceMock = new Mock<IChatService>(MockBehavior.Strict);
            serviceMock
                .SetupSet(svc => svc.Connected = true);
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()));
            var viewModel = new ChatViewModel(serviceMock.Object);

            Assert.IsFalse(viewModel.ShowDetailsCommand.CanExecute(null));

            serviceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenContactIsSelected_ThenCommandCanBeExecutedAndNotifiesChange()
        {
            Action<IOperationResult<IEnumerable<Contact>>> getContactsCallback = null;
            var serviceMock = new Mock<IChatService>();
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()))
                .Callback<Action<IOperationResult<IEnumerable<Contact>>>>(c => getContactsCallback = c);

            var contacts = new[] { new Contact(), new Contact() };
            var resultMock = new Mock<IOperationResult<IEnumerable<Contact>>>();
            resultMock.SetupGet(r => r.Result).Returns(contacts);

            var viewModel = new ChatViewModel(serviceMock.Object);

            getContactsCallback(resultMock.Object);

            bool commandChanged = false;
            viewModel.ShowDetailsCommand.CanExecuteChanged += (s, e) => commandChanged = true;

            viewModel.ContactsView.MoveCurrentTo(contacts[0]);

            Assert.IsTrue(viewModel.ShowDetailsCommand.CanExecute(null));
            Assert.IsTrue(commandChanged);

            serviceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenShowDetailsCommandIsExecuted_ThenUpdatesShowingDetails()
        {
            Action<IOperationResult<IEnumerable<Contact>>> getContactsCallback = null;
            var serviceMock = new Mock<IChatService>();
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()))
                .Callback<Action<IOperationResult<IEnumerable<Contact>>>>(c => getContactsCallback = c);

            var contacts = new[] { new Contact(), new Contact() };
            var resultMock = new Mock<IOperationResult<IEnumerable<Contact>>>();
            resultMock.SetupGet(r => r.Result).Returns(contacts);

            var viewModel = new ChatViewModel(serviceMock.Object);

            getContactsCallback(resultMock.Object);

            viewModel.ContactsView.MoveCurrentTo(contacts[0]);

            var tracker = new PropertyChangeTracker(viewModel);

            Assert.IsFalse(viewModel.ShowDetails);

            viewModel.ShowDetailsCommand.Execute(true);

            Assert.IsTrue(viewModel.ShowDetails);
            CollectionAssert.AreEqual(new[] { "ShowDetails" }, tracker.ChangedProperties);
        }

        [TestMethod]
        public void WhenSendingMessage_ThenInitiatesInteraction()
        {
            var serviceMock = new Mock<IChatService>();
            var viewModel = new ChatViewModel(serviceMock.Object);

            bool interactionRaised = false;
            viewModel.SendMessageRequest.Raised += (s, e) => interactionRaised = true;

            viewModel.SendMessage();

            Assert.IsTrue(interactionRaised);
        }

        [TestMethod]
        public void WhenCancellingTheSendMessageInteraction_ThenDoesNothing()
        {
            var serviceMock = new Mock<IChatService>(MockBehavior.Strict);
            serviceMock.SetupSet(svc => svc.Connected = true);
            serviceMock.Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()));

            var viewModel = new ChatViewModel(serviceMock.Object);

            InteractionRequestedEventArgs args = null;
            viewModel.SendMessageRequest.Raised += (s, e) => args = e;

            viewModel.SendMessage();

            Assert.IsNotNull(args);

            var sendMessage = args.Context as SendMessageViewModel;
            sendMessage.Result = false;

            args.Callback();

            serviceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenAcceptingTheSendMessageInteraction_ThenSendsMessage()
        {
            var contact = new Contact();
            var message = "message";
            Action<IOperationResult<IEnumerable<Contact>>> getContactsCallback = null;
            Action<IOperationResult> sendMessageCallback = null;

            var serviceMock = new Mock<IChatService>(MockBehavior.Strict);
            serviceMock
                .SetupSet(svc => svc.Connected = true);
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()))
                .Callback<Action<IOperationResult<IEnumerable<Contact>>>>(cb => getContactsCallback = cb);
            serviceMock
                .Setup(svc => svc.SendMessage(contact, message, It.IsAny<Action<IOperationResult>>()))
                .Callback<Contact, string, Action<IOperationResult>>((c, m, cb) => sendMessageCallback = cb);


            var getContactsOperationMock = new Mock<IOperationResult<IEnumerable<Contact>>>();
            getContactsOperationMock.SetupGet(or => or.Result).Returns(new[] { contact });

            var viewModel = new ChatViewModel(serviceMock.Object);

            InteractionRequestedEventArgs args = null;
            viewModel.SendMessageRequest.Raised += (s, e) => args = e;

            getContactsCallback(getContactsOperationMock.Object);

            viewModel.ContactsView.MoveCurrentTo(contact);

            viewModel.SendMessage();

            Assert.IsNotNull(args);

            var sendMessage = args.Context as SendMessageViewModel;
            sendMessage.Result = true;
            sendMessage.Message = message;

            args.Callback();

            serviceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenSendingMessage_ThenUpdatesAndNotifiesSendingMessage()
        {
            var contact = new Contact();
            var message = "message";
            Action<IOperationResult<IEnumerable<Contact>>> getContactsCallback = null;
            Action<IOperationResult> sendMessageCallback = null;

            var serviceMock = new Mock<IChatService>(MockBehavior.Strict);
            serviceMock
                .SetupSet(svc => svc.Connected = true);
            serviceMock
                .Setup(svc => svc.GetContacts(It.IsAny<Action<IOperationResult<IEnumerable<Contact>>>>()))
                .Callback<Action<IOperationResult<IEnumerable<Contact>>>>(cb => getContactsCallback = cb);
            serviceMock
                .Setup(svc => svc.SendMessage(contact, message, It.IsAny<Action<IOperationResult>>()))
                .Callback<Contact, string, Action<IOperationResult>>((c, m, cb) => sendMessageCallback = cb);


            var getContactsOperationMock = new Mock<IOperationResult<IEnumerable<Contact>>>();
            getContactsOperationMock.SetupGet(or => or.Result).Returns(new[] { contact });

            var viewModel = new ChatViewModel(serviceMock.Object);

            InteractionRequestedEventArgs args = null;
            viewModel.SendMessageRequest.Raised += (s, e) => args = e;

            getContactsCallback(getContactsOperationMock.Object);

            viewModel.ContactsView.MoveCurrentTo(contact);

            var tracker = new PropertyChangeTracker(viewModel);

            Assert.IsFalse(viewModel.SendingMessage);

            viewModel.SendMessage();

            Assert.IsFalse(viewModel.SendingMessage);

            var sendMessage = args.Context as SendMessageViewModel;
            sendMessage.Result = true;
            sendMessage.Message = message;

            args.Callback();

            Assert.IsTrue(viewModel.SendingMessage);
            CollectionAssert.AreEqual(new[] { "SendingMessage" }, tracker.ChangedProperties);

            tracker.Reset();

            sendMessageCallback(new Mock<IOperationResult>().Object);

            Assert.IsFalse(viewModel.SendingMessage);
            CollectionAssert.AreEqual(new[] { "SendingMessage" }, tracker.ChangedProperties);
        }
    }
}
