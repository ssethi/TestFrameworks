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
using System.Windows.Controls.Primitives;

namespace StockTraderRI.Infrastructure.Behaviors
{
    /// <summary>
    /// Defines a behavior for <see cref="ButtonBase"/> that on <see cref="ButtonBase.Click"/> closes the ancestor <see cref="Popup"/> in the Visual Tree.
    /// </summary>
    public static class ButtonBehaviors
    {
        /// <summary>
        /// When <see langword="true"/> closes the ancestor <see cref="Popup"/> on <see cref="ButtonBase.Click"/>.
        /// </summary>
        public static readonly DependencyProperty CloseAncestorPopupProperty = DependencyProperty.RegisterAttached(
               "CloseAncestorPopup", typeof(bool), typeof(ButtonBehaviors), new PropertyMetadata(OnCloseAncestorPopupChanged));

        /// <summary>
        /// Gets the value of <see cref="CloseAncestorPopupProperty"/>.
        /// </summary>
        /// <param name="dependencyObject">The button on which the behavior is attached.</param>
        /// <returns>The value of <see cref="CloseAncestorPopupProperty"/>.</returns>
        public static bool GetCloseAncestorPopup(DependencyObject dependencyObject)
        {
            return (bool)(dependencyObject.GetValue(CloseAncestorPopupProperty) ?? false);
        }

        /// <summary>
        /// Sets the value of <see cref="CloseAncestorPopupProperty"/>.
        /// </summary>
        /// <param name="dependencyObject">The button on which the behavior will be attached.</param>
        /// <param name="value">The value to set on <see cref="CloseAncestorPopupProperty"/>.</param>
        public static void SetCloseAncestorPopup(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(CloseAncestorPopupProperty, value);
        }

        private static void OnCloseAncestorPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonBase button = d as ButtonBase;
            if (button != null)
            {
                if ((bool) e.NewValue)
                {
                    button.Click += CloseButtonClicked;
                }
                else
                {
                    button.Click -= CloseButtonClicked;
                }
            }
        }

        private static void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            ButtonBase button = sender as ButtonBase;
            if (button != null && GetCloseAncestorPopup(button))
            {
                var popup = TreeHelper.FindAncestor(button, d => d is Popup) as Popup;
                if (popup != null)
                {
                    popup.IsOpen = false;
                }
            }
        }
    }
}
