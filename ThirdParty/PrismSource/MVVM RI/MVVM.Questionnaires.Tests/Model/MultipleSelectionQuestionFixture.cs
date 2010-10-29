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
    public class MultipleSelectionQuestionFixture
    {
        [TestMethod]
        public void WhenQuestionIsNew_ThenItIsNotComplete()
        {
            var template = new MultipleSelectionQuestionTemplate()
                               {
                                   Range = new[] { "one", "two", "three" },
                                   MaxSelections = 2
                               };
            var question = template.CreateNewQuestion();

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionIsCreated_ThenTemplateValuesAreCopied()
        {
            var theRange = new string[] { "aRangeValue", "anotherRangeValue" };
            var maxSelections = 1;
            var question = new MultipleSelectionQuestionTemplate() { Range = theRange, MaxSelections = maxSelections }.CreateNewQuestion() as MultipleSelectionQuestion;

            Assert.AreEqual(theRange, question.Range);
            Assert.AreEqual(maxSelections, question.MaxSelections);
        }

        [TestMethod]
        public void WhenQuestionHasInvalidValue_ThenItIsNotComplete()
        {
            var template = new MultipleSelectionQuestionTemplate()
                               {
                                   Range = new[] { "one", "two", "three" },
                                   MaxSelections = 2
                               };
            var question = template.CreateNewQuestion() as MultipleSelectionQuestion;
            question.Response = new[] { "one", "two", "three" };

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasNullValue_ThenItIsNotComplete()
        {
            var template = new MultipleSelectionQuestionTemplate()
                               {
                                   Range = new[] { "one", "two", "three" },
                                   MaxSelections = 2
                               };
            var question = template.CreateNewQuestion() as MultipleSelectionQuestion;

            question.Response = new[] { "one" };
            question.Response = null;

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasValidValue_ThenItIsComplete()
        {
            var template = new MultipleSelectionQuestionTemplate()
                    {
                        Range = new[] { "one", "two", "three" },
                        MaxSelections = 2
                    };
            var question = template.CreateNewQuestion() as MultipleSelectionQuestion;

            question.Response = new[] { "one" };

            Assert.IsTrue(question.IsComplete);
        }

        [TestMethod]
        public void WhenResponseHasLessElementsThanMax_ThenIndicatesNoErrorOnResponse()
        {
            var template = new MultipleSelectionQuestionTemplate()
                               {
                                   Range = new[] { "one", "two", "three" },
                                   MaxSelections = 2
                               };
            var question = template.CreateNewQuestion() as MultipleSelectionQuestion;

            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = new[] { "one" };

            Assert.IsFalse(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());
        }

        [TestMethod]
        public void WhenResponseHasMoreElementsThanMax_ThenIndicatesErrorOnResponse()
        {
            var template = new MultipleSelectionQuestionTemplate()
                               {
                                   Range = new[] { "one", "two", "three" },
                                   MaxSelections = 2
                               };
            var question = template.CreateNewQuestion() as MultipleSelectionQuestion;

            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = new[] { "one", "two", "three" };

            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());
        }

        [TestMethod]
        public void WhenSettingResponseToNull_ThenIndicatesErrorOnResponse()
        {
            var template = new MultipleSelectionQuestionTemplate()
                               {
                                   Range = new[] { "one", "two", "three" },
                                   MaxSelections = 2
                               };
            var question = template.CreateNewQuestion() as MultipleSelectionQuestion;

            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = new[] { "one" };
            Assert.IsFalse(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());

            question.Response = null;
            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<ValidationResult>().Any());
        }
    }
}
