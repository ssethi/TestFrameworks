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
using System.Diagnostics.CodeAnalysis;

namespace MVVM.Client.Infrastructure.StateManagement
{
    /// <summary>
    /// Generic implementation for the <see cref="IState{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The state type.</typeparam>
    /// <seealso cref="ICurrentState{T}"/>
    public abstract class State<T> : IState<T>
    {
        /// <summary>
        /// Sets the current state.
        /// </summary>
        /// <remarks>
        /// This set-only property is intended to be imported by MEF, which results in the current state being 
        /// stored as the <see cref="State{T}.Value"/>
        /// </remarks>
        [Import]
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Justification = "Needs to be a property to be composed by MEF")]
        public ICurrentState<T> CurrentState
        {
            set { this.Value = value.Value; }
        }

        /// <summary>
        /// Gets the state stored by this object.
        /// </summary>
        public T Value { get; private set; }
    }
}
