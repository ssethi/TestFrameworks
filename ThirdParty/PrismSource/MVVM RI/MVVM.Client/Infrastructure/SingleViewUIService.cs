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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Controls;
using MVVM.Client.Infrastructure.StateManagement;

namespace MVVM.Client.Infrastructure
{
    /// <summary>
    /// Implementation of the <see cref="ISingleViewUIService"/> that relies on a <see cref="IMainView"/>
    /// </summary>
    /// <typeparam name="TMainView"><see cref="IMainView"/> type</typeparam>
    /// <seealso cref="ISingleViewUIService"/>
    /// <seealso cref="IState{T}"/>
    /// <seealso cref="ICurrentState{T}"/>
    public abstract class SingleViewUIService<TMainView> : ISingleViewUIService
        where TMainView : UserControl, IMainView, new()
    {
        private readonly TMainView mainWindow;

        protected SingleViewUIService()
        {
            this.mainWindow = new TMainView();
        }

        [Import(typeof(ViewFactory))]
        public ViewFactory ViewFactory { get; set; }

        [Import(typeof(StateHandler))]
        public StateHandler StateHandler { get; set; }

        /// <summary>
        /// Gets the MainWindow which hosts the current view;
        /// </summary>
        public IMainView MainWindow
        {
            get
            {
                return this.mainWindow;
            }
        }

        /// <summary>
        /// Shows the view identified by <see cref="viewName"/> as the current view.
        /// </summary>
        /// <param name="viewName">The view name.</param>
        public void ShowView(string viewName)
        {
            var view = this.ViewFactory.GetView(viewName);
            this.MainWindow.CurrentView = view;
        }

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
        public void ShowView<T>(string viewName, T context)
        {
            var previousValue = StateHandler.SetState(context);

            try
            {
                this.ShowView(viewName);
            }
            finally
            {
                StateHandler.SetState(previousValue);
            }
        }
    }
}
