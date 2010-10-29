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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVVM.Model;
using MVVM.Test.Mocks;
using MVVM.Test.ViewModels.Utility;
using MVVM.ViewModels;

namespace MVVM.Test.ViewModels
{
    [TestClass]
    public class QuestionnaireViewModelFixture
    {
        [TestMethod]
        public void WhenQuestionnaireIsCreatedWithService_ThenRetrievesQuestionnaireFromService()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);
            var changeTracker = new PropertyChangeTracker(viewModel);

            Assert.AreEqual("Loading", viewModel.CurrentState);
            Assert.AreEqual(0, changeTracker.ChangedProperties.Count());

            callback(CreateQuestionnaireResult());

            // Assertions
            Assert.IsTrue(changeTracker.ChangedProperties.Contains("Questionnaire"));
            Assert.AreEqual("Normal", viewModel.CurrentState);
            Assert.IsTrue(changeTracker.ChangedProperties.Contains("CurrentState"));
            CollectionAssert.AreEqual(
                new[] { "Enter your name", "Enter your address" },
                viewModel.Questions.Cast<OpenQuestionViewModel>().Select(q => q.Question.QuestionText).ToArray());
        }

        [TestMethod]
        public void WhenQuestionnaireIsCompleted_ThenCanSubmitReturnsTrue()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            viewModel.Questionnaire.Name = "QuestionnaireName";
            viewModel.Questionnaire.Age = 5;
            AnswerAllQuestions(viewModel);

            // Assertions
            Assert.IsTrue(viewModel.CanSubmit);
        }

        [TestMethod]
        public void WhenQuestionnaireIsNotCompleted_ThenCanSubmitReturnsFalse()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            AnswerAllQuestions(viewModel);

            // Assertions
            Assert.IsFalse(viewModel.CanSubmit);
        }

        [TestMethod]
        public void WhenNameIsSet_ThenPropertyChangeOnCanSubmitIsFired()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(new Questionnaire { }));

            int canSubmitPropertyChanges = 0;
            viewModel.PropertyChanged += (s, e) => { if (e.PropertyName == "CanSubmit") canSubmitPropertyChanges++; };

            viewModel.Questionnaire.Name = "name";

            Assert.AreEqual(1, canSubmitPropertyChanges);
        }

        [TestMethod]
        public void WhenAgeIsSet_ThenPropertyChangeOnCanSubmitIsFired()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(new Questionnaire { }));

            int canSubmitPropertyChanges = 0;
            viewModel.PropertyChanged += (s, e) => { if (e.PropertyName == "CanSubmit") canSubmitPropertyChanges++; };

            viewModel.Questionnaire.Age = 25;

            Assert.AreEqual(1, canSubmitPropertyChanges);
        }

        [TestMethod]
        public void WhenHasErrorIsChangedToTrueOnTheViewModel_ThenPropertyChangeOnCanSubmitIsFired()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(new Questionnaire { }));

            int canSubmitPropertyChanges = 0;
            viewModel.PropertyChanged += (s, e) => { if (e.PropertyName == "CanSubmit") canSubmitPropertyChanges++; };
            viewModel.HasAgeBindingError = true;

            // Assertions
            Assert.AreEqual(1, canSubmitPropertyChanges);
        }

        [TestMethod]
        public void WhenHasErrorIsChangedToFalseOnTheViewModel_ThenPropertyChangeOnCanSubmitIsFired()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(new Questionnaire { }));

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
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(new Questionnaire { }));

            viewModel.Questionnaire.Age = 25;
            viewModel.Questionnaire.Name = "name";

            Assert.IsTrue(viewModel.CanSubmit);

            viewModel.HasAgeBindingError = true;

            Assert.IsFalse(viewModel.CanSubmit);
        }


        [TestMethod]
        public void WhenQuestionnaireIsCreated_ThenAllQuestionsAreUnanswered()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            // Assertions
            Assert.AreEqual(2, viewModel.TotalQuestions);
            Assert.AreEqual(2, viewModel.UnansweredQuestions);
        }

        [TestMethod]
        public void WhenQuestionIsAnswered_ThenAllUnansweredQuestionsAreUpdated()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            var question = viewModel.Questions.First() as OpenQuestionViewModel;
            question.Question.Response = "some text";

            // Assertions
            Assert.AreEqual(2, viewModel.TotalQuestions);
            Assert.AreEqual(1, viewModel.UnansweredQuestions);
        }

        [TestMethod]
        public void WhenQuestionIsAnswered_ThenQuestionnaireModelNotifiesUpdate()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            var question = viewModel.Questions.First() as OpenQuestionViewModel;
            var changeTracker = new PropertyChangeTracker(viewModel);
            question.Question.Response = "some text";

            // Assertions
            Assert.IsTrue(changeTracker.ChangedProperties.Contains("UnansweredQuestions"));
        }

        [TestMethod]
        public void WhenQuestionnaireIsSubmitted_ThenSubmitsToService()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            repository.Setup(r => r.SubmitQuestionnaireAsync(It.IsAny<Questionnaire>(), It.IsAny<Action<IOperationResult>>()))
                .Verifiable("SubmitQuestionnaireAsync not called");

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            viewModel.Questionnaire.Name = "Name";
            viewModel.Questionnaire.Age = 21;
            AnswerAllQuestions(viewModel);
            viewModel.Submit();

            repository.Verify();
        }

        [TestMethod]
        public void WhenCurrentStateIsModified_ThenViewModelNotifiesUpdate()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            var changeTracker = new PropertyChangeTracker(viewModel);

            viewModel.CurrentState = "newState";

            // Assertions
            Assert.IsTrue(changeTracker.ChangedProperties.Contains("CurrentState"));
        }

        [TestMethod]
        public void WhenServiceIsDoneSubmitting_ThenStateIsSetToNormal()
        {
            Action<IOperationResult<Questionnaire>> callback = null;
            Action<IOperationResult> submitCallback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);
            repository.Setup(r => r.SubmitQuestionnaireAsync(It.IsAny<Questionnaire>(), It.IsAny<Action<IOperationResult>>()))
                .Callback<Questionnaire, Action<IOperationResult>>(
                (q, sc) => submitCallback = sc);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            viewModel.Submit();
            Assert.AreEqual("Submitting", viewModel.CurrentState);

            var tracker = new PropertyChangeTracker(viewModel);

            // Responds as the service would.
            submitCallback(new Mock<IOperationResult>().Object);
            callback(CreateQuestionnaireResult());

            Assert.AreEqual("Normal", viewModel.CurrentState);
            CollectionAssert.Contains(tracker.ChangedProperties, "CanSubmit");
        }

        [TestMethod]
        public void AfterSubmitting_ANewQuestionnaireIsCreated()
        {
            QuestionViewModel[] originalQuestions = null;
            Action<IOperationResult<Questionnaire>> callback = null;
            Action<IOperationResult> submitCallback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);
            repository.Setup(r => r.SubmitQuestionnaireAsync(It.IsAny<Questionnaire>(), It.IsAny<Action<IOperationResult>>()))
                .Callback<Questionnaire, Action<IOperationResult>>(
                (q, sc) => submitCallback = sc);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            originalQuestions = viewModel.Questions.ToArray();

            viewModel.Questionnaire.Name = "TestName";
            viewModel.Questionnaire.Age = 14;
            AnswerAllQuestions(viewModel);

            viewModel.Submit();
            submitCallback(new Mock<IOperationResult>().Object);
            callback(CreateQuestionnaireResult());

            CollectionAssert.AreNotEquivalent(originalQuestions, viewModel.Questions.ToArray());
        }

        [TestMethod]
        public void AfterResetting_ThenANewQuestionnaireIsCreated()
        {
            QuestionViewModel[] originalQuestions = null;
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);
            callback(CreateQuestionnaireResult());

            originalQuestions = viewModel.Questions.ToArray();

            viewModel.Questionnaire.Name = "TestName";
            viewModel.Questionnaire.Age = 14;
            AnswerAllQuestions(viewModel);

            viewModel.Reset();

            var tracker = new PropertyChangeTracker(viewModel);
            callback(CreateQuestionnaireResult());

            CollectionAssert.AreNotEquivalent(originalQuestions, viewModel.Questions.ToArray());
            CollectionAssert.Contains(tracker.ChangedProperties, "CanSubmit");
        }

        [TestMethod]
        public void AfterSubmittingQuestionnaire_ThenChangeDataRelatedNotificationsAreFired()
        {
            Action<IOperationResult<Questionnaire>> callback = null;
            Action<IOperationResult> submitCallback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);
            repository.Setup(r => r.SubmitQuestionnaireAsync(It.IsAny<Questionnaire>(), It.IsAny<Action<IOperationResult>>()))
                .Callback<Questionnaire, Action<IOperationResult>>(
                (q, sc) => submitCallback = sc);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult());

            var tracker = new PropertyChangeTracker(viewModel);
            viewModel.Questionnaire.Name = "TestName";
            viewModel.Questionnaire.Age = 25;
            AnswerAllQuestions(viewModel);

            viewModel.Submit();
            submitCallback(new Mock<IOperationResult>().Object);
            callback(CreateQuestionnaireResult());

            Assert.IsTrue(tracker.ChangedProperties.Contains("Questionnaire"));
        }

        [TestMethod]
        public void WhenQuestionnaireHasOpenTextQuestion_ThenTheMatchingViewModelIsCreated()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(new Questionnaire(new OpenQuestion("Open"))));

            // Assertions
            Assert.IsInstanceOfType(viewModel.Questions[0], typeof(OpenQuestionViewModel));
        }

        [TestMethod]
        public void WhenQuestionnaireHasMultipleSelectionQuestion_ThenTheMatchingViewModelIsCreated()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(
                new Questionnaire(
                    new MultipleSelectionQuestion("Multiple selection", new[] { "a" })
                    )));

            // Assertions
            Assert.IsInstanceOfType(viewModel.Questions[0], typeof(MultipleSelectionQuestionViewModel));
        }

        [TestMethod]
        public void WhenQuestionnaireHasNumericQuestion_ThenTheMatchingViewModelIsCreated()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(new Questionnaire(new NumericQuestion("Numeric"))));

            // Assertions
            Assert.IsInstanceOfType(viewModel.Questions[0], typeof(NumericQuestionViewModel));
        }

        [TestMethod]
        public void WhenQuestionnaireHasUnknownQuestion_ThenAnExceptionIsThrown()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            try
            {
                callback(CreateQuestionnaireResult(new Questionnaire(new MockQuestion("unknown"))));
                Assert.Fail("Expected ArgumentException");
            }
            catch (ArgumentException)
            {
                // expected exception
            }
        }

        [TestMethod]
        public void WhenQuestionHasFormattingErrorAfterHavingValidResponse_ThenViewModelCannotBeSubmitted()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            callback(CreateQuestionnaireResult(new Questionnaire(new NumericQuestion("unknown"))));
            var questionViewModel = ((NumericQuestionViewModel)viewModel.Questions[0]);

            viewModel.Questionnaire.Name = "name";
            viewModel.Questionnaire.Age = 25;
            questionViewModel.Question.Response = 100;

            Assert.IsTrue(viewModel.CanSubmit);

            questionViewModel.HasBindingError = true;

            Assert.IsFalse(viewModel.CanSubmit);
        }

        [TestMethod]
        public void WhenGettingCalculatedPropertiesBeforeGettingAQuestionnaire_ThenReturnsDefaultValues()
        {
            Action<IOperationResult<Questionnaire>> callback = null;

            var repository = new Mock<IQuestionnaireRepository>();
            repository.Setup(r => r.GetQuestionnaireAsync(It.IsAny<Action<IOperationResult<Questionnaire>>>()))
                .Callback<Action<IOperationResult<Questionnaire>>>(c => callback = c);

            var viewModel = new QuestionnaireViewModel(repository.Object);

            Assert.AreEqual(0, viewModel.TotalQuestions);
            Assert.AreEqual(0, viewModel.UnansweredQuestions);
            Assert.IsFalse(viewModel.CanSubmit);
        }

        private static IOperationResult<Questionnaire> CreateQuestionnaireResult(Questionnaire questionnaire)
        {
            var result = new Mock<IOperationResult<Questionnaire>>();
            result.Setup(r => r.Result)
                .Returns(questionnaire);

            return result.Object;
        }

        private static IOperationResult<Questionnaire> CreateQuestionnaireResult()
        {
            return CreateQuestionnaireResult(CreateQuestionnaire());
        }

        private static Questionnaire CreateQuestionnaire()
        {
            var questionnaire = new Questionnaire(
                            new OpenQuestion("Enter your name") { MaxLength = 100 },
                            new OpenQuestion("Enter your address") { MaxLength = 100 }
            );

            return questionnaire;
        }

        private static void AnswerAllQuestions(QuestionnaireViewModel viewModel)
        {
            foreach (OpenQuestionViewModel questionViewModel in viewModel.Questions)
            {
                questionViewModel.Question.Response = "response";
            }
        }
    }
}
