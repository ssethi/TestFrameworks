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
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM.Client.Infrastructure;

namespace MVVM.Client.Tests.Infrastructure
{
    [TestClass]
    public class ViewFactoryFixture
    {
        [TestMethod]
        public void WhenViewRequested_ThenReturnsFromContainer()
        {
            var catalog = new TypeCatalog(typeof(TestView));
            var container = new CompositionContainer(catalog);

            var viewFactory = new ViewFactory(container);

            var view = viewFactory.GetView("TestView");
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void WhenNonExistingVeiwRequested_ThenThrowsException()
        {
            var catalog = new TypeCatalog(typeof(TestView));
            var container = new CompositionContainer(catalog);

            var viewFactory = new ViewFactory(container);

            ExceptionAssert.Throws<Exception>(
                () =>
                {
                    viewFactory.GetView("NonExistentView");
                }
            );
        }

        [ExportView("TestView")]
        public class TestView : UserControl
        {
        }
    }



}
