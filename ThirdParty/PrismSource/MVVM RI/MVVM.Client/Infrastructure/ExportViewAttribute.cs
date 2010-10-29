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
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace MVVM.Client.Infrastructure
{
    /// <summary>
    /// MEF Metadata attribute to ensure views are registered appropriately
    /// </summary>
    /// <remarks>
    /// This works in conjunction with <see cref="ViewFactory"/> to locate views from the <see cref="System.ComponentModel.Composition.Hosting.CompositionContainer"/>
    /// </remarks>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportViewAttribute : ExportAttribute
    {
        public ExportViewAttribute(string viewName)
            : base(viewName, typeof(UserControl))
        {
        }
    }
}
