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
using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Client.Infrastructure.StateManagement;

namespace MVVM.Client.Tests.Infrastructure
{
    [TestClass]
    public class StateHandlerFixture
    {
        [TestMethod]
        public void WhenSettingState_SetsStateValueAppropriately()
        {
            var catalog = new TypeCatalog(typeof(CurrentStringState), typeof(StringState));
            var container = new CompositionContainer(catalog);

            var stateHandler = new StateHandler(container);

            var previousState = stateHandler.SetState<string>("FirstString");

            Assert.IsTrue(string.IsNullOrEmpty(previousState));

            previousState = stateHandler.SetState<string>("SecondState");
            Assert.AreEqual("FirstString", previousState);
        }

        [TestMethod]
        public void WhenSettingOnMissingContext_ThenThrows()
        {
            var catalog = new TypeCatalog(typeof(CurrentStringState), typeof(StringState));
            var container = new CompositionContainer(catalog);

            var stateHandler = new StateHandler(container);

            ExceptionAssert.Throws<Exception>(
                () =>
                {
                    stateHandler.SetState<Uri>(new Uri("http://localhost"));
                }
                );
        }

        public class CurrentStringState : CurrentState<string>
        {
        }

        public class StringState : State<string>
        {

        }
    }
}
