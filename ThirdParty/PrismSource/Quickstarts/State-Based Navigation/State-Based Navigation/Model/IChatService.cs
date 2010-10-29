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
using StateBasedNavigation.Infrastructure;

namespace StateBasedNavigation.Model
{
    public interface IChatService
    {
        event EventHandler ConnectionStatusChanged;

        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        bool Connected { get; set; }

        void GetContacts(Action<IOperationResult<IEnumerable<Contact>>> callback);

        void SendMessage(Contact contact, string message, Action<IOperationResult> callback);
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(Contact contact, string message)
        {
            this.Message = new ReceivedMessage(contact, message);
        }

        public ReceivedMessage Message { get; private set; }
    }
}
