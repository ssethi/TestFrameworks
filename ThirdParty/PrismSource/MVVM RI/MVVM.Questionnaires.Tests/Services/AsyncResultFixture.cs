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
using System.Threading;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Questionnaires.Services;

namespace MVVM.Questionnaires.Tests.Services
{
    [TestClass]
    public class AsyncResultFixture : SilverlightTest
    {
        [TestMethod]
        public void WhenCreated_ThenIsNotComplete()
        {
            var result = new AsyncResult<object>(null, this);

            Assert.IsFalse(result.IsCompleted);
            Assert.IsFalse(result.CompletedSynchronously);
        }

        [TestMethod]
        public void WhenCreated_ThenCanReturnAsyncState()
        {
            var result = new AsyncResult<object>(null, this);

            Assert.AreSame(this, result.AsyncState);
        }

        [TestMethod]
        public void WhenCompletedWithNoCallback_ThenUpdatesCompletionStatus()
        {
            var result = new AsyncResult<object>(null, null);

            result.SetComplete(null, false);

            Assert.IsTrue(result.IsCompleted);
            Assert.IsFalse(result.CompletedSynchronously);
        }

        [TestMethod]
        public void WhenCompleted_ThenESetsResult()
        {
            var result = new AsyncResult<object>(null, null);

            result.SetComplete(this, true);

            Assert.AreSame(this, result.Result);
        }

        [TestMethod]
        public void WhenCompletedSynchronouslyWithNoCallback_ThenUpdatesCompletionStatus()
        {
            var result = new AsyncResult<object>(null, null);

            result.SetComplete(null, true);

            Assert.IsTrue(result.IsCompleted);
            Assert.IsTrue(result.CompletedSynchronously);
        }

        [TestMethod]
        public void WhenCompleted_ThenExecutesCallback()
        {
            bool executed = false;
            var result = new AsyncResult<object>(ar => executed = true, null);

            result.SetComplete(null, true);

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void WhenCompleted_ThenSignalsWaitHandleIfRequested()
        {
            var result = new AsyncResult<object>(null, null);

            var waitHandle = result.AsyncWaitHandle;
            var lockObject = new object();

            lock (lockObject)
            {
                ThreadPool.QueueUserWorkItem(
                    o =>
                    {
                        lock (lockObject)
                        {
                        }

                        result.SetComplete(null, false);
                    });
            }

            Assert.IsTrue(waitHandle.WaitOne(5000));
        }

        [TestMethod]
        public void WhenGettingWaitHandleAfterCompletion_ThenWaitHandleIsAlreadySet()
        {
            var result = new AsyncResult<object>(null, null);

            result.SetComplete(null, false);
            var waitHandle = result.AsyncWaitHandle;

            Assert.IsTrue(waitHandle.WaitOne(5000));
        }

        [TestMethod]
        public void WhenInvokingEndAfterCompleted_ThenContinuesExecution()
        {
            var result = new AsyncResult<object>(null, null);

            result.SetComplete(null, false);

            var returnedResult = AsyncResult<object>.End((IAsyncResult)result);

            Assert.AreSame(result, returnedResult);
        }

        [TestMethod]
        public void WhenInvokingEndAfterCompletedWithException_ThenThrowsCompletionException()
        {
            var result = new AsyncResult<object>(null, null);
            var exception = new Exception();

            result.SetComplete(exception, false);

            try
            {
                AsyncResult<object>.End((IAsyncResult)result);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreSame(exception, e);
            }
        }

        [TestMethod]
        public void WhenInvokingEndForASecondTime_ThenThrowsException()
        {
            var result = new AsyncResult<object>(null, null);

            result.SetComplete(null, false);

            AsyncResult<object>.End(result);
            try
            {
                AsyncResult<object>.End(result);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void WhenInvokingEndForTheWrongAsyncResultType_ThenThrowsException()
        {
            var result = new AsyncResult<object>(null, null);

            result.SetComplete(null, false);

            try
            {
                AsyncResult<int>.End(result);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenInvokingEndBeforeCompletion_ThenBlocksUntilComplete()
        {
            var result = new AsyncResult<object>(null, null);
            bool pre = false;
            bool post = false;

            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    pre = true;
                    AsyncResult<object>.End(result);
                    post = true;
                });

            EnqueueConditional(() => pre);

            EnqueueCallback(
                () =>
                {
                    Assert.IsFalse(post);
                    result.SetComplete(null, false);
                });

            EnqueueConditional(() => pre);

            EnqueueTestComplete();
        }
    }
}
