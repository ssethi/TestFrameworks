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
using System.Collections;
using System.ComponentModel;
using System.Linq;
using MVVM.Model;
using MVVM.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MVVM.Test.ViewModels
{
    [TestClass]
    public class MultipleSelectionQuestionViewModelFixture
    {
        [TestMethod]
        public void WhenCreatingAViewModelWithANullQuestionThenAnExceptionIsThrown()
        {
            try
            {
                new MultipleSelectionQuestionViewModel(null);
                Assert.Fail("should have thrown");
            }
            catch(ArgumentException)
            {
                // expected
            }
        }

        [TestMethod]
        public void WhenCreatingAViewModelForAQuestionWithNullResponse_ThenInitializesTheResponseToAnEmptyCollection()
        {
            var question = new MultipleSelectionQuestion { Range = new[] { "a", "b", "c" } };
            var viewModel = new MultipleSelectionQuestionViewModel(question);

            CollectionAssert.AreEqual(new string[0], question.Response.ToArray());
        }

        [TestMethod]
        public void WhenUpdatingTheElementsInTheSelectionsListInTheViewModel_ThenResponseIsSetInTheModelAndValidationIsUpdated()
        {
            var question = new MultipleSelectionQuestion { Range = new[] { "a", "b", "c" }, MaxSelections = 2 };
            var questionAsINDEI = question as INotifyDataErrorInfo;
            var viewModel = new MultipleSelectionQuestionViewModel(question);

            viewModel.Selections.Add("a");

            CollectionAssert.AreEqual((ICollection)viewModel.Selections, question.Response.ToArray());
            Assert.IsFalse(questionAsINDEI.HasErrors);

            viewModel.Selections.Add("b");

            CollectionAssert.AreEqual((ICollection)viewModel.Selections, question.Response.ToArray());
            Assert.IsFalse(questionAsINDEI.HasErrors);

            viewModel.Selections.Add("c");

            CollectionAssert.AreEqual((ICollection)viewModel.Selections, question.Response.ToArray());
            Assert.IsTrue(questionAsINDEI.HasErrors);

            viewModel.Selections.Remove("a");

            CollectionAssert.AreEqual((ICollection)viewModel.Selections, question.Response.ToArray());
            Assert.IsFalse(questionAsINDEI.HasErrors);
        }

        [TestMethod]
        public void WhenUpdatingTheCollectionInitializedByTheViewModel_ThenTheViewModelNotifiesChangesToTheResponse()
        {
            var question = new MultipleSelectionQuestion { Range = new[] { "a", "b", "c" }, MaxSelections = 2 };
            var viewModel = new MultipleSelectionQuestionViewModel(question);
            int responseChanges = 0;
            viewModel.ResponseChanged += (s, e) => responseChanges++;

            viewModel.Selections.Add("a");

            Assert.AreEqual(1, responseChanges);

            viewModel.Selections.Add("b");

            Assert.AreEqual(2, responseChanges);

            viewModel.Selections.Add("c");

            Assert.AreEqual(3, responseChanges);

            viewModel.Selections.Remove("b");

            Assert.AreEqual(4, responseChanges);
        }

        [TestMethod]
        public void WhenCreatingTheViewModel_ThenHasChangesIsFalse()
        {
            var question = new MultipleSelectionQuestion { Range = new[] { "a", "b", "c" }, MaxSelections = 2 };
            var viewModel = new MultipleSelectionQuestionViewModel(question);

            bool hasChanges = false;
            viewModel.ResponseChanged += (o, e) => { hasChanges = true; };            
            Assert.IsFalse(hasChanges);
        }

        [TestMethod]
        public void WhenSettingASelectionOnTheViewModel_ThenHasChangesIsTrue()
        {
            var question = new MultipleSelectionQuestion { Range = new[] { "a", "b", "c" }, MaxSelections = 2 };
            var viewModel = new MultipleSelectionQuestionViewModel(question);
            int responseChanges = 0;
            viewModel.ResponseChanged += (s, e) => responseChanges++;

            viewModel.Selections.Add("a");

            Assert.IsTrue(responseChanges > 0);
        }
    }
}
