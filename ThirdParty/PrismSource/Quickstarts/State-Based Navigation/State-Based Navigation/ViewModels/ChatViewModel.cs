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
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using StateBasedNavigation.Infrastructure;
using StateBasedNavigation.Model;

namespace StateBasedNavigation.ViewModels
{
    public class ChatViewModel : ViewModel
    {
        private readonly IChatService chatService;
        private readonly InteractionRequest<SendMessageViewModel> sendMessageRequest;
        private readonly InteractionRequest<ReceivedMessage> showReceivedMessageRequest;
        private readonly ObservableCollection<Contact> contacts;
        private readonly PagedCollectionView contactsView;
        private readonly ShowDetailsCommandImplementation showDetailsCommand;
        private bool showDetails;
        private bool sendingMessage;

        public ChatViewModel(IChatService chatService)
        {
            this.contacts = new ObservableCollection<Contact>();
            this.contactsView = new PagedCollectionView(this.contacts);
            this.sendMessageRequest = new InteractionRequest<SendMessageViewModel>();
            this.showReceivedMessageRequest = new InteractionRequest<ReceivedMessage>();
            this.showDetailsCommand = new ShowDetailsCommandImplementation(this);

            this.contactsView.CurrentChanged += this.OnCurrentContactChanged;

            this.chatService = chatService;
            this.chatService.Connected = true;
            this.chatService.ConnectionStatusChanged += (s, e) => this.RaisePropertyChanged(() => this.ConnectionStatus);
            this.chatService.MessageReceived += this.OnMessageReceived;

            this.chatService.GetContacts(
                result =>
                {
                    if (result.Error == null)
                    {
                        foreach (var item in result.Result)
                        {
                            this.contacts.Add(item);
                        }
                    }
                });
        }

        public ObservableCollection<Contact> Contacts
        {
            get { return this.contacts; }
        }

        public ICollectionView ContactsView
        {
            get { return this.contactsView; }
        }

        public IInteractionRequest SendMessageRequest
        {
            get { return this.sendMessageRequest; }
        }

        public IInteractionRequest ShowReceivedMessageRequest
        {
            get { return this.showReceivedMessageRequest; }
        }

        public string ConnectionStatus
        {
            get
            {
                return this.chatService.Connected ? "Available" : "Unavailable";
            }

            set
            {
                this.chatService.Connected = value == "Available";
            }
        }

        public Contact CurrentContact
        {
            get
            {
                return this.contactsView.CurrentItem as Contact;
            }
        }

        public bool ShowDetails
        {
            get
            {
                return this.showDetails;
            }

            set
            {
                if (this.showDetails != value)
                {
                    this.showDetails = value;
                    this.RaisePropertyChanged(() => this.ShowDetails);
                }
            }
        }

        public bool SendingMessage
        {
            get
            {
                return this.sendingMessage;
            }

            private set
            {
                if (this.sendingMessage != value)
                {
                    this.sendingMessage = value;
                    this.RaisePropertyChanged(() => this.SendingMessage);
                }
            }
        }

        public ICommand ShowDetailsCommand
        {
            get { return this.showDetailsCommand; }
        }

        public void SendMessage()
        {
            var contact = this.CurrentContact;
            this.sendMessageRequest.Raise(
                new SendMessageViewModel(contact, this),
                sendMessage =>
                {
                    if (sendMessage.Result.HasValue && sendMessage.Result.Value)
                    {
                        this.SendingMessage = true;

                        this.chatService.SendMessage(
                            contact,
                            sendMessage.Message,
                            result =>
                            {
                                this.SendingMessage = false;
                            });
                    }
                });
        }

        private void OnCurrentContactChanged(object sender, EventArgs a)
        {
            this.RaisePropertyChanged(() => this.CurrentContact);
            this.showDetailsCommand.RaiseCanExecuteChanged();
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs a)
        {
            this.showReceivedMessageRequest.Raise(a.Message);
        }

        private class ShowDetailsCommandImplementation : ICommand
        {
            private readonly ChatViewModel owner;

            public ShowDetailsCommandImplementation(ChatViewModel owner)
            {
                this.owner = owner;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return this.owner.ContactsView.CurrentItem != null;
            }

            public void Execute(object parameter)
            {
                this.owner.ShowDetails = (bool)parameter;
            }

            public void RaiseCanExecuteChanged()
            {
                var handler = this.CanExecuteChanged;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
    }
}
