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
using System.Windows.Data;
using System.Windows.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Phone.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.Interactivity.Tests
{
    [TestClass]
    public class BindableInteractionRequestTriggerBaseFixture
    {
        [TestMethod]
        public void TriggerReceivesNotificationsFromBindedInteractionRequest()
        {
            string notificationReceived = "Foo";
            var page = new PhoneApplicationPage();
            var myNotificationAwareObject = new MyNotificationAwareClass();
            var binding = new Binding("InteractionRequest") { Source = myNotificationAwareObject, Mode = BindingMode.OneWay };
            var trigger = new MyTriggerClass
            {
                 RequestBinding = binding,
                 NotifyAction = c => notificationReceived = c.Title,
            };
            Interaction.GetBehaviors(page).Add(trigger);

            var initialState = notificationReceived;
            myNotificationAwareObject.InteractionRequest.Raise(new Notification { Title = "Bar" });
            var barState = notificationReceived;
            myNotificationAwareObject.InteractionRequest.Raise(new Notification { Title = "Finish" });
            var finalState = notificationReceived;
            Assert.AreEqual("Foo", initialState);
            Assert.AreEqual("Bar", barState);
            Assert.AreEqual("Finish", finalState);
        }

        [TestMethod]
        public void TriggerInvokesCallbacksAfterProcessingReceivedNotifications()
        {
            string notificationReceived = "Foo";
            var callbackInvoked = 0;
            var callbackCallsWhenNotificationIsHandled = 0;
            var page = new PhoneApplicationPage();
            var myNotificationAwareObject = new MyNotificationAwareClass();
            var binding = new Binding("InteractionRequest") { Source = myNotificationAwareObject, Mode = BindingMode.OneWay };
            var trigger = new MyTriggerClass
            {
                RequestBinding = binding,
                NotifyAction = c => { notificationReceived = c.Title; callbackCallsWhenNotificationIsHandled = callbackInvoked; }
            };
            Interaction.GetBehaviors(page).Add(trigger);
            var initialState = notificationReceived;
            myNotificationAwareObject.InteractionRequest.Raise(new Notification { Title = "Bar" }, n => callbackInvoked++);
            var barState = notificationReceived;
            myNotificationAwareObject.InteractionRequest.Raise(new Notification { Title = "Finish" }, n => callbackInvoked++);
            var finalState = notificationReceived;

            Assert.AreEqual("Foo", initialState);
            Assert.AreEqual("Bar", barState);
            Assert.AreEqual("Finish", finalState);
            Assert.AreEqual(2, callbackInvoked);
            Assert.AreEqual(1, callbackCallsWhenNotificationIsHandled);
        }

        public class MyTriggerClass : BindableInteractionRequestTriggerBase
        {
            public Action<Notification> NotifyAction;

            protected override void Notify(object sender, InteractionRequestedEventArgs e)
            {
                if (this.NotifyAction != null)
                {
                    this.NotifyAction.Invoke(e.Context);
                }

                if (e.Callback != null)
                {
                    e.Callback.Invoke();
                }
            }
        }

        public class MyNotificationAwareClass
        {
            public MyNotificationAwareClass()
            {
                this.InteractionRequest = new InteractionRequest<Notification>();
            }

            public InteractionRequest<Notification> InteractionRequest { get; private set; }
        }
    }
}
