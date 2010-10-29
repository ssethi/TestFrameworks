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
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;

namespace UIComposition.Shell.ViewModels
{
    public class ShellViewModel : INotifyPropertyChanged
    {
        public ShellViewModel()
        {
            // Initialize this ViewModel's commands.
            this.ExitCommand = new DelegateCommand<object>(this.AppExit, this.CanAppExit);
        }

        #region ExitCommand

        public DelegateCommand<object> ExitCommand { get; private set; }

        private void AppExit(object commandArg)
        {
        }

        private bool CanAppExit(object commandArg)
        {
            return true;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}