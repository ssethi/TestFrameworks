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
using System.Linq;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVVM.Client.Infrastructure;
using MVVM.Client.Infrastructure.StateManagement;
using MVVM.Client.Tests.Mocks;
using MVVM.Client.ViewModels;
using MVVM.Questionnaires.Model;
using MVVM.Repository;
using MVVM.TestSupport;

namespace MVVM.Client.Tests.ViewModels
{
    [TestClass]
    public class QuestionnaireViewModelFixture
    {
        [TestMethod]
        public void WhenTheViewModelIsCreatedWithATemplate_ThenItCreatesAQuestionnaireAndTheViewModelsForItsQuestions()
        {
            var questionnaireTemplate = CreateQuestionnaireTemplate();

            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            Assert.AreSame(questionnaireTemplate, viewModel.Questionnaire.Template);
            Assert.AreEqual(2, viewModel.Questions.Count);
            CollectionAssert.AreEqual(
                new[] { "Enter your name", "Enter your address" },
                viewModel.Questions.Cast<OpenQuestionViewModel>().Select(q => q.Question.QuestionText).ToArray());
        }

        [TestMethod]
        public void WhenQuestionnaireIsCreated_ThenAllQuestionsAreUnanswered()
        {
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            // Assertions
            Assert.AreEqual(2, viewModel.TotalQuestions);
            Assert.AreEqual(2, viewModel.UnansweredQuestions);
        }

        [TestMethod]
        public void WhenQuestionnaireIsCreated_ThenHasNoChanges()
        {
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            // Assertions
            Assert.IsFalse(viewModel.HasChanges);
        }

        [TestMethod]
        public void WhenQuestionIsAnswered_ThenAllUnansweredQuestionsAreUpdated()
        {
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            var question = viewModel.Questions.First() as OpenQuestionViewModel;
            question.Question.Response = "some text";

            // Assertions
            Assert.AreEqual(2, viewModel.TotalQuestions);
            Assert.AreEqual(1, viewModel.UnansweredQuestions);
        }

        [TestMethod]
        public void WhenQuestionIsAnswered_ThenHasChanges()
        {
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            var question = viewModel.Questions.First() as OpenQuestionViewModel;
            question.Question.Response = "some text";

            // Assertions
            Assert.IsTrue(viewModel.HasChanges);
        }

        [TestMethod]
        public void WhenQuestionIsAnswered_ThenQuestionnaireModelNotifiesUpdate()
        {
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            var changeTracker = new PropertyChangeTracker(viewModel);

            var question = viewModel.Questions.First() as OpenQuestionViewModel;
            question.Question.Response = "some text";

            // Assertions
            Assert.IsTrue(changeTracker.ChangedProperties.Contains("UnansweredQuestions"));
        }

        [TestMethod]
        public void WhenQuestionnaireIsNotCompleted_ThenCanSubmitReturnsFalse()
        {
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            AnswerAllQuestions(viewModel);

            // Assertions
            Assert.IsFalse(viewModel.CanSubmit);
        }

        [TestMethod]
        public void WhenQuestionnaireIsCompleted_ThenCanSubmitReturnsTrue()
        {
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            viewModel.Questionnaire.Name = "QuestionnaireName";
            viewModel.Questionnaire.Age = 25;
            AnswerAllQuestions(viewModel);

            // Assertions
            Assert.IsTrue(viewModel.CanSubmit);
        }

        [TestMethod]
        public void WhenNameIsSet_ThenHasChanges()
        {
            var questionnaireTemplate = new QuestionnaireTemplate { Title = "questionnaire" };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            viewModel.Questionnaire.Name = "name";

            Assert.IsTrue(viewModel.HasChanges);
        }

        [TestMethod]
        public void WhenAgeIsSet_ThenHasChanges()
        {
            var questionnaireTemplate = new QuestionnaireTemplate { Title = "questionnaire" };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            viewModel.Questionnaire.Age = 25;

            Assert.IsTrue(viewModel.HasChanges);
        }

        [TestMethod]
        public void WhenNameIsSet_ThenPropertyChangeOnCanSubmitIsFired()
        {
            var questionnaireTemplate = new QuestionnaireTemplate { Title = "questionnaire" };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            int canSubmitPropertyChanges = 0;
            viewModel.PropertyChanged += (s, e) => { if (e.PropertyName == "CanSubmit") canSubmitPropertyChanges++; };

            viewModel.Questionnaire.Name = "name";

            Assert.AreEqual(1, canSubmitPropertyChanges);
        }

        [TestMethod]
        public void WhenAgeIsSet_ThenPropertyChangeOnCanSubmitIsFired()
        {
            var questionnaireTemplate = new QuestionnaireTemplate { Title = "questionnaire" };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            int canSubmitPropertyChanges = 0;
            viewModel.PropertyChanged += (s, e) => { if (e.PropertyName == "CanSubmit") canSubmitPropertyChanges++; };

            viewModel.Questionnaire.Age = 25;

            Assert.AreEqual(1, canSubmitPropertyChanges);
        }

        [TestMethod]
        public void WhenHasErrorIsChangedToTrueOnTheViewModel_ThenPropertyChangeOnCanSubmitIsFired()
        {
            var questionnaireTemplate = new QuestionnaireTemplate { Title = "questionnaire" };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            int canSubmitPropertyChanges = 0;
            viewModel.PropertyChanged += (s, e) => { if (e.PropertyName == "CanSubmit") canSubmitPropertyChanges++; };
            viewModel.HasAgeBindingError = true;

            // Assertions
            Assert.AreEqual(1, canSubmitPropertyChanges);
        }

        [TestMethod]
        public void WhenHasErrorIsChangedToFalseOnTheViewModel_ThenPropertyChangeOnCanSubmitIsFired()
        {
            var questionnaireTemplate = new QuestionnaireTemplate { Title = "questionnaire" };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            viewModel.HasAgeBindingError = true;
            int canSubmitPropertyChanges = 0;
            viewModel.PropertyChanged += (s, e) => { if (e.PropertyName == "CanSubmit") canSubmitPropertyChanges++; };
            viewModel.HasAgeBindingError = false;

            // Assertions
            Assert.AreEqual(1, canSubmitPropertyChanges);
        }

        [TestMethod]
        public void WhenAgeIsSetOnTheModel_ThenTheHasErrorPropertyInTheViewModelIsClearedAndSinglePropertyChangeNotificationIsFired()
        {
            var questionnaireTemplate = new QuestionnaireTemplate { Title = "questionnaire" };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            viewModel.Questionnaire.Age = 25;
            viewModel.Questionnaire.Name = "name";

            Assert.IsTrue(viewModel.CanSubmit);

            viewModel.HasAgeBindingError = true;

            Assert.IsFalse(viewModel.CanSubmit);
        }


        [TestMethod]
        public void WhenCurrentStateIsModified_ThenViewModelNotifiesUpdate()
        {
            var questionnaireTemplate = CreateQuestionnaireTemplate();
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            var changeTracker = new PropertyChangeTracker(viewModel);

            viewModel.CurrentState = "newState";

            // Assertions
            Assert.IsTrue(changeTracker.ChangedProperties.Contains("CurrentState"));
        }

        [TestMethod]
        public void WhenQuestionnaireHasOpenTextQuestion_ThenTheMatchingViewModelIsCreated()
        {
            var questionnaireTemplate =
                new QuestionnaireTemplate
                {
                    Questions = { new OpenQuestionTemplate { QuestionText = "Open" } }
                };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            Assert.IsInstanceOfType(viewModel.Questions[0], typeof(OpenQuestionViewModel));
        }

        [TestMethod]
        public void WhenQuestionnaireHasMultipleSelectionQuestion_ThenTheMatchingViewModelIsCreated()
        {
            var questionnaireTemplate =
                new QuestionnaireTemplate
                {
                    Questions = { new MultipleSelectionQuestionTemplate { QuestionText = "Multiple selection" } }
                };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            Assert.IsInstanceOfType(viewModel.Questions[0], typeof(MultipleSelectionQuestionViewModel));
        }

        [TestMethod]
        public void WhenQuestionnaireHasNumericQuestion_ThenTheMatchingViewModelIsCreated()
        {
            var questionnaireTemplate =
                new QuestionnaireTemplate
                {
                    Questions = { new NumericQuestionTemplate { QuestionText = "Numeric" } }
                };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);

            Assert.IsInstanceOfType(viewModel.Questions[0], typeof(NumericQuestionViewModel));
        }

        [TestMethod]
        public void WhenQuestionnaireHasUnknownQuestion_ThenAnExceptionIsThrown()
        {
            var questionnaireTemplate =
                new QuestionnaireTemplate
                {
                    Questions = { new MockQuestionTemplate { QuestionText = "unknown" } }
                };

            try
            {
                var viewModel =
                    new QuestionnaireViewModel(
                        CreateStateMock(questionnaireTemplate).Object,
                        new Mock<IQuestionnaireRepository>().Object,
                        new Mock<ISingleViewUIService>().Object);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentException)
            {
                // expected
            }
        }

        [TestMethod]
        public void WhenQuestionHasFormattingErrorAfterHavingValidResponse_ThenViewModelCannotBeSubmitted()
        {
            var questionnaireTemplate =
                new QuestionnaireTemplate { Questions = { new NumericQuestionTemplate { QuestionText = "unknown" } } };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    new Mock<IQuestionnaireRepository>().Object,
                    new Mock<ISingleViewUIService>().Object);
            var questionViewModel = ((NumericQuestionViewModel)viewModel.Questions[0]);

            viewModel.Questionnaire.Name = "name";
            viewModel.Questionnaire.Age = 25;
            questionViewModel.Question.Response = 100;

            Assert.IsTrue(viewModel.CanSubmit);

            questionViewModel.HasBindingError = true;

