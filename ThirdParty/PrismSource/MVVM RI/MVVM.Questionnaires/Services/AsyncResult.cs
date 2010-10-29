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
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace MVVM.Questionnaires.Services
{
    [SuppressMessage("Microsoft.Design", "CA1001",
        Justification = "Calling the End method, which is part of the contract of using an IAsyncResult, releases the IDisposable.")]
    public class AsyncResult<T> : IAsyncResult
    {
        private readonly object lockObject;
        private readonly AsyncCallback asyncCallback;
        private readonly object asyncState;
        private ManualResetEvent waitHandle;
        private T result;
        private Exception exception;
        private bool isCompleted;
        private bool completedSynchronously;
        private bool endCalled;

        public AsyncResult(AsyncCallback asyncCallback, object asyncState)
        {
            this.lockObject = new object();
            this.asyncCallback = asyncCallback;
            this.asyncState = asyncState;
        }

        public object AsyncState
        {
            get { return this.asyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this.lockObject)
                {
                    if (this.waitHandle == null)
                    {
                        this.waitHandle = new ManualResetEvent(this.IsCompleted);
                    }
                }

                return this.waitHandle;
            }
        }

        public bool CompletedSynchronously
        {
            get { return this.completedSynchronously; }
        }

        public bool IsCompleted
        {
            get { return this.isCompleted; }
        }

        public T Result
        {
            get { return this.result; }
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "Entry point to be used to implement End* methods.")]
        public static AsyncResult<T> End(IAsyncResult asyncResult)
        {
            var localResult = asyncResult as AsyncResult<T>;
            if (localResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }

            lock (localResult.lockObject)
            {
                if (localResult.endCalled)
                {
                    throw new InvalidOperationException("End method already called");
                }

                localResult.endCalled = true;
            }

            if (!localResult.IsCompleted)
            {
                localResult.AsyncWaitHandle.WaitOne();
            }

            if (localResult.waitHandle != null)
            {
                localResult.waitHandle.Close();
            }

            if (localResult.exception != null)
            {
                throw localResult.exception;
            }

            return localResult;
        }

        public void SetComplete(T result, bool completedSynchronously)
        {
            this.result = result;

            this.DoSetComplete(completedSynchronously);
        }

        public void SetComplete(Exception e, bool completedSynchronously)
        {
            this.exception = e;

            this.DoSetComplete(completedSynchronously);
        }

        private void DoSetComplete(bool completedSynchronously)
        {
            if (completedSynchronously)
            {
                this.completedSynchronously = true;
                this.isCompleted = true;
            }
            else
            {
                lock (this.lockObject)
                {
                    this.isCompleted = true;
                    if (this.waitHandle != null)
                    {
                        this.waitHandle.Set();
                    }
                }
            }

            if (this.asyncCallback != null)
            {
                this.asyncCallback(this);
            }
        }
    }
}
