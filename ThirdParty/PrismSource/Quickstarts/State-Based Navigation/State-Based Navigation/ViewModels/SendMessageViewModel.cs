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
using System.Linq.Expressions;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.ComponentModel;
using StateBasedNavigation.Infrastructure;
using StateBasedNavigation.Model;

namespace StateBasedNavigation.ViewModels
{
    public class SendMessageViewModel : Notification, INotifyPropertyChanged
    {
        private readonly Contact contact;
        private readonly ChatViewModel parent;
        private bool? result;
        private string message;

        public SendMessageViewModel(Contact contact, ChatViewModel parent)
        {
            this.contact = contact;
            this.parent = parent;
        }

        public Contact Contact
        {
            get { return this.contact; }
        }

        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                if (value != this.message)
                {
                    this.message = value;                    
                    RaisePropertyChanged(() => this.Message);
                }
            }
        }

       
        public bool? Result
        {
            get
            {
                return this.result;
            }

            set
            {
                if (value != this.result)
                {
                    this.result = value;
                    RaisePropertyChanged(() => this.Result);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaisePropertyChanged<T>(Expression<Func<T>> lambda)
        {
            var name = PropertySupport.ExtractPropertyName<T>(lambda);
            OnPropertyChanged(name);
        }

    }
}
