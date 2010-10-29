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

namespace MVVM.Infrastructure
{
    public class WeakEventListener<TInstance, TSender, TEventArgs>
        where TInstance : class
    {
        private readonly WeakReference instanceReference;
        private Action<TInstance, TSender, TEventArgs> handlerAction;
        private Action<WeakEventListener<TInstance, TSender, TEventArgs>> detachAction;

        public WeakEventListener(
            TInstance instance,
            Action<TInstance, TSender, TEventArgs> handlerAction,
            Action<WeakEventListener<TInstance, TSender, TEventArgs>> detachAction)
        {
            this.instanceReference = new WeakReference(instance);
            this.handlerAction = handlerAction;
            this.detachAction = detachAction;
        }

        public void Detach()
        {
            if (this.detachAction != null)
            {
                this.detachAction(this);
                this.detachAction = null;
            }
        }

        public void OnEvent(TSender sender, TEventArgs e)
        {
            var instance = this.instanceReference.Target as TInstance;
            if (instance != null)
            {
                if (this.handlerAction != null)
                {
                    this.handlerAction((TInstance)this.instanceReference.Target, sender, e);
                }
            }
            else
            {
                this.Detach();
            }
        }
    }
}
