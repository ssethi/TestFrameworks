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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Implementation of <see cref="IViewsCollection"/> that takes an <see cref="ObservableCollection{T}"/> of <see cref="ItemMetadata"/>
    /// and filters it to display an <see cref="INotifyCollectionChanged"/> collection of
    /// <see cref="object"/> elements (the items which the <see cref="ItemMetadata"/> wraps).
    /// </summary>
    public partial class ViewsCollection : IViewsCollection
    {
        private readonly ObservableCollection<ItemMetadata> subjectCollection;
        private readonly Dictionary<ItemMetadata, MonitorInfo> monitoredItems = new Dictionary<ItemMetadata, MonitorInfo>();
        private readonly Predicate<ItemMetadata> filter;
        private Comparison<object> sort;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsCollection"/> class.
        /// </summary>
        /// <param name="list">The list to wrap and filter.</param>
        /// <param name="filter">A predicate to filter the <paramref name="list"/> collection.</param>
        public ViewsCollection(ObservableCollection<ItemMetadata> list, Predicate<ItemMetadata> filter)
        {
            this.subjectCollection = list;
            this.filter = filter;
            MonitorAllMetadataItems();
            subjectCollection.CollectionChanged += SourceCollectionChanged;
        }

        private IEnumerable<object> FilteredItems
        {
            get { return this.subjectCollection.Where(i => this.filter(i)).Select(i => i.Item); }
        }

        /// <summary>
        /// Gets or sets the comparison used to sort the views.
        /// </summary>
        /// <value>The comparison to use.</value>
        public Comparison<object> SortComparison
        {
            get
            {
                return this.sort;
            }
            set
            {
                if (this.sort != value)
                {
                    this.sort = value;
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="value">The object to locate in the collection.</param>
        /// <returns><see langword="true" /> if <paramref name="value"/> is found in the collection; otherwise, <see langword="false" />.</returns>
        public bool Contains(object value)
        {
            return this.FilteredItems.Contains(value);
        }

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///</returns>
        public IEnumerator<object> GetEnumerator()
        {
            return this.FilteredItems.OrderBy<object, object>(o => o, new RegionItemComparer(SortComparison)).GetEnumerator();
        }

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        ///</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Used to invoked the <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler Handler = CollectionChanged;
            if (Handler != null) Handler(this, e);
        }

        private void NotifyReset()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes all monitoring of underlying MetadataItems and re-adds them.
        /// </summary>
        private void ResetAllMonitors()
        {
            RemoveAllMetadataMonitors();
            MonitorAllMetadataItems();
        }

        /// <summary>
        /// Adds all underlying MetadataItems to the list from the subjectCollection
        /// </summary>
        private void MonitorAllMetadataItems()
        {
            foreach (var item in this.subjectCollection)
            {
                this.AddMetadataMonitor(item, this.filter(item));
            }
        }

        /// <summary>
        /// Removes all monitored items from our monitoring list.
        /// </summary>
        private void RemoveAllMetadataMonitors()
        {
            foreach (var item in this.monitoredItems)
            {
                item.Key.MetadataChanged -= this.OnItemMetadataChanged;
            }
            this.monitoredItems.Clear();
        }

        /// <summary>
        /// Adds handler to monitor the MetadatItem and adds it to our monitoring list.
        /// </summary>
        /// <param name="itemMetadata"></param>
        /// <param name="isInList"></param>
        private void AddMetadataMonitor(ItemMetadata itemMetadata, bool isInList)
        {
            itemMetadata.MetadataChanged += this.OnItemMetadataChanged;
            monitoredItems.Add(
                itemMetadata, 
                new MonitorInfo
                    {
                        IsInList = isInList
                    });
        }

        /// <summary>
        /// Unhooks from the MetadataItem change event and removes from our monitoring list.
        /// </summary>
        /// <param name="itemMetadata"></param>
        private void RemoveMetadataMonitor(ItemMetadata itemMetadata)
        {
            itemMetadata.MetadataChanged -= OnItemMetadataChanged;
            this.monitoredItems.Remove(itemMetadata);
        }

        /// <summary>
        /// Invoked when any of the underlying ItemMetadata items we're monitoring changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemMetadataChanged(object sender, EventArgs e)
        {
            ItemMetadata itemMetadata = (ItemMetadata)sender;

            // Our monitored item may have been removed during another event before
            // our OnItemMetadataChanged got called back, so it's not unexpected
            // that we may not have it in our list.
            MonitorInfo monitorInfo;
            bool foundInfo = this.monitoredItems.TryGetValue(itemMetadata, out monitorInfo);
            if (!foundInfo) return;

            if (this.filter(itemMetadata))
            {
                if (!monitorInfo.IsInList)
                { 
                    // This passes our filter and wasn't marked
                    // as in our list so we can consider this
                    // an Add.
                    monitorInfo.IsInList = true;
                    this.NotifyAdd(new[] { itemMetadata.Item });
                }
            }
            else
            {
                // This doesn't fit our filter, we remove from our
                // tracking list, but should not remove any monitoring in
                // case it fits our filter in the future.
                monitorInfo.IsInList = false;
                this.NotifyRemove(new[] { itemMetadata.Item });
            }
        }

        /// <summary>
        /// The event handler due to changes in the underlying collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            List<object> changedItems = new List<object>();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ItemMetadata itemMetadata in e.NewItems)
                    {
                        bool isInFilter = filter(itemMetadata);
                        AddMetadataMonitor(itemMetadata, isInFilter);
                        if (isInFilter)
                        {
                            changedItems.Add(itemMetadata.Item);
                        }
                    }
                    this.NotifyAdd(changedItems);

                    // If we're sorting we can't predict how
                    // the collection has changed on an add so we 
                    // resort to a reset notification.
                    if (sort != null)
                    {
                        this.NotifyReset();
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (ItemMetadata itemMetadata in e.OldItems)
                    {
                        RemoveMetadataMonitor(itemMetadata);
                        if (this.filter(itemMetadata))
                        {
                            changedItems.Add(itemMetadata.Item);
                        }
                    }
                    this.NotifyRemove(changedItems);
                    break;

                default:
                    this.ResetAllMonitors();
                    this.NotifyReset();
                    break;
            }
        }

        private class MonitorInfo
        {
            public bool IsInList { get; set; }
        }

        private class RegionItemComparer : Comparer<object>
        {
            private Comparison<object> comparer;

            public RegionItemComparer(Comparison<object> comparer)
            {
                this.comparer = comparer;
            }

            public override int Compare(object x, object y)
            {
                if (this.comparer == null)
                {
                    return 0;
                }

                return this.comparer(x, y);
            }
        }
    }
}