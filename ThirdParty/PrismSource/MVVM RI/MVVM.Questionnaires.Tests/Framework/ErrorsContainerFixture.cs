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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Questionnaires.Framework;

namespace MVVM.Questionnaires.Tests.Framework
{
    [TestClass]
    public class ErrorsContainerFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingAnInstanceWithANullAction_ThenAnExceptionIsThrown()
        {
            new ErrorsContainer<object>(null);
        }

        [TestMethod]
        public void WhenCreatingInstance_ThenHasNoErrors()
        {
            var validation = new ErrorsContainer<ValidationResult>(pn => { });

            Assert.IsFalse(validation.HasErrors);
            Assert.IsFalse(validation.GetErrors("property1").Any());
        }

        [TestMethod]
        public void WhenSettingErrorsForPropertyWithNoErrors_ThenNotifiesChangesAndHasErrors()
        {
            List<string> validatedProperties = new List<string>();

            var validation = new ErrorsContainer<ValidationResult>(pn => validatedProperties.Add(pn));

            var validationError = new ValidationResult("message", new[] { "property1" });
            validation.SetErrors("property1", new[] { validationError });

            Assert.IsTrue(validation.HasErrors);
            Assert.IsTrue(validation.GetErrors("property1").Contains(validationError));
            CollectionAssert.AreEqual(new[] { "property1" }, validatedProperties);
        }

        [TestMethod]
        public void WhenSettingNoErrorsForPropertyWithNoErrors_ThenDoesNotNotifyChangesAndHasNoErrors()
        {
            List<string> validatedProperties = new List<string>();

            var validation = new ErrorsContainer<ValidationResult>(pn => validatedProperties.Add(pn));

            validation.SetErrors("property1", new ValidationResult[0]);

            Assert.IsFalse(validation.HasErrors);
            Assert.IsFalse(validation.GetErrors("property1").Any());
            Assert.IsFalse(validatedProperties.Any());
        }

        [TestMethod]
        public void WhenSettingErrorsForPropertyWithErrors_ThenNotifiesChangesAndHasErrors()
        {
            List<string> validatedProperties = new List<string>();

            var validation = new ErrorsContainer<ValidationResult>(pn => validatedProperties.Add(pn));
            var validationError = new ValidationResult("message", new[] { "property1" });

            validation.SetErrors("property1", new[] { validationError });

            validatedProperties.Clear();

            validation.SetErrors("property1", new[] { validationError });

            Assert.IsTrue(validation.HasErrors);
            Assert.IsTrue(validation.GetErrors("property1").Contains(validationError));
            CollectionAssert.AreEqual(new[] { "property1" }, validatedProperties);
        }

        [TestMethod]
        public void WhenSettingNoErrorsForPropertyWithErrors_ThenNotifiesChangesAndHasNoErrors()
        {
            List<string> validatedProperties = new List<string>();

            var validation = new ErrorsContainer<ValidationResult>(pn => validatedProperties.Add(pn));
            var validationError = new ValidationResult("message", new[] { "property1" });

            validation.SetErrors("property1", new[] { validationError });

            validatedProperties.Clear();

            validation.SetErrors("property1", new ValidationResult[0]);

            Assert.IsFalse(validation.HasErrors);
            Assert.IsFalse(validation.GetErrors("property1").Any());
            CollectionAssert.AreEqual(new[] { "property1" }, validatedProperties);
        }
    }
}
