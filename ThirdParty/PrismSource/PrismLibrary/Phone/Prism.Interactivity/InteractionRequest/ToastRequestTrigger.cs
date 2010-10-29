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
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
   
    /// <summary>
    /// Displays a popup "toast" item with content for some time 
    /// and then closes the popup.
    /// </summary>
    public class ToastRequestTrigger : BindableInteractionRequestTriggerBase
    {
        ///<summary>
        /// The element name of the <see cref="Popup"/> to show upon the interaction request.
        ///</summary>
        public static readonly DependencyProperty PopupElementNameProperty =
            DependencyProperty.Register(
                "PopupElementName",
                typeof(string),
                typeof(ToastRequestTrigger),
                new PropertyMetadata(null));

        private Timer closePopupTimer;

        ///<summary>
        /// Gets or sets the name of the <see cref="Popup"/> element to show when
        /// an <see cref="IInteractionRequest"/> is received.
        ///</summary>
        public string PopupElementName
        {
            get { return (string)GetValue(PopupElementNameProperty); }
            set { SetValue(PopupElementNameProperty, value); }
        }

        /// <summary>
        /// Shows the popup for 6 seconds and then closes it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Notify(object sender, InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            
            var popUp = (Popup)this.AssociatedObject.FindName(this.PopupElementName);
            popUp.DataContext = e.Context;
            popUp.IsOpen = true;
            this.DisposeTimer();
            this.closePopupTimer = new Timer(
                s => Deployment.Current.Dispatcher.BeginInvoke(() => popUp.IsOpen = false),
                null,
                6000,
                5000);
            popUp.Closed += this.OnPopupClosed;
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            this.DisposeTimer();
            ((Popup)sender).Closed -= this.OnPopupClosed;
        }

        private void DisposeTimer()
        {
            lock (this)
            {
                if (this.closePopupTimer != null)
                {
                    this.closePopupTimer.Dispose();
                    this.closePopupTimer = null;
                }
            }
        }
    }
}
