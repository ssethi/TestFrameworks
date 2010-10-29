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
using System.ComponentModel.Composition.Hosting;

namespace MVVM.Client.Infrastructure.StateManagement
{
    /// <summary>
    /// Provides access to state management objects in the container and handles setting state while returning previous values.
    /// </summary>
    public class StateHandler
    {
        private readonly CompositionContainer container;

        public StateHandler(CompositionContainer container)
        {
            this.container = container;
        }

        public T SetState<T>(T context)
        {
            return SetCurrentState<T>(context);
        }

        /// <summary>
        /// Sets <paramref name="context"/> as the current context for type <typeparamref name="T"/>.
        /// </summary>
        private T SetCurrentState<T>(T context)
        {
            var currentState = GetInstance<ICurrentState<T>>();
            var previousValue = currentState.Value;
            currentState.Value = context;

            return previousValue;
        }

        private T GetInstance<T>()
        {
            return this.container.GetExportedValue<T>();
        }
    }
}
