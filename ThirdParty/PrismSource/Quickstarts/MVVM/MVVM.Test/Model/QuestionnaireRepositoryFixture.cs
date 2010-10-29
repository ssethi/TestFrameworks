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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Model;
using MVVM.Test.Mocks;
using Microsoft.Silverlight.Testing;
using System.Threading;
using System;
using Moq;

namespace MVVM.Test.Model
{
    [TestClass]
    public class QuestionnaireRepositoryFixture : Microsoft.Silverlight.Testing.SilverlightTest
    {
        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenQueryingForQuestionnaires_ThenInvokesCallback()
        {
            AsyncResult<Questionnaire> ar = null;

            var serviceMock = new MockQuestionnaireService();
            serviceMock.HandleBeginGetQuestionnaire = (r) => ar = r;

            var repository = new QuestionnaireRepository(serviceMock);

            bool calledBack = false;

            repository.GetQuestionnaireAsync((result) => { calledBack = true; });

            serviceMock.ProceedGetQuestionnaire(ar, new Questionnaire());

            EnqueueConditional(() => calledBack);

            EnqueueCallback(() => { Assert.IsTrue(calledBack); });
            EnqueueTestComplete();
        }


        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenQueryingForQuestionnares_ThenProvidesQuestionnaireResult()
        {
            AsyncResult<Questionnaire> ar = null;
            Questionnaire suppliedQuestionnaire = new Questionnaire();

            var serviceMock = new MockQuestionnaireService();
            serviceMock.HandleBeginGetQuestionnaire = (r) => ar = r;

            var repository = new QuestionnaireRepository(serviceMock);

            IOperationResult<Questionnaire> getResult = null;
            repository.GetQuestionnaireAsync((result) => getResult = result);

            serviceMock.ProceedGetQuestionnaire(ar, suppliedQuestionnaire);

            EnqueueConditional(() => getResult != null);
            EnqueueCallback(() =>
            {
                Assert.AreSame(suppliedQuestionnaire, getResult.Result);
            });

            EnqueueTestComplete();
        }


        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenQueryingForQuestionnaires_ThenPostsCallingThreadsSyncContext()
        {
            var mockAsyncResult = new Mock<IAsyncResult>();
            var mockService = new Mock<IQuestionnaireService>();

            mockService
                .Setup(s => s.BeginGetQuestionnaire(It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns(mockAsyncResult.Object)
                .Callback<AsyncCallback, object>((c, o) =>
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem(
                            (state) =>
                            {
                                c(mockAsyncResult.Object);
                            });
                    });

            mockService
                .Setup(s => s.EndGetQuestionnaire(It.IsAny<IAsyncResult>()))
                .Returns(new Questionnaire());

            var repository = new QuestionnaireRepository(mockService.Object);
            int callingThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            int calledBackThreadId = 0;

            IOperationResult<Questionnaire> getResult = null;
            repository.GetQuestionnaireAsync((r) =>
                {
                    getResult = r;
                    calledBackThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                }
            );

            EnqueueConditional(() => getResult != null);
            EnqueueCallback(() => { Assert.AreEqual(callingThreadId, calledBackThreadId); });

            EnqueueTestComplete();
        }


        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenServiceThrowsAnError_ThenErrorIsCaptured()
        {
            var mockAsyncResult = new Mock<IAsyncResult>();
            var mockService = new Mock<IQuestionnaireService>();

            mockService
                .Setup(s => s.BeginGetQuestionnaire(It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns(mockAsyncResult.Object)
                .Callback<AsyncCallback, object>((c, o) =>
                {
                    c(mockAsyncResult.Object);
                });

            mockService
                .Setup(s => s.EndGetQuestionnaire(It.IsAny<IAsyncResult>()))
                .Throws(new NotImplementedException("TestException"));

            var repository = new QuestionnaireRepository(mockService.Object);

            IOperationResult<Questionnaire> getResult = null;
            repository.GetQuestionnaireAsync((r) => { getResult = r; });

            EnqueueConditional(() => getResult != null);
            EnqueueCallback(() => { Assert.IsInstanceOfType(getResult.Error, typeof(NotImplementedException)); });

            EnqueueTestComplete();
        }


        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenSubmittingQuestionnare_ThenInvokesCallback()
        {
            AsyncResult<object> ar = null;

            var serviceMock = new MockQuestionnaireService();
            serviceMock.HandleBeginSubmitResponses = (r) => ar = r;

            var repository = new QuestionnaireRepository(serviceMock);

            bool calledBack = false;

            repository.SubmitQuestionnaireAsync(new Questionnaire(), (result) => { calledBack = true; });

            serviceMock.ProceedSubmitResponses(ar);

            EnqueueConditional(() => calledBack);

            EnqueueCallback(() => { Assert.IsTrue(calledBack); });
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenSubmittingQuestionnaire_ThenPostsToCallingThreadsSyncContext()
        {
            var mockAsyncResult = new Mock<IAsyncResult>();
            var mockService = new Mock<IQuestionnaireService>();

            mockService
                .Setup(s => s.BeginSubmitResponses(It.IsAny<Questionnaire>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns(mockAsyncResult.Object)
                .Callback<Questionnaire, AsyncCallback, object>((q, c, o) =>
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(
                        (state) =>
                        {
                            c(mockAsyncResult.Object);
                        });
                });

            mockService
                .Setup(s => s.EndSubmitResponses(It.IsAny<IAsyncResult>()));

            var repository = new QuestionnaireRepository(mockService.Object);
            int callingThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            int calledBackThreadId = 0;

            IOperationResult submitResult = null;
            repository.SubmitQuestionnaireAsync(
                new Questionnaire(),
                (r) =>
                {
                    submitResult = r;
                    calledBackThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                }
            );

            EnqueueConditional(() => submitResult != null);
            EnqueueCallback(() => { Assert.AreEqual(callingThreadId, calledBackThreadId); });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenExceptionOccursDuringSubmission_ThenExceptionCapturedAndReturned()
        {
            var mockAsyncResult = new Mock<IAsyncResult>();
            var mockService = new Mock<IQuestionnaireService>();

            mockService
                .Setup(s => s.BeginSubmitResponses(It.IsAny<Questionnaire>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns(mockAsyncResult.Object)
                .Callback<Questionnaire, AsyncCallback, object>((q, c, o) =>
                {
                    c(mockAsyncResult.Object);
                });

            mockService
                .Setup(s => s.EndSubmitResponses(It.IsAny<IAsyncResult>()))
                .Throws(new NotImplementedException("TestException"));

            var repository = new QuestionnaireRepository(mockService.Object);

            IOperationResult submitResult = null;
            repository.SubmitQuestionnaireAsync(
                new Questionnaire(),
                (r) => { submitResult = r; }
            );

            EnqueueConditional(() => submitResult != null);
            EnqueueCallback(() => { Assert.IsInstanceOfType(submitResult.Error, typeof(NotImplementedException)); });

            EnqueueTestComplete();
        }
    }
}
