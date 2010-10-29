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

namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    /// Displays a message box as a result of <see cref="IInteractionRequest"/>.
    /// </summary>
    public class MessageBoxRequestTrigger : BindableInteractionRequestTriggerBase
    {
        /// <summary>
        /// Invoked when the interaction request is raised and displays
        /// a message box with content specified in the <see cref="InteractionRequestedEventArgs"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Notify(object sender, InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            MessageBox.Show((string)e.Context.Content, e.Context.Title, MessageBoxButton.OK);
            if (e.Callback != null)
            {
                e.Callback.Invoke();
            }
        }
    }
}
