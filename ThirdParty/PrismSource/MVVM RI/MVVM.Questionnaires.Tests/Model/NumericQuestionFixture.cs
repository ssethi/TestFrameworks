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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Questionnaires.Model;

namespace MVVM.Questionnaires.Tests.Model
{
    [TestClass]
    public class NumericQuestionFixture
    {
        [TestMethod]
        public void WhenQuestionIsNew_ThenItIsNotComplete()
        {
            var question = new NumericQuestionTemplate() { }.CreateNewQuestion() as NumericQuestion;

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasInvalidValue_ThenItIsNotComplete()
        {
            var question = new NumericQuestionTemplate() { MaxValue = 15 }.CreateNewQuestion() as NumericQuestion;
            question.Response = 20;

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasNegativeValue_ThenItIsNotComplete()
        {
            var question = new NumericQuestionTemplate() { MaxValue = 15 }.CreateNewQuestion() as NumericQuestion;
            question.Response = -1;

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasNullValue_ThenItIsNotComplete()
        {
            var question = new NumericQuestionTemplate() { }.CreateNewQuestion() as NumericQuestion;
            question.Response = 10;
            question.Response = null;

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasValidValue_ThenItIsComplete()
        {
            var question = new NumericQuestionTemplate() { }.CreateNewQuestion() as NumericQuestion;
            question.Response = 10;

            Assert.IsTrue(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionIsCreated_ThenTemplateValuesAreCopied()
        {
            var question = new NumericQuestionTemplate() { MaxValue = 35 }.CreateNewQuestion() as NumericQuestion;
            Assert.AreEqual(35, question.MaxValue);
        }

        [TestMethod]
        public void WhenResponseIsNegative_ThenIndicatesErrorOnResponse()
        {
            var question = new NumericQuestionTemplate() { }.CreateNewQuestion() as NumericQuestion;
            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = -15;

            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());
        }

        [TestMethod]
        public void WhenResponseIsPositiveWithNoMaximum_ThenIndicatesNoErrorOnResponse()
        {
            var question = new NumericQuestionTemplate() { }.CreateNewQuestion() as NumericQuestion;
            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = 15;

            Assert.IsFalse(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());
        }

        [TestMethod]
        public void WhenResponseIsPositiveBelowMaximum_ThenIndicatesNoErrorOnResponse()
        {
            var question = new NumericQuestionTemplate() { MaxValue = 20 }.CreateNewQuestion() as NumericQuestion;
            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = 15;

            Assert.IsFalse(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());
        }

        [TestMethod]
        public void WhenResponseIsPositiveOverMaximum_ThenIndicatesErrorOnResponse()
        {
            var question = new NumericQuestionTemplate() { MaxValue = 10 }.CreateNewQuestion() as NumericQuestion;
            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = 15;

            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());
        }

        [TestMethod]
        public void WhenSettingResponseToNull_ThenIndicatesErrorOnResponse()
        {
            var question = new NumericQuestionTemplate() { }.CreateNewQuestion() as NumericQuestion;
            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = 10;
            Assert.IsFalse(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());

            question.Response = null;
            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());
        }
    }
}
