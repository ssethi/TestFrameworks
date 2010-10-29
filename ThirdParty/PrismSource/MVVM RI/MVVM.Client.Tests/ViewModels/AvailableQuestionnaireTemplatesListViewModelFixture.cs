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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVVM.Client.Infrastructure;
using MVVM.Client.ViewModels;
using MVVM.Questionnaires.Model;
using MVVM.Repository;

namespace MVVM.Client.Tests.ViewModels
{
    [TestClass]
    public class AvailableQuestionnaireTemplatesListViewModelFixture
    {
        [TestMethod]
        public void WhenViewModelIsCreated_ThenItRequestsAvailableTemplates()
        {
            bool loadRequested = false;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r =>
                    r.GetQuestionnaireTemplatesAsync(It.IsAny<Action<IOperationResult<IEnumerable<QuestionnaireTemplate>>>>()))
                .Callback<Action<IOperationResult<IEnumerable<QuestionnaireTemplate>>>>(c => loadRequested = true);

            var uiService = new Mock<ISingleViewUIService>();

            var viewModel = new AvailableQuestionnaireTemplatesListViewModel(repository.Object, uiService.Object);

            Assert.IsTrue(loadRequested);
            Assert.AreEqual(0, viewModel.QuestionnaireTemplates.Count);
        }

        [TestMethod]
        public void WhenInitialLoadOperationIscompleted_ThenViewModelPopulatesTheCollectionOfQuestionnaireTemplates()
        {
            Action<IOperationResult<IEnumerable<QuestionnaireTemplate>>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r =>
                    r.GetQuestionnaireTemplatesAsync(It.IsAny<Action<IOperationResult<IEnumerable<QuestionnaireTemplate>>>>()))
                .Callback<Action<IOperationResult<IEnumerable<QuestionnaireTemplate>>>>(c => callback = c);

            var uiService = new Mock<ISingleViewUIService>();

            var viewModel = new AvailableQuestionnaireTemplatesListViewModel(repository.Object, uiService.Object);

            Assert.AreEqual(0, viewModel.QuestionnaireTemplates.Count);

            callback(CreateQuestionnaireTemplates());

            Assert.AreEqual("Questionnaire #1", viewModel.QuestionnaireTemplates[0].Title);
            Assert.AreEqual("Questionnaire #2", viewModel.QuestionnaireTemplates[1].Title);
        }

        [TestMethod]
        public void WhenExecutingTheTakeSurveyCommand_ThenRequestsTransitionToQuestionnaireView()
        {
            var repository = new Mock<IQuestionnaireRepository>();
            repository
                .Setup(r => r.GetQuestionnaireTemplatesAsync(It.IsAny<Action<IOperationResult<IEnumerable<QuestionnaireTemplate>>>>()));


            QuestionnaireTemplate templateRequested = null;
            var uiServiceMock = new Mock<ISingleViewUIService>();
            uiServiceMock
                .Setup(svc => svc.ShowView(ViewNames.CompleteQuestionnaire, It.IsAny<QuestionnaireTemplate>()))
                .Callback<string, QuestionnaireTemplate>((vn, qt) => templateRequested = qt);

            var template = new QuestionnaireTemplate();

            var templateViewModel =
                new AvailableQuestionnaireTemplatesListViewModel(repository.Object, uiServiceMock.Object);

            templateViewModel.TakeSurveyCommand.Execute(template);

            Assert.AreSame(template, templateRequested);
        }

        private IOperationResult<IEnumerable<QuestionnaireTemplate>> CreateQuestionnaireTemplates()
        {
            return CreateQuestionnaireTemplates(
                new[] 
                {
                    new QuestionnaireTemplate { Title = "Questionnaire #1"},
                    new QuestionnaireTemplate { Title = "Questionnaire #2"}
                });
        }

        private IOperationResult<IEnumerable<QuestionnaireTemplate>> CreateQuestionnaireTemplates(IEnumerable<QuestionnaireTemplate> questionnaireTemplates)
        {
            var result = new Mock<IOperationResult<IEnumerable<QuestionnaireTemplate>>>();
            result.Setup(r => r.Result)
                .Returns(questionnaireTemplates);

            return result.Object;
        }
    }
}
