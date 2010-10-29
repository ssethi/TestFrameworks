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
using System.Linq;
using MVVM.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MVVM.Test.Model
{
    [TestClass]
    public class MultipleSelectionQuestionFixture
    {
        [TestMethod]
        public void WhenQuestionIsNew_ThenItIsNotComplete()
        {
            var question =
                new MultipleSelectionQuestion()
                {
                    Range = new[] { "one", "two", "three" },
                    MaxSelections = 2
                };

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasInvalidValue_ThenItIsNotComplete()
        {
            var question =
                new MultipleSelectionQuestion()
                {
                    Range = new[] { "one", "two", "three" },
                    MaxSelections = 2
                };
            question.Response = new[] { "one", "two", "three" };

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasNullValue_ThenItIsNotComplete()
        {
            var question =
                new MultipleSelectionQuestion()
                {
                    Range = new[] { "one", "two", "three" },
                    MaxSelections = 2
                };

            question.Response = new[] { "one" };
            question.Response = null;

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasValidValue_ThenItIsComplete()
        {
            var question =
                new MultipleSelectionQuestion()
                {
                    Range = new[] { "one", "two", "three" },
                    MaxSelections = 2
                };

            question.Response = new[] { "one" };

            Assert.IsTrue(question.IsComplete);
        }

        [TestMethod]
        public void WhenResponseHasLessElementsThanMax_ThenIndicatesNoErrorOnResponse()
        {
            var question =
                new MultipleSelectionQuestion()
                {
                    Range = new[] { "one", "two", "three" },
                    MaxSelections = 2
                };

            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = new[] { "one" };

            Assert.IsFalse(notifyErrorInfo.GetErrors("Response").Cast<string>().Any());
        }

        [TestMethod]
        public void WhenResponseHasMoreElementsThanMax_ThenIndicatesErrorOnResponse()
        {
            var question =
                new MultipleSelectionQuestion()
                {
                    Range = new[] { "one", "two", "three" },
                    MaxSelections = 2
                };

            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = new[] { "one", "two", "three" };

            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<string>().Any());
        }

        [TestMethod]
        public void WhenSettingResponseToNull_ThenIndicatesErrorOnResponse()
        {
            var question =
                new MultipleSelectionQuestion()
                {
                    Range = new[] { "one", "two", "three" },
                    MaxSelections = 2
                };

            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = new[] { "one" };
            Assert.IsFalse(notifyErrorInfo.GetErrors("Response").Cast<string>().Any());

            question.Response = null;
            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<string>().Any());
        }
    }
}
