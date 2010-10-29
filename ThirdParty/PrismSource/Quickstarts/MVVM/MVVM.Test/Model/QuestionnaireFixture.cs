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
using System.Linq;
using MVVM.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MVVM.Test
{
    [TestClass]
    public class QuestionnaireFixture
    {
        [TestMethod]
        public void WhenCreated_TheQuestionnaireIsNotComplete()
        {
            var questionnaire = new Questionnaire();

            Assert.IsFalse(questionnaire.IsComplete);
        }

        [TestMethod]
        public void WhenNameAndAgeAreSetToValidValues_TheQuestionnaireIsComplete()
        {
            var questionnaire = new Questionnaire();

            questionnaire.Name = "name";
            questionnaire.Age = 25;

            Assert.IsTrue(questionnaire.IsComplete);
        }

        [TestMethod]
        public void WhenAgeIsSetToInvalidValue_ThenQuestionnaireIsNotComplete()
        {
            var questionnaire = new Questionnaire() { MaxAge = 100 };

            questionnaire.Name = "name";
            questionnaire.Age = 200;

            Assert.IsFalse(questionnaire.IsComplete);
        }
    }
}
