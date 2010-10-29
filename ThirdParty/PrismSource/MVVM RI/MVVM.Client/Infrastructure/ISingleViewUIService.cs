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

namespace MVVM.Client.Infrastructure
{
    /// <summary>
    /// Simple UI service that maps a view name to a view and shows it as the current view.
    /// </summary>
    /// <seealso cref="IMainView"/>
    /// <seealso cref="IState{T}"/>
    /// <seealso cref="SingleViewUIService{TMainView}"/>
    public interface ISingleViewUIService
    {
        /// <summary>
        /// Gets the MainWindow which hosts the current view;
        /// </summary>
        IMainView MainWindow { get; }

        /// <summary>
        /// Shows the view identified by <see cref="viewName"/> as the current view.
        /// </summary>
        /// <param name="viewName">The view name.</param>
        void ShowView(string viewName);

        /// <summary>
        /// Shows the view identified by <see cref="viewName"/> as the current view, making <paramref name="context"/>
        /// available to views and view models.
        /// </summary>
        /// <remarks>
        /// <paramref name="context"/> is a plain object. It must be imported by the view associated to 
        /// <paramref name="viewName"/> or its view model. See <see cref="IState{T}"/> for details.
        /// </remarks>
        /// <typeparam name="T">The type for the <paramref name="context"/>.</typeparam>
        /// <param name="viewName">The view name.</param>
        /// <param name="context">Context information required by the target view.</param>
        void ShowView<T>(string viewName, T context);
    }
}
