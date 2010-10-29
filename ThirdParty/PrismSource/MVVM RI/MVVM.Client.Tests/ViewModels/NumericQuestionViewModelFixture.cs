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
using MVVM.Client.ViewModels;
using MVVM.Questionnaires.Model;

namespace MVVM.Client.Tests.ViewModels
{
    [TestClass]
    public class NumericQuestionViewModelFixture
    {
        [TestMethod]
        public void WhenCreatingAViewModelWithANullQuestion_ThenAnExceptionIsThrown()
        {
            try
            {
                new NumericQuestionViewModel(null);
                Assert.Fail("should have thrown");
            }
            catch (ArgumentNullException)
            {
                // expected
            }
        }

        [TestMethod]
        public void WhenResponseChangesOnTheModel_ThenResponseValueChangeIsFired()
        {
            var question = new NumericQuestionTemplate { MaxValue = 100 }.CreateNewQuestion() as NumericQuestion;
            var viewModel = new NumericQuestionViewModel(question);

            bool responseChangeFired = false;
            viewModel.ResponseChanged += (s, e) => { responseChangeFired = true; };
            question.Response = 15;

            // Assertions
            Assert.IsTrue(responseChangeFired);
        }

        [TestMethod]
        public void WhenHasErrorIsChangedToTrueOnTheViewModel_ThenResponseValueChangeIsFired()
        {
            var question = new NumericQuestionTemplate { MaxValue = 100 }.CreateNewQuestion() as NumericQuestion;
            var viewModel = new NumericQuestionViewModel(question);

            viewModel.HasBindingError = false;
            bool responseChangeFired = false;
            viewModel.ResponseChanged += (s, e) => { responseChangeFired = true; };
            viewModel.HasBindingError = true;

            // Assertions
            Assert.IsTrue(responseChangeFired);
        }

        [TestMethod]
        public void WhenHasErrorIsChangedToFalseOnTheViewModel_ThenResponseValueChangeIsFired()
        {
            var question = new NumericQuestionTemplate { MaxValue = 100 }.CreateNewQuestion() as NumericQuestion;
            var viewModel = new NumericQuestionViewModel(question);

            viewModel.HasBindingError = true;
            bool responseChangeFired = false;
            viewModel.ResponseChanged += (s, e) => { responseChangeFired = true; };
            viewModel.HasBindingError = false;

            // Assertions
            Assert.IsTrue(responseChangeFired);
        }

        [TestMethod]
        public void WhenResponseIsSetOnTheModel_ThenTheHasErrorPropertyInTheViewModelIsCleared()
        {
            var question = new NumericQuestionTemplate { MaxValue = 100 }.CreateNewQuestion() as NumericQuestion;
            var viewModel = new NumericQuestionViewModel(question);

            viewModel.HasBindingError = true;
            int responseChanges = 0;
            viewModel.ResponseChanged += (s, e) => { responseChanges++; };
            question.Response = 15;

            // Assertions
            Assert.IsFalse(viewModel.HasBindingError);
            Assert.AreEqual(1, responseChanges);
        }

        [TestMethod]
        public void WhenCreatingANewViewModel_ThenHasChangesIsFalse()
        {
            var question = new NumericQuestionTemplate { MaxValue = 100 }.CreateNewQuestion() as NumericQuestion;
            var viewModel = new NumericQuestionViewModel(question);

            // Assertions
            Assert.IsFalse(viewModel.HasChanges);
        }

        [TestMethod]
        public void WhenResponseIsSetOnTheModel_ThenHasChangesIsTrue()
        {
            var question = new NumericQuestionTemplate { MaxValue = 100 }.CreateNewQuestion() as NumericQuestion;
            var viewModel = new NumericQuestionViewModel(question);

            question.Response = 15;

            // Assertions
            Assert.IsTrue(viewModel.HasChanges);
        }

        [TestMethod]
        public void WhenViewModelHasErrorIsSet_ThenDoesNotHaveCompletedResponse()
        {
            var question = new NumericQuestionTemplate { MaxValue = 100 }.CreateNewQuestion() as NumericQuestion;
            var viewModel = new NumericQuestionViewModel(question);

            question.Response = 15;
            viewModel.HasBindingError = true;

            // Assertions
            Assert.IsFalse(viewModel.ResponseComplete);
        }
    }
}
