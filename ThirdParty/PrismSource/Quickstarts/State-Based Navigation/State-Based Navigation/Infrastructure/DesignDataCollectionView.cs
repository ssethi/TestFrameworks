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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StateBasedNavigation.Infrastructure
{
    /// <summary>
    /// This class is used to specify design data for view models that expose an ICollectionView. It is not intended
    /// to be used by production code.
    /// </summary>
    public class DesignDataCollectionView : Collection<object>, ICollectionView
    {
        public bool CanFilter
        {
            get { throw new NotImplementedException(); }
        }

        public bool CanGroup
        {
            get { throw new NotImplementedException(); }
        }

        public bool CanSort
        {
            get { throw new NotImplementedException(); }
        }

        public System.Globalization.CultureInfo Culture
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

#pragma warning disable 0067
        public event EventHandler CurrentChanged;

        public event CurrentChangingEventHandler CurrentChanging;
#pragma warning restore 0067
        public object CurrentItem
        {
            get { throw new NotImplementedException(); }
        }

        public int CurrentPosition
        {
            get { throw new NotImplementedException(); }
        }

        public IDisposable DeferRefresh()
        {
            throw new NotImplementedException();
        }

        public Predicate<object> Filter
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyObservableCollection<object> Groups
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCurrentAfterLast
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCurrentBeforeFirst
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveCurrentTo(object item)
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToFirst()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToLast()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToNext()
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToPosition(int position)
        {
            throw new NotImplementedException();
        }

        public bool MoveCurrentToPrevious()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public SortDescriptionCollection SortDescriptions
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.IEnumerable SourceCollection
        {
            get { throw new NotImplementedException(); }
        }

#pragma warning disable 0067
        public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;
#pragma warning restore 0067
    }
}
