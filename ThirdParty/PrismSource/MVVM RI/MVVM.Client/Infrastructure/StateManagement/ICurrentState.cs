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

namespace MVVM.Client.Infrastructure.StateManagement
{
    /// <summary>
    /// Holds the current state for type <typeparamref name="T"/> in a <see cref="CompositionContainer"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Types consuming the current state must import the <see cref="IState{T}"/>, while types setting the
    /// current state must acquire the <see cref="ICurrentState{T}"/> and set its <see cref="Value"/>.
    /// </para>
    /// <para>
    /// Parts implementing this interface do not require to be explicitly exported because this interface is
    /// annotated with the <see cref="InheritedExportAttribute."/>
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type for which the current state is managed.</typeparam>
    /// <seealso cref="IState{T}"/>
    [InheritedExport]
    public interface ICurrentState<T>
    {
        /// <summary>
        /// Gets and sets the current value.
        /// </summary>
        T Value { get; set; }
    }
}
