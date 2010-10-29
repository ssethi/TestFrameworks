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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Microsoft.Practices.Prism.Regions
{
    public partial class ViewsCollection
    {
        private void NotifyAdd(IList items)
        {
            if (items.Count <= 0)
            {
                return;
            }

            foreach (var item in items)
            {
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                             NotifyCollectionChangedAction.Add,
                                             item,
                                             0));
            }
        }

        private void NotifyRemove(IList items)
        {
            if (items.Count <= 0)
            {
                return;
            }

            foreach (var item in items)
            {
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                             NotifyCollectionChangedAction.Remove,
                                             item,
                                             0));
            }
        }
    }
}
