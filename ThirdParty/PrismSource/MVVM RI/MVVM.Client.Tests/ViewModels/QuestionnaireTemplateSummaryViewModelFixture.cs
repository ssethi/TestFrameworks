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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVVM.Client.ViewModels;
using MVVM.Questionnaires.Model;
using MVVM.Repository;
using MVVM.TestSupport;

namespace MVVM.Client.Tests.ViewModels
{
    [TestClass]
    public class QuestionnaireTemplateSummaryViewModelFixture
    {
        [TestMethod]
        public void WhenViewModelIsCreated_ThenItHasNoSummaryOrSelectedTemplate()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>();

            var viewModel = new QuestionnaireTemplateSummaryViewModel(questionnaireRepositoryMock.Object);

            Assert.IsNull(viewModel.CurrentlySelectedQuestionnaireTemplate);
            Assert.IsNull(viewModel.QuestionnaireTemplateSummary);
        }

        [TestMethod]
        public void WhenSelectedTemplateIsSetToNonNull_ThenSummaryIsRequestedAndStatusChangesToUpdatingSummary()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>();
            var requestedSummary = false;
            questionnaireRepositoryMock
                .Setup(qs => qs.GetQuestionnaireTemplateSummaryAsync(It.IsAny<QuestionnaireTemplate>(), It.IsAny<Action<IOperationResult<QuestionnaireTemplateSummary>>>()))
                .Callback<QuestionnaireTemplate, Action<IOperationResult<QuestionnaireTemplateSummary>>>((qt, ac) => { requestedSummary = true; });

            var viewModel = new QuestionnaireTemplateSummaryViewModel(questionnaireRepositoryMock.Object);

            var tracker = new PropertyChangeTracker(viewModel);
            viewModel.CurrentlySelectedQuestionnaireTemplate = new QuestionnaireTemplate();

            Assert.IsTrue(requestedSummary);
            Assert.IsNull(viewModel.QuestionnaireTemplateSummary);
            Assert.AreEqual("RequestingSummary", viewModel.CurrentState);
            CollectionAssert.Contains(tracker.ChangedProperties, "CurrentState");
        }

        [TestMethod]
        public void WhenSelectedTemplateIsSetToNull_ThenSummaryIsCleared()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);

            var viewModel = new QuestionnaireTemplateSummaryViewModel(questionnaireRepositoryMock.Object);

            viewModel.CurrentlySelectedQuestionnaireTemplate = null;

            Assert.IsNull(viewModel.QuestionnaireTemplateSummary);
            Assert.AreEqual("Normal", viewModel.CurrentState);
            questionnaireRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void WhenSummaryRequestOperationisCompleted_ThenUpdatesCurrentSummaryAndStatusChangesToNormal()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>();

            Action<IOperationResult<QuestionnaireTemplateSummary>> summaryCallback = null;
            var summary = new QuestionnaireTemplateSummary();
            questionnaireRepositoryMock
                .Setup(qs => qs.GetQuestionnaireTemplateSummaryAsync(It.IsAny<QuestionnaireTemplate>(), It.IsAny<Action<IOperationResult<QuestionnaireTemplateSummary>>>()))
                .Callback<QuestionnaireTemplate, Action<IOperationResult<QuestionnaireTemplateSummary>>>((qt, ac) => summaryCallback = ac);

            var viewModel = new QuestionnaireTemplateSummaryViewModel(questionnaireRepositoryMock.Object);

            viewModel.CurrentlySelectedQuestionnaireTemplate = new QuestionnaireTemplate();

            var summaryResult = new Mock<IOperationResult<QuestionnaireTemplateSummary>>();
            summaryResult.SetupGet(r => r.Result).Returns(summary);

            var tracker = new PropertyChangeTracker(viewModel);
            summaryCallback(summaryResult.Object);

            Assert.AreEqual("Normal", viewModel.CurrentState);
            Assert.AreSame(summary, viewModel.QuestionnaireTemplateSummary);
            CollectionAssert.Contains(tracker.ChangedProperties, "CurrentState");
            CollectionAssert.Contains(tracker.ChangedProperties, "QuestionnaireTemplateSummary");
        }

        [TestMethod]
        public void WhenCurrentlySelectedTemplateChangesBeforePreviousSummaryRequestOperationisCompleted_ThenOriginalRequestOperationCompletionDoesNotCauseAnUpdate()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>();

            Action<IOperationResult<QuestionnaireTemplateSummary>> summaryCallback = null;
            var summary = new QuestionnaireTemplateSummary();
            questionnaireRepositoryMock
                .Setup(qs => qs.GetQuestionnaireTemplateSummaryAsync(It.IsAny<QuestionnaireTemplate>(), It.IsAny<Action<IOperationResult<QuestionnaireTemplateSummary>>>()))
                .Callback<QuestionnaireTemplate, Action<IOperationResult<QuestionnaireTemplateSummary>>>((qt, ac) => summaryCallback = ac);

            var viewModel = new QuestionnaireTemplateSummaryViewModel(questionnaireRepositoryMock.Object);

            viewModel.CurrentlySelectedQuestionnaireTemplate = new QuestionnaireTemplate();

            var originalSummaryCallback = summaryCallback;

            var summaryResult = new Mock<IOperationResult<QuestionnaireTemplateSummary>>();
            summaryResult.SetupGet(r => r.Result).Returns(summary);

            viewModel.CurrentlySelectedQuestionnaireTemplate = new QuestionnaireTemplate();

            originalSummaryCallback(summaryResult.Object);

            Assert.AreEqual("RequestingSummary", viewModel.CurrentState);
            Assert.IsNull(viewModel.QuestionnaireTemplateSummary);

            summaryCallback(summaryResult.Object);

            Assert.AreEqual("Normal", viewModel.CurrentState);
            Assert.AreSame(summary, viewModel.QuestionnaireTemplateSummary);
        }

        [TestMethod]
        public void WhenCurrentlySelectedTemplateChangesToNullBeforePreviousSummaryRequestOperationisCompleted_ThenStatusIsSetToNormalAndOriginalRequestOperationCompletionDoesNotCauseAnUpdate()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>();

            Action<IOperationResult<QuestionnaireTemplateSummary>> summaryCallback = null;
            var summary = new QuestionnaireTemplateSummary();
            questionnaireRepositoryMock
                .Setup(qs => qs.GetQuestionnaireTemplateSummaryAsync(It.IsAny<QuestionnaireTemplate>(), It.IsAny<Action<IOperationResult<QuestionnaireTemplateSummary>>>()))
                .Callback<QuestionnaireTemplate, Action<IOperationResult<QuestionnaireTemplateSummary>>>((qt, ac) => summaryCallback = ac);

            var viewModel = new QuestionnaireTemplateSummaryViewModel(questionnaireRepositoryMock.Object);

            viewModel.CurrentlySelectedQuestionnaireTemplate = new QuestionnaireTemplate();

            var originalSummaryCallback = summaryCallback;

            var summaryResult = new Mock<IOperationResult<QuestionnaireTemplateSummary>>();
            summaryResult.SetupGet(r => r.Result).Returns(summary);

            viewModel.CurrentlySelectedQuestionnaireTemplate = null;

            Assert.AreEqual("Normal", viewModel.CurrentState);
            Assert.IsNull(viewModel.QuestionnaireTemplateSummary);

            originalSummaryCallback(summaryResult.Object);

            Assert.AreEqual("Normal", viewModel.CurrentState);
            Assert.IsNull(viewModel.QuestionnaireTemplateSummary);
        }
    }
}