            Assert.IsFalse(viewModel.CanSubmit);
        }

        // cancelling

        [TestMethod]
        public void WhenCancellingNewQuestionnaire_ThenViewModelNavigatesToQuestionnaireListView()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);

            var uiServiceMock = new Mock<ISingleViewUIService>(MockBehavior.Strict);
            uiServiceMock
                .Setup(svc => svc.ShowView(ViewNames.QuestionnaireTemplatesList))
                .Verifiable();

            var questionnaireTemplate =
                new QuestionnaireTemplate { Questions = { new NumericQuestionTemplate { QuestionText = "unknown" } } };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    questionnaireRepositoryMock.Object,
                    uiServiceMock.Object);

            viewModel.Cancel();

            questionnaireRepositoryMock.Verify();
            uiServiceMock.Verify();
        }

        [TestMethod]
        public void WhenCancellingModifiedQuestionnaire_ThenViewModelPromptsForConfirmation()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);

            var uiServiceMock = new Mock<ISingleViewUIService>(MockBehavior.Strict);

            var questionnaireTemplate =
                new QuestionnaireTemplate { Questions = { new NumericQuestionTemplate { QuestionText = "unknown" } } };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    questionnaireRepositoryMock.Object,
                    uiServiceMock.Object);

            viewModel.Questionnaire.Name = "name";

            InteractionRequestedEventArgs args = null;
            viewModel.CancelConfirmationInteractionRequest.Raised += (o, e) => { args = e; };

            viewModel.Cancel();

            Assert.IsNotNull(args);
            questionnaireRepositoryMock.Verify();
            uiServiceMock.Verify();
        }

        [TestMethod]
        public void WhenCancellingTheConfirmationViewAfterCancellingASubmit_ThenViewModelStaysOnTheQuestionnairePage()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);

            var uiServiceMock = new Mock<ISingleViewUIService>(MockBehavior.Strict);

            var questionnaireTemplate =
                new QuestionnaireTemplate { Questions = { new NumericQuestionTemplate { QuestionText = "unknown" } } };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    questionnaireRepositoryMock.Object,
                    uiServiceMock.Object);

            viewModel.Questionnaire.Name = "name";

            InteractionRequestedEventArgs args = null;
            viewModel.CancelConfirmationInteractionRequest.Raised += (o, e) => { args = e; };

            viewModel.Cancel();

            args.Callback();

            questionnaireRepositoryMock.Verify();
            uiServiceMock.Verify();
        }

        [TestMethod]
        public void WhenAcceptingTheConfirmationViewAfterCancellingASubmit_ThenViewModelNavigatesToQuestionnaireListView()
        {
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);

            bool navigatedAway = false;
            var uiServiceMock = new Mock<ISingleViewUIService>(MockBehavior.Strict);
            uiServiceMock
                .Setup(svc => svc.ShowView(ViewNames.QuestionnaireTemplatesList))
                .Callback(() => { navigatedAway = true; })
                .Verifiable();

            var questionnaireTemplate =
                new QuestionnaireTemplate { Questions = { new NumericQuestionTemplate { QuestionText = "unknown" } } };
            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(questionnaireTemplate).Object,
                    questionnaireRepositoryMock.Object,
                    uiServiceMock.Object);

            viewModel.Questionnaire.Name = "name";

            InteractionRequestedEventArgs args = null;
            viewModel.CancelConfirmationInteractionRequest.Raised += (o, e) => { args = e; };

            viewModel.Cancel();

            ((Confirmation)args.Context).Confirmed = true;

            args.Callback();

            Assert.IsTrue(navigatedAway);
            questionnaireRepositoryMock.Verify();
            uiServiceMock.Verify();
        }


        // submitting

        [TestMethod]
        public void WhenValidQuestionnaireIsSubmitted_ThenSubmitsChanges()
        {
            bool beginSubmitInvoked = false;
            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);
            questionnaireRepositoryMock
                .Setup(r => r.SubmitQuestionnaireAsync(It.IsAny<Questionnaire>(), It.IsAny<Action<IOperationResult>>()))
                .Callback(() => { beginSubmitInvoked = true; });

            var uiServiceMock = new Mock<ISingleViewUIService>();

            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    questionnaireRepositoryMock.Object,
                    uiServiceMock.Object); ;

            CompleteQuestionnaire(viewModel);

            viewModel.Submit();

            Assert.AreEqual("Submitting", viewModel.CurrentState);
            Assert.IsTrue(beginSubmitInvoked);
        }

        [TestMethod]
        public void WhenGettingSuccessfulSubmitNotification_ThenNavigatesToQuestionnaireListView()
        {
            Action<IOperationResult> callback = null;

            Mock<IOperationResult> submitResultMock = new Mock<IOperationResult>();
            submitResultMock.Setup(sr => sr.Error).Returns<Exception>(null);

            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);
            questionnaireRepositoryMock
                .Setup(r => r.SubmitQuestionnaireAsync(It.IsAny<Questionnaire>(), It.IsAny<Action<IOperationResult>>()))
                .Callback<Questionnaire, Action<IOperationResult>>((q, a) => { callback = a; });

            string requestedViewName = null;
            var uiServiceMock = new Mock<ISingleViewUIService>();
            uiServiceMock
                .Setup(svc => svc.ShowView(ViewNames.QuestionnaireTemplatesList))
                .Callback<string>(viewName => requestedViewName = viewName);

            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    questionnaireRepositoryMock.Object,
                    uiServiceMock.Object); ;

            CompleteQuestionnaire(viewModel);

            viewModel.Submit();

            callback(submitResultMock.Object);

            Assert.AreEqual(ViewNames.QuestionnaireTemplatesList, requestedViewName);
        }

        [TestMethod]
        public void WhenGettingFailedSubmitNotification_ThenRaisesNotification()
        {
            Action<IOperationResult> callback = null;
            Exception error = new InvalidOperationException();

            Mock<IOperationResult> submitResultMock = new Mock<IOperationResult>();
            submitResultMock.Setup(sr => sr.Error).Returns(error);

            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);
            questionnaireRepositoryMock
                .Setup(r => r.SubmitQuestionnaireAsync(It.IsAny<Questionnaire>(), It.IsAny<Action<IOperationResult>>()))
                .Callback<Questionnaire, Action<IOperationResult>>((q, a) => { callback = a; });

            var uiServiceMock = new Mock<ISingleViewUIService>(MockBehavior.Strict);

            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    questionnaireRepositoryMock.Object,
                    uiServiceMock.Object);

            CompleteQuestionnaire(viewModel);

            InteractionRequestedEventArgs notificationArgs = null;
            viewModel.SubmitErrorInteractionRequest.Raised += (o, e) => notificationArgs = e;

            viewModel.Submit();

            callback(submitResultMock.Object);

            Assert.IsNotNull(notificationArgs);
            Assert.IsInstanceOfType(notificationArgs.Context, typeof(Notification));

            questionnaireRepositoryMock.VerifyAll();
            uiServiceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenAcceptingFailedSubmitNotification_ThenStaysOnSameView()
        {
            Action<IOperationResult> callback = null;
            Exception error = new InvalidOperationException();

            Mock<IOperationResult> submitResultMock = new Mock<IOperationResult>();
            submitResultMock.Setup(sr => sr.Error).Returns(error);

            var questionnaireRepositoryMock = new Mock<IQuestionnaireRepository>(MockBehavior.Strict);
            questionnaireRepositoryMock
                .Setup(r => r.SubmitQuestionnaireAsync(It.IsAny<Questionnaire>(), It.IsAny<Action<IOperationResult>>()))
                .Callback<Questionnaire, Action<IOperationResult>>((q, a) => { callback = a; });

            var uiServiceMock = new Mock<ISingleViewUIService>(MockBehavior.Strict);

            var viewModel =
                new QuestionnaireViewModel(
                    CreateStateMock(CreateQuestionnaireTemplate()).Object,
                    questionnaireRepositoryMock.Object,
                    uiServiceMock.Object);

            CompleteQuestionnaire(viewModel);

            Action notificationCallback = null;
            viewModel.SubmitErrorInteractionRequest.Raised += (o, e) => notificationCallback = e.Callback;

            viewModel.Submit();

            callback(submitResultMock.Object);

            notificationCallback();

            questionnaireRepositoryMock.VerifyAll();
            uiServiceMock.VerifyAll();
        }


        private static void CompleteQuestionnaire(QuestionnaireViewModel viewModel)
        {
            viewModel.Questionnaire.Name = "Name";
            viewModel.Questionnaire.Age = 25;
            AnswerAllQuestions(viewModel);
        }

        private static Questionnaire CreateNonAnsweredQuestionnaire()
        {
            var questionnaireTemplate = CreateQuestionnaireTemplate();

            var questionnaire = new Questionnaire(questionnaireTemplate);

            return questionnaire;
        }

        private static QuestionnaireTemplate CreateQuestionnaireTemplate()
        {
            var questionnaireTemplate = new QuestionnaireTemplate()
            {
                Title = "Questionnaire",
                Questions = 
                { 
                    new OpenQuestionTemplate() 
                    { 
                        QuestionText = "Enter your name" ,
                        MaxLength = 25
                    },
                    new OpenQuestionTemplate() 
                    { 
                        QuestionText = "Enter your address",
                        MaxLength = 25
                    }
                }
            };
            return questionnaireTemplate;
        }

        private static void AnswerAllQuestions(QuestionnaireViewModel viewModel)
        {
            foreach (OpenQuestionViewModel questionViewModel in viewModel.Questions)
            {
                questionViewModel.Question.Response = "response";
            }
        }

        private static Mock<IState<T>> CreateStateMock<T>(T state)
        {
            var mock = new Mock<IState<T>>();
            mock.SetupGet(s => s.Value).Returns(state);

            return mock;
        }
    }
}
