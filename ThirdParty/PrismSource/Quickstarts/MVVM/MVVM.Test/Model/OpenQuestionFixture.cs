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
    public class OpenQuestionFixture
    {
        [TestMethod]
        public void WhenQuestionIsNew_ThenItIsNotComplete()
        {
            var question = new OpenQuestion { MaxLength = 10 };
            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionIsNew_ThenTemplateValuesAreCopied()
        {
            var question = new OpenQuestion { MaxLength = 10 };
            Assert.AreEqual(10, question.MaxLength);
        }

        [TestMethod]
        public void WhenQuestionHasInvalidValue_ThenItIsNotComplete()
        {
            var question = new OpenQuestion { MaxLength = 10 };
            question.Response = "aaaaaaaaaaaaaaaaaaaaaaaaa";

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasEmptyValue_ThenItIsNotComplete()
        {
            var question = new OpenQuestion { MaxLength = 10 };
            question.Response = "";

            Assert.IsFalse(question.IsComplete);
        }

        [TestMethod]
        public void WhenQuestionHasValidValue_ThenItIsComplete()
        {
            var question = new OpenQuestion { MaxLength = 10 };
            question.Response = "aaaa";

            Assert.IsTrue(question.IsComplete);
        }

        [TestMethod]
        public void WhenResponseLengthExceedsMaxLength_ThenIndicatesErrorOnResponse()
        {
            var question = new OpenQuestion { MaxLength = 10 };
            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = "ThisIsLongerThanTenCharacters";

            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<string>().Any());
        }

        [TestMethod]
        public void WhenSettingResponseToNull_ThenIndicatesErrorOnResponse()
        {
            var question = new OpenQuestion { MaxLength = 10 };
            var notifyErrorInfo = (INotifyDataErrorInfo)question;

            question.Response = "valid";
            Assert.IsFalse(notifyErrorInfo.GetErrors("Response").Cast<string>().Any());

            question.Response = null;
            Assert.IsTrue(notifyErrorInfo.GetErrors("Response").Cast<string>().Any());
        }
    }
}
