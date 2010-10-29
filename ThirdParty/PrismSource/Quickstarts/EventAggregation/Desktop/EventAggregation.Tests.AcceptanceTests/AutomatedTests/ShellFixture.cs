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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventAggregation.AcceptanceTests.TestInfrastructure;
using EventAggregation.AcceptanceTests.ApplicationObserver;
using Core.UIItems.ListBoxItems;
using Core.UIItems;
using Core.UIItems.Finders;

namespace EventAggregation.AcceptanceTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    [DeploymentItem(@".\bin\Debug")]
    [DeploymentItem(@".\EventAggregation.Tests.AcceptanceTests\bin\Debug")]
    public class ShellFixture : FixtureBase
    {
        #region Additional test attributes
        
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            // Check whether any exception occured during previous application launches. 
            // If so, fail the test case.
            if (StateDiagnosis.IsFailed)
            {
                Assert.Fail(TestDataInfrastructure.GetTestInputData("ApplicationLoadFailure"));
            }

            base.TestInitialize();
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            base.TestCleanup();
        }
        
        #endregion

        [TestMethod]
        public void ApplicationLaunch()
        {
            //check if the controls expected to be loaded on the Shell window is loaded properly, as expected
            ComboBox customerCombobox = Window.Get<ComboBox>(TestDataInfrastructure.GetControlId("CustomerCombobox"));
            Assert.IsNotNull(customerCombobox);

            ComboBox fundCombobox = Window.Get<ComboBox>(TestDataInfrastructure.GetControlId("FundCombobox"));
            Assert.IsNotNull(fundCombobox);

            Button addButton = Window.Get<Button>(TestDataInfrastructure.GetControlId("AddButton"));
            Assert.IsNotNull(addButton);

            Label activity1Label = (Label)Window.Get(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("ActivityLabel"))
                                                            .AndByText(TestDataInfrastructure.GetTestInputData("Customer1ActivityLabelText"))
                                                            .AndControlType(typeof(Label)));
            Assert.IsNotNull(activity1Label);

            Label activity2Label = (Label)Window.Get(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("ActivityLabel"))
                                                            .AndByText(TestDataInfrastructure.GetTestInputData("Customer2ActivityLabelText"))
                                                            .AndControlType(typeof(Label)));
            Assert.IsNotNull(activity2Label);
        }
    }
}
