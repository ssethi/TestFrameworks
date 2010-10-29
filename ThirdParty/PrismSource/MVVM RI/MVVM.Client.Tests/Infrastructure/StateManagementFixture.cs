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
using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Client.Infrastructure.StateManagement;
using MVVM.Client.Tests.TestObjects;

namespace MVVM.Client.Tests.Infrastructure
{
    [TestClass]
    public class StateManagementFixture
    {
        [TestMethod]
        public void CurrentStateIsShared()
        {
            var container =
                new CompositionContainer(
                    new AggregateCatalog(
                        new AssemblyCatalog(typeof(ICurrentState<>).Assembly),
                        new TypeCatalog(typeof(TestCurrentState))));

            var currentState = container.GetExportedValue<ICurrentState<string>>();

            Assert.IsInstanceOfType(currentState, typeof(TestCurrentState));
            Assert.AreSame(currentState, container.GetExportedValue<ICurrentState<string>>());
        }

        [TestMethod]
        public void StateIsNotShared()
        {
            var container =
                new CompositionContainer(
                    new AggregateCatalog(
                        new AssemblyCatalog(typeof(ICurrentState<>).Assembly),
                        new TypeCatalog(typeof(TestCurrentState), typeof(TestState))));

            var state = container.GetExportedValue<IState<string>>();

            Assert.IsInstanceOfType(state, typeof(TestState));
            Assert.AreNotSame(state, container.GetExportedValue<IState<string>>());
        }

        [TestMethod]
        public void StateCapturesTheCurrentState()
        {
            var container =
                new CompositionContainer(
                    new AggregateCatalog(
                        new AssemblyCatalog(typeof(ICurrentState<>).Assembly),
                        new TypeCatalog(typeof(TestCurrentState), typeof(TestState))));

            var currentState = container.GetExportedValue<ICurrentState<string>>();

            currentState.Value = "value";

            var state = container.GetExportedValue<IState<string>>();

            Assert.AreEqual("value", state.Value);
        }

        [TestMethod]
        public void StateCapturesTheCurrentStateAndWillNotChangeItIfCurrentStateChanges()
        {
            var container =
                new CompositionContainer(
                    new AggregateCatalog(
                        new AssemblyCatalog(typeof(ICurrentState<>).Assembly),
                        new TypeCatalog(typeof(TestCurrentState), typeof(TestState))));

            var currentState = container.GetExportedValue<ICurrentState<string>>();

            currentState.Value = "value";

            var state = container.GetExportedValue<IState<string>>();

            currentState.Value = "new value";

            Assert.AreEqual("value", state.Value);
        }
    }
}
