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
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Windows.Controls;

namespace MVVM.Client.Infrastructure
{
    /// <summary>
    /// Creates factories from the container.  This is explicitly registered vs. exported to avoid registering the container in the container.
    /// </summary>
    public class ViewFactory
    {
        private readonly CompositionContainer container;

        public ViewFactory(CompositionContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Returns the view that matches the supplied view name.
        /// </summary>
        /// <seealso cref="ExportViewAttribute"/>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public UserControl GetView(string viewName)
        {
            var view = this.container.GetExportedValueOrDefault<UserControl>(viewName);
            if (view == null)
            {
                throw new InvalidOperationException(string.Format("Unable to locate view with name {0}.", viewName));
            }

            return view;
        }
    }
}
