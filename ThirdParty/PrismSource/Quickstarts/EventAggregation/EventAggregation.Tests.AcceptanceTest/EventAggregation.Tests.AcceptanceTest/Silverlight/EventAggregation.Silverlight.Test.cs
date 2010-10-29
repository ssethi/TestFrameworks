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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.ApplicationHelper;
using System.IO;
using AcceptanceTestLibrary.Common.Silverlight;
using EventAggregation.Tests.AcceptanceTest.TestEntities.Page;
using EventAggregation.Tests.AcceptanceTest.TestEntities.Assertion;
using AcceptanceTestLibrary.TestEntityBase;
using System.Reflection;
using System.Threading;

namespace EventAggregation.Tests.AcceptanceTest.Silverlight
{
    /// <summary>
    /// Summary description for EventAggregation
    /// </summary>
#if DEBUG
    [DeploymentItem(@"..\Silverlight\bin\Debug", "SL")]
    [DeploymentItem(@".\EventAggregation.Tests.AcceptanceTest\bin\Debug")]
#else
    [DeploymentItem(@"..\Silverlight\bin\Release", "SL")]
    [DeploymentItem(@".\EventAggregation.Tests.AcceptanceTest\bin\Release")]
#endif

    [TestClass]
    public class EventAggregation_Silverlight_Tests : FixtureBase<SilverlightAppLauncher>
    {
        private const int BACKTRACKLENGTH = 4;

        #region Additional test attributes
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            EventAggregationPage<SilverlightAppLauncher>.Window = base.LaunchApplication(currentOutputPath + GetSilverlightApplication(), GetBrowserTitle())[0];
            Thread.Sleep(5000);   
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PageBase<SilverlightAppLauncher>.DisposeWindow();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());
        }

        #endregion

        #region Test Methods
        /// <summary>
        /// Tests if the Silverlight EA application is launched.
        /// </summary>
        [TestMethod]
        public void SilverlightEALaunchTest()
        {
            Assert.IsNotNull(EventAggregationPage<SilverlightAppLauncher>.Window, "EventAggregation application is not launched.");
        }

        /// <summary>
        /// Check if the add button click adds the selected fund to the selected customer.
        /// 
        /// Repro Steps:
        /// 1. Launch the EventAggregation QS
        /// 2. Select a customer in the Customer combo box.
        /// 3. Select a fund in the Fund combo box.
        /// 4. Click on the add button
        /// 5. Check for the content displayed in the ArticleView displayed on the right side of the screen.
        /// 
        /// Expected Result:
        /// On clicking the Add button, the selected fund should be added to the Activity view of the
        /// selected customer.        
        /// </summary>
        [TestMethod]
        public void SilverlightAddFundToCustomer()
        {
            //Assert AddFund To Customer
            EventAggregationAssertion<SilverlightAppLauncher>.AssertAddFundToCustomer();
        }

        /// <summary>
        /// Check if the add button click adds the selected fund to the selected customer.
        /// 
        /// Repro Steps:
        /// 1. Launch the EventAggregation QS
        /// 2. Select a customer in the Customer combo box.
        /// 3. Select a fund in the Fund combo box.
        /// 4. Click on the add button
        /// 5. Select another fund in Fund combo box
        /// 6. Click on the add button
        /// 7. Check for the content displayed in the ArticleView displayed on the right side of the screen.
        /// 
        /// Expected Result:
        /// On clicking the Add button, the selected fund should be added to the Activity view of the
        /// selected customer.        
        /// </summary>
        [TestMethod]
        public void SilverlightAddMultipleFundsToCustomer()
        {
            //Assert AddFund To Customer
            EventAggregationAssertion<SilverlightAppLauncher>.AssertAddMultipleFundsToCustomer();
        }
        #endregion

        #region private Methods

        private static string GetSilverlightApplication()
        {
            return ConfigHandler.GetValue("SilverlightAppLocation");
        }

        private static string GetSilverlightApplicationPath(int backTrackLength)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!String.IsNullOrEmpty(currentDirectory) && Directory.Exists(currentDirectory))
            {
                for (int iIndex = 0; iIndex < backTrackLength; iIndex++)
                {
                    currentDirectory = Directory.GetParent(currentDirectory).ToString();
                }
            }
            return currentDirectory + GetSilverlightApplication();
        }

        private static string GetBrowserTitle()
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SilverlightAppTitle");
        }
        #endregion
    }
}
