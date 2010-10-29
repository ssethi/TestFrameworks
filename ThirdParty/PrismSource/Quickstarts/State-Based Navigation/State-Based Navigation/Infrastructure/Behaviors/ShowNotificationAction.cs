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
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace StateBasedNavigation.Infrastructure.Behaviors
{
    /// <summary>
    /// Trigger action that handles an interaction request by temporarily adding the context of the interaction request 
    /// to a collection and setting this collection as the DataContext of a framework element.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class ShowNotificationAction : TargetedTriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty NotificationTimeoutProperty =
            DependencyProperty.Register("NotificationTimeout", typeof(TimeSpan), typeof(ShowNotificationAction), new PropertyMetadata(new TimeSpan(0, 0, 5)));

        private ObservableCollection<object> notifications;

        public ShowNotificationAction()
        {
            this.notifications = new ObservableCollection<object>();
        }

        public TimeSpan NotificationTimeout
        {
            get { return (TimeSpan)GetValue(NotificationTimeoutProperty); }

            set { SetValue(NotificationTimeoutProperty, value); }
        }

        protected override void OnTargetChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
        {
            base.OnTargetChanged(oldTarget, newTarget);

            if (oldTarget != null)
            {
                this.Target.ClearValue(FrameworkElement.DataContextProperty);
            }

            if (newTarget != null)
            {
                this.Target.DataContext = this.notifications;
            }
        }

        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
            {
                return;
            }

            var notification = args.Context;

            this.notifications.Insert(0, notification);

            var timer = new DispatcherTimer { Interval = this.NotificationTimeout };
            EventHandler timerCallback = null;
            timerCallback =
                (o, e) =>
                {
                    timer.Stop();
                    timer.Tick -= timerCallback;
                    this.notifications.Remove(notification);
                };
            timer.Tick += timerCallback;
            timer.Start();

            args.Callback();
        }
    }
}
