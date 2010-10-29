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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVVM.Client.Tests.Mocks;
using MVVM.Questionnaires.Model;
using MVVM.Questionnaires.Services;
using MVVM.Repository;

namespace MVVM.Client.Tests.Repository
{
    [TestClass]
    public class QuestionnaireRepositoryFixture : SilverlightTest
    {
        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenQueryingForQuestionnaireTemplates_ThenInvokesCallback()
        {
            AsyncResult<IEnumerable<QuestionnaireTemplate>> ar = null;

            var serviceMock = new MockQuestionnaireService();

            serviceMock.HandleBeginGetQuestionnaireTemplates = (r) => ar = r;

            var repository = new QuestionnaireRepository(serviceMock);

            bool calledBack = false;

            repository.GetQuestionnaireTemplatesAsync((result) => { calledBack = true; });

            serviceMock.ProceedGetQuestionnaireTemplates(ar, new[] { new QuestionnaireTemplate() });

            EnqueueConditional(() => calledBack);

            EnqueueCallback(() => { Assert.IsTrue(calledBack); });
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenQueryingForQuestionnareTemplates_ThenProvidesQuestionnaireTemplateResult()
        {
            AsyncResult<IEnumerable<QuestionnaireTemplate>> ar = null;
            QuestionnaireTemplate[] suppliedQuestionnaire = new[] { new QuestionnaireTemplate() };

            var serviceMock = new MockQuestionnaireService();

            serviceMock.HandleBeginGetQuestionnaireTemplates = (r) => ar = r;


            var repository = new QuestionnaireRepository(serviceMock);

            IOperationResult<IEnumerable<QuestionnaireTemplate>> getResult = null;
            repository.GetQuestionnaireTemplatesAsync((result) => getResult = result);

            serviceMock.ProceedGetQuestionnaireTemplates(ar, suppliedQuestionnaire);

            EnqueueConditional(() => getResult != null);
            EnqueueCallback(() =>
            {
                Assert.AreSame(suppliedQuestionnaire.ElementAt(0), getResult.Result.ElementAt(0));
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
                .Setup(s => s.BeginGetQuestionnaireTemplates(It.IsAny<AsyncCallback>(), It.IsAny<object>()))
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
                .Setup(s => s.EndGetQuestionnaireTemplates(It.IsAny<IAsyncResult>()))
                .Returns(new[] { new QuestionnaireTemplate() });

            var repository = new QuestionnaireRepository(mockService.Object);
            int callingThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            int calledBackThreadId = 0;

            IOperationResult<IEnumerable<QuestionnaireTemplate>> getResult = null;
            repository.GetQuestionnaireTemplatesAsync((r) =>
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
                .Setup(s => s.BeginGetQuestionnaireTemplates(It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns(mockAsyncResult.Object)
                .Callback<AsyncCallback, object>((c, o) =>
                {
                    c(mockAsyncResult.Object);
                });

            mockService
                .Setup(s => s.EndGetQuestionnaireTemplates(It.IsAny<IAsyncResult>()))
                .Throws(new NotImplementedException("TestException"));

            var repository = new QuestionnaireRepository(mockService.Object);

            IOperationResult<IEnumerable<QuestionnaireTemplate>> getResult = null;
            repository.GetQuestionnaireTemplatesAsync((r) => { getResult = r; });

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
                .Setup(s => s.BeginSubmitQuestionnaire(It.IsAny<Questionnaire>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
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
                .Setup(s => s.EndSubmitQuestionnaire(It.IsAny<IAsyncResult>()));

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
                .Setup(s => s.BeginSubmitQuestionnaire(It.IsAny<Questionnaire>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns(mockAsyncResult.Object)
                .Callback<Questionnaire, AsyncCallback, object>((q, c, o) =>
                {
                    c(mockAsyncResult.Object);
                });

            mockService
                .Setup(s => s.EndSubmitQuestionnaire(It.IsAny<IAsyncResult>()))
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


        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenQueryingForQuestionnaireTemplateSummary_ThenInvokesCallback()
        {
            AsyncResult<QuestionnaireTemplateSummary> ar = null;

            var serviceMock = new MockQuestionnaireService();

            serviceMock.HandleBeginGetQuestionnaireTemplateSummary = (r) => ar = r;

            var repository = new QuestionnaireRepository(serviceMock);

            bool calledBack = false;

            repository.GetQuestionnaireTemplateSummaryAsync(new QuestionnaireTemplate(), (result) => { calledBack = true; });

            serviceMock.ProceedGetQuestionnaireTemplateSummary(ar, new QuestionnaireTemplateSummary());

            EnqueueConditional(() => calledBack);

            EnqueueCallback(() => { Assert.IsTrue(calledBack); });
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenQueryingForQuestionnareTemplateSummary_ThenProvidesQuestionnaireTemplateSummaryResult()
        {
            AsyncResult<QuestionnaireTemplateSummary> ar = null;
            QuestionnaireTemplateSummary suppliedQuestionnaireTemplateSummary = new QuestionnaireTemplateSummary();

            var serviceMock = new MockQuestionnaireService();

            serviceMock.HandleBeginGetQuestionnaireTemplateSummary = (r) => ar = r;


            var repository = new QuestionnaireRepository(serviceMock);

            IOperationResult<QuestionnaireTemplateSummary> getResult = null;
            repository.GetQuestionnaireTemplateSummaryAsync(new QuestionnaireTemplate(), (result) => getResult = result);

            serviceMock.ProceedGetQuestionnaireTemplateSummary(ar, suppliedQuestionnaireTemplateSummary);

            EnqueueConditional(() => getResult != null);
            EnqueueCallback(() =>
            {
                Assert.AreSame(suppliedQuestionnaireTemplateSummary, getResult.Result);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenQueryingForQuestionnaireTemplateSummary_ThenPostsCallingThreadsSyncContext()
        {
            var mockAsyncResult = new Mock<IAsyncResult>();
            var mockService = new Mock<IQuestionnaireService>();

            mockService
                .Setup(s => s.BeginGetQuestionnaireTemplateSummary(It.IsAny<QuestionnaireTemplate>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns(mockAsyncResult.Object)
                .Callback<QuestionnaireTemplate, AsyncCallback, object>(
                    (qt, c, o) =>
                    {
                        System.Threading.ThreadPool.QueueUserWorkItem(
                            (state) =>
                            {
                                c(mockAsyncResult.Object);
                            });
                    });

            mockService
                .Setup(s => s.EndGetQuestionnaireTemplateSummary(It.IsAny<IAsyncResult>()))
                .Returns(new QuestionnaireTemplateSummary());

            var repository = new QuestionnaireRepository(mockService.Object);
            int callingThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            int calledBackThreadId = 0;

            IOperationResult<QuestionnaireTemplateSummary> getResult = null;
            repository.GetQuestionnaireTemplateSummaryAsync(
                new QuestionnaireTemplate(),
                (r) =>
                {
                    getResult = r;
                    calledBackThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                });

            EnqueueConditional(() => getResult != null);
            EnqueueCallback(() => { Assert.AreEqual(callingThreadId, calledBackThreadId); });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenExceptionOccursDuringQueryForQuestionnaireTemplateSummary_ThenExceptionCapturedAndReturned()
        {
            var mockAsyncResult = new Mock<IAsyncResult>();
            var mockService = new Mock<IQuestionnaireService>();

            mockService
                .Setup(s => s.BeginGetQuestionnaireTemplateSummary(It.IsAny<QuestionnaireTemplate>(), It.IsAny<AsyncCallback>(), It.IsAny<object>()))
                .Returns(mockAsyncResult.Object)
                .Callback<QuestionnaireTemplate, AsyncCallback, object>(
                    (qt, c, o) =>
                    {
                        c(mockAsyncResult.Object);
                    });

            mockService
                .Setup(s => s.EndGetQuestionnaireTemplateSummary(It.IsAny<IAsyncResult>()))
                .Throws(new NotImplementedException("TestException"));

            var repository = new QuestionnaireRepository(mockService.Object);

            IOperationResult<QuestionnaireTemplateSummary> getResult = null;
            repository.GetQuestionnaireTemplateSummaryAsync(new QuestionnaireTemplate(), (r) => { getResult = r; });

            EnqueueConditional(() => getResult != null);
            EnqueueCallback(() => { Assert.IsInstanceOfType(getResult.Error, typeof(NotImplementedException)); });

            EnqueueTestComplete();
        }
    }
}
