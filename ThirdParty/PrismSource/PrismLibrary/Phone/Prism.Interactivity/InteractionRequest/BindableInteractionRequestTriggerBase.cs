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
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// A bindable interaction request for building new triggers.
    /// </summary>
    public abstract class BindableInteractionRequestTriggerBase : Behavior<FrameworkElement>
    {
        /// <summary>
        /// The binding to the <see cref="IInteractionRequest"/>.
        /// </summary>
        public static readonly DependencyProperty RequestBindingProperty =
            DependencyProperty.Register("RequestBinding", typeof(Binding), typeof(BindableInteractionRequestTriggerBase), new PropertyMetadata(HandleBindingChanged));

        private readonly BindingListener requestBindinglistener;

        /// <summary>
        /// Instantiates a new instance of <see cref="BindableInteractionRequestTriggerBase"/>
        /// </summary>
        protected BindableInteractionRequestTriggerBase()
        {
            this.requestBindinglistener = new BindingListener(this.ChangeHandler);
        }

        /// <summary>
        /// The <see cref="Binding"/> that should resolve to a <see cref="IInteractionRequest"/>
        /// </summary>
        public Binding RequestBinding
        {
            get { return (Binding)GetValue(RequestBindingProperty); }
            set { SetValue(RequestBindingProperty, value); }
        }

        /// <summary>
        /// The <see cref="IInteractionRequest"/> that is the result of the <see cref="RequestBinding"/>.
        /// </summary>
        protected IInteractionRequest Request { get; set; }

        /// <summary>
        /// Invoked when the binding value changes.
        /// </summary>
        /// <param name="e"></param>
        protected void OnBindingChanged(DependencyPropertyChangedEventArgs e)
        {
            this.requestBindinglistener.Binding = (Binding)e.NewValue;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            this.HandleBindingChange(null);
            base.OnDetaching();
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            this.requestBindinglistener.Element = this.AssociatedObject;
            base.OnAttached();
        }

        /// <summary>
        /// Invoked when the interaction request is raised. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void Notify(object sender, InteractionRequestedEventArgs e);

        private static void HandleBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((BindableInteractionRequestTriggerBase)sender).OnBindingChanged(e);
        }

        private void ChangeHandler(object sender, BindingChangedEventArgs e)
        {
            this.HandleBindingChange(this.requestBindinglistener.Value as IInteractionRequest);
        }

        private void HandleBindingChange(IInteractionRequest newRequest)
        {
            if (this.Request != null)
            {
                this.Request.Raised -= this.Notify;
            }

            if (newRequest != null)
            {
                newRequest.Raised += this.Notify;
            }

            this.Request = newRequest;
        }
    }
}
