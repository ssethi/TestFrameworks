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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Client.Infrastructure;
using MVVM.Client.Infrastructure.StateManagement;
using MVVM.Client.Tests.TestObjects;

namespace MVVM.Client.Tests.Infrastructure
{
    [TestClass]
    public class UIServiceFixture
    {
        [TestMethod]
        public void ServiceProvidesAMainView()
        {
            var container =
                new CompositionContainer(
                        new TypeCatalog(typeof(TestUIService)));
            container.ComposeExportedValue(new ViewFactory(container));
            container.ComposeExportedValue(new StateHandler(container));

            var service = container.GetExportedValue<ISingleViewUIService>();

            Assert.IsNotNull(service.MainWindow);
        }

        [TestMethod]
        public void ServiceProvidesTheSameMainViewEachTime()
        {
            var container =
                new CompositionContainer(
                        new TypeCatalog(typeof(TestUIService)));
            container.ComposeExportedValue(new ViewFactory(container));
            container.ComposeExportedValue(new StateHandler(container));


            var service = container.GetExportedValue<ISingleViewUIService>();

            Assert.AreSame(service.MainWindow, service.MainWindow);
        }

        [TestMethod]
        public void WhenGivenAviewName_ThenServiceDiscoversViewAndSetsViewModel()
        {
            var container =
                new CompositionContainer(
                    new TypeCatalog(
                        typeof(TestUIService),
                        typeof(TestView),
                        typeof(TestViewModel)));
            container.ComposeExportedValue(new ViewFactory(container));
            container.ComposeExportedValue(new StateHandler(container));


            var service = container.GetExportedValue<ISingleViewUIService>();

            service.ShowView("TestView");

            var currentView = service.MainWindow.CurrentView as TestView;

            Assert.IsNotNull(currentView);
            Assert.IsNotNull(currentView.DataContext);
            Assert.IsInstanceOfType(currentView.DataContext, typeof(TestViewModel));
        }

        [TestMethod]
        public void WhenGivenAviewNameAndAContext_ThenServiceDiscoversViewAndSetsViewModelWithContext()
        {
            var container =
                new CompositionContainer(
                    new TypeCatalog(
                        typeof(TestUIService),
                        typeof(TestViewWithContext),
                        typeof(TestViewModelWithContext),
                        typeof(TestCurrentState),
                        typeof(TestState)));
            container.ComposeExportedValue(new ViewFactory(container));
            container.ComposeExportedValue(new StateHandler(container));


            var service = container.GetExportedValue<ISingleViewUIService>();

            service.ShowView<string>("TestViewWithContext", "context");

            var currentView = service.MainWindow.CurrentView as TestViewWithContext;

            Assert.IsNotNull(currentView);
            Assert.IsNotNull(currentView.DataContext);
            Assert.IsInstanceOfType(currentView.DataContext, typeof(TestViewModelWithContext));
            Assert.AreEqual("context", ((TestViewModelWithContext)currentView.DataContext).State.Value);
        }

        [TestMethod]
        public void WhenGivenAviewNameAndAContext_ThenServiceRestoresCurrentState()
        {
            var container =
                new CompositionContainer(
                    new TypeCatalog(
                        typeof(TestUIService),
                        typeof(TestViewWithContext),
                        typeof(TestViewModelWithContext),
                        typeof(TestCurrentState),
                        typeof(TestState)));
            container.ComposeExportedValue(new ViewFactory(container));
            container.ComposeExportedValue(new StateHandler(container));


            var service = container.GetExportedValue<ISingleViewUIService>();

            var currentState = container.GetExportedValue<ICurrentState<string>>();

            currentState.Value = "initial";

            service.ShowView<string>("TestViewWithContext", "context");

            var currentView = service.MainWindow.CurrentView as TestViewWithContext;

            Assert.AreEqual("initial", container.GetExportedValue<IState<string>>().Value);
        }
    }
}
