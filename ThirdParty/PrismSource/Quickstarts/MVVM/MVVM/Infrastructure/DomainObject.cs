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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.Prism.ViewModel;

namespace MVVM.Infrastructure
{
    /// <summary>
    /// Base Entity class.
    /// </summary>
    /// <remarks>
    /// Provides support for implementing <see cref="INotifyPropertyChanged"/> and 
    /// implements <see cref="INotifyDataErrorInfo"/> using <see cref="ValidationAttribute"/> instances
    /// on the validated properties.
    /// </remarks>
    public abstract class DomainObject : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private ErrorsContainer<string> errorsContainer;

        protected DomainObject()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors
        {
            get { return this.ErrorsContainer.HasErrors; }
        }

        protected ErrorsContainer<string> ErrorsContainer
        {
            get
            {
                if (this.errorsContainer == null)
                {
                    this.errorsContainer =
                        new ErrorsContainer<string>(pn => this.RaiseErrorsChanged(pn));
                }

                return this.errorsContainer;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return this.ErrorsContainer.GetErrors(propertyName);
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Method supports event.")]
        protected void RaisePropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void ValidateProperty(string propertyName, object value)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Method supports event.")]
        protected void RaiseErrorsChanged(string propertyName)
        {
            this.OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            var handler = this.ErrorsChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
