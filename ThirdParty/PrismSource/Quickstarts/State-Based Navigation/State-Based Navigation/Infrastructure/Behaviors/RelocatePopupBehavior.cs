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
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace StateBasedNavigation.Infrastructure.Behaviors
{
    /// <summary>
    /// Behavior that ensures a popup is located at the bottom-right corner of its parent.
    /// </summary>
    public class RelocatePopupBehavior : Behavior<Popup>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.Opened += this.OnPopupOpened;
            this.AssociatedObject.Closed += this.OnPopupClosed;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Opened -= this.OnPopupOpened;
            this.AssociatedObject.Closed -= this.OnPopupClosed;

            this.DetachSizeChangeHandlers();

            base.OnDetaching();
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            this.UpdatePopupOffsets();
            this.AttachSizeChangeHandlers();
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            this.DetachSizeChangeHandlers();
        }

        private void AttachSizeChangeHandlers()
        {
            var child = this.AssociatedObject.Child as FrameworkElement;
            if (child != null)
            {
                child.SizeChanged += this.OnChildSizeChanged;
            }

            var parent = this.AssociatedObject.Parent as FrameworkElement;
            if (parent != null)
            {
                parent.SizeChanged += this.OnParentSizeChanged;
            }
        }

        private void DetachSizeChangeHandlers()
        {
            var child = this.AssociatedObject.Child as FrameworkElement;
            if (child != null)
            {
                child.SizeChanged -= this.OnChildSizeChanged;
            }

            var parent = this.AssociatedObject.Parent as FrameworkElement;
            if (parent != null)
            {
                parent.SizeChanged -= this.OnParentSizeChanged;
            }
        }

        private void OnChildSizeChanged(object sender, EventArgs e)
        {
            this.UpdatePopupOffsets();
        }

        private void OnParentSizeChanged(object sender, EventArgs e)
        {
            this.UpdatePopupOffsets();
        }

        private void UpdatePopupOffsets()
        {
            if (this.AssociatedObject != null)
            {
                var child = this.AssociatedObject.Child as FrameworkElement;
                var parent = this.AssociatedObject.Parent as FrameworkElement;

                if (child != null && parent != null)
                {
                    var anchor = new Point(parent.ActualWidth, parent.ActualHeight);

                    this.AssociatedObject.HorizontalOffset = anchor.X - child.ActualWidth;
                    this.AssociatedObject.VerticalOffset = anchor.Y - child.ActualHeight;
                }
            }
        }
    }
}
