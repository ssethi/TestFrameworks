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
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Client.Infrastructure;

namespace MVVM.Client.Tests.Infrastructure
{
    [TestClass]
    public class WeakEventListenerFixture : SilverlightTest
    {
        [TestMethod]
        public void WhenAttachedToAnEvent_ThenInvokesCallbackOnEvent()
        {
            var instance = this;
            var sender = new EventSource();
            var args = new EventArgs();
            var invoked = false;
            var weakHandler =
                new WeakEventHandler<WeakEventListenerFixture, object, EventArgs>(
                    instance,
                    (i, s, e) =>
                    {
                        Assert.AreSame(this, i);
                        Assert.AreSame(sender, s);
                        Assert.AreSame(args, e);
                        invoked = true;
                    },
                    handler => { sender.Event -= handler.OnEvent; });

            sender.Event += weakHandler.OnEvent;

            sender.FireEvent(args);

            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void WhenDetached_ThenDoesNotInvokeCallbackOnEvent()
        {
            var instance = this;
            var sender = new EventSource();
            var invoked = false;
            var weakHandler =
                new WeakEventHandler<WeakEventListenerFixture, object, EventArgs>(
                    instance,
                    (i, s, e) =>
                    {
                        invoked = true;
                    },
                    handler => { sender.Event -= handler.OnEvent; });

            sender.Event += weakHandler.OnEvent;
            weakHandler.Detach();

            sender.FireEvent(new EventArgs());

            Assert.IsFalse(invoked);
        }

        [TestMethod]
        public void WhenDetachedWithNullDetachAction_ThenInvokesCallbackOnEvent()
        {
            var instance = this;
            var sender = new EventSource();
            var invoked = false;
            var weakHandler =
                new WeakEventHandler<WeakEventListenerFixture, object, EventArgs>(
                    instance,
                    (i, s, e) =>
                    {
                        invoked = true;
                    },
                    null);

            sender.Event += weakHandler.OnEvent;
            weakHandler.Detach();

            sender.FireEvent(new EventArgs());

            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void WhenInstanceIsNull_ThenHandlerIsDetached()
        {
            WeakEventListenerFixture instance = null;
            var sender = new EventSource();
            var invoked = false;
            var detached = false;
            var weakHandler =
                new WeakEventHandler<WeakEventListenerFixture, object, EventArgs>(
                    instance,
                    (i, s, e) =>
                    {
                        invoked = true;
                    },
                    handler =>
                    {
                        sender.Event -= handler.OnEvent;
                        detached = true;
                    });

            sender.Event += weakHandler.OnEvent;

            sender.FireEvent(new EventArgs());

            Assert.IsFalse(invoked);
            Assert.IsTrue(detached);
        }

        private class EventSource
        {
            public event EventHandler Event;

            public void FireEvent(EventArgs e)
            {
                if (this.Event != null)
                {
                    this.Event(this, e);
                }
            }
        }
    }
}
