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
    /// Snapshot of the current state for <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Parts implementing this interface do not require to be explicitly exported because this interface is
    /// annotated with the <see cref="InheritedExportAttribute."/>
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type for which a snapshot is required.</typeparam>
    /// <seealso cref="ICurrentState{T}"/>
    [InheritedExport]
    public interface IState<T>
    {
        /// <summary>
        /// Gets the value for the snapshot.
        /// </summary>
        T Value { get; }
    }
}
