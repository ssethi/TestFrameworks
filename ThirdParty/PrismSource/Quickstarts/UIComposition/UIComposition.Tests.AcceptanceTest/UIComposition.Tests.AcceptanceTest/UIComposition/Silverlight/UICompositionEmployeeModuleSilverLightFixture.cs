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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.ApplicationHelper;
using System.IO;
using AcceptanceTestLibrary.Common.Silverlight;
using AcceptanceTestLibrary.TestEntityBase;
using System.Reflection;
using UIComposition.Tests.AcceptanceTest.TestEntities.Page;
using UIComposition.Tests.AcceptanceTest.TestEntities.Assertion;
using System.Threading;
using System.Text.RegularExpressions;

namespace UIComposition.Tests.AcceptanceTest.ViewDiscovery.Silverlight
{
    /// <summary>
    /// Summary description for UI Composition
    /// </summary>
#if DEBUG
    [DeploymentItem(@"..\UIComposition.Tests.AcceptanceTest\bin\Debug", "TS")]
    [DeploymentItem(@".\UIComposition.Tests.AcceptanceTest\bin\Debug")]
#else
    //[DeploymentItem(@"..\ViewDiscovery\SilverLight\UIComposition\bin\Release", "TDSL")]
    [DeploymentItem(@"..\UIComposition.Tests.AcceptanceTest\bin\Release", "TS")]
    [DeploymentItem(@".\UIComposition.Tests.AcceptanceTest\bin\Release")]
#endif

    [TestClass]
    public class UICompositionEmployeeModuleSilverLightFixture : FixtureBase<SilverlightAppLauncher>
    {
        private const int BACKTRACKLENGTH = 5;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            string pp = GetAbsolutePath(BACKTRACKLENGTH);

            //Parameter list as follows.
            //1. Port number of host application 2. Absolute path of the Silverlight Host.

            base.StartWebServer(GetPortNumber(GetSilverlightApplication()), GetAbsolutePath(BACKTRACKLENGTH) + GetSilverlightApplicationHostPath());
            UICompositionPage<SilverlightAppLauncher>.Window = base.LaunchApplication(GetSilverlightApplication(), GetBrowserTitle())[0];
            Thread.Sleep(5000);
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PageBase<SilverlightAppLauncher>.DisposeWindow();
            base.StopWebServer();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());
        }


        #region Test Methods
        /// <summary>
        /// Tests if the Silverlight UI composition View Discovery application is launched.
        /// </summary>
        [TestMethod]
        public void SilverlightUICompositionLaunchTest()
        {
            Assert.IsNotNull(UICompositionPage<SilverlightAppLauncher>.Window, "View Discovery Visual composition application is not launched.");
        }

        /// <summary>
        /// Validate if the details view of the selected employee are displayed correctly.
        /// 
        /// Repro Steps:
        /// 1. Launch the QS application
        /// 2. Click on the first employee row in the Employee List table
        /// 3. Check if the Details View Tab is displayed, and the number of tab items is 3.
        /// 4. Check if the tab items headers match with "General", "Location" and "Current Projects"
        /// 
        /// Expected Result:
        /// Details View Tab is dispalyed with 2 tab items. 
        /// The tab items headers match with "General" and "Current Projects"
        /// </summary>
        [TestMethod]
        public void SilverLightUICompositionValidateEmployeeSelection()
        {
            UICompositionAssertion<SilverlightAppLauncher>.AssertSilverLightEmployeeSelection();
        }

        /// <summary>
        /// Validate General details in the General Tab for selected employee
        /// 
        /// Repro Steps:
        /// 1. Launch the QS Application
        /// 2. Select the first employee row in the Employee List table
        /// 3. Check if the details of the selected employee are displayed in the General tab
        /// 
        /// Expected results:
        /// Employee First Name, Last Name, Phone and Email are correctly displayed in the General Tab
        /// </summary>
        [TestMethod]
        public void SilverLightUICompositionValidateGeneralDataForAllEmployees()
        {
            UICompositionAssertion<SilverlightAppLauncher>.AssertSilverLightEmployeeGeneralData();
        }

        /// <summary>
        /// Validate Current Projects details in the Current Projects Tab for selected employee
        /// 
        /// Repro Steps:
        /// 1. Launch the QS Application
        /// 2. Select the first employee row in the Employee List table
        /// 3. Check if the Current Projects of the selected employee are displayed in the Current Projects tab
        /// 
        /// Expected results:
        /// Current Project and Role of the selected Employee are correctly displayed in the Current Projects Tab
        /// </summary>
        [TestMethod]
        public void SilverLightUICompositionValidateProjects()
        {
            UICompositionAssertion<SilverlightAppLauncher>.AssertSilverLightEmployeeCurrentProjects();
        }



        #endregion
        #region Helper Methods
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

        private static string GetAbsolutePath(int backTrackLength)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!String.IsNullOrEmpty(currentDirectory) && Directory.Exists(currentDirectory))
            {
                for (int iIndex = 0; iIndex < backTrackLength; iIndex++)
                {
                    currentDirectory = Directory.GetParent(currentDirectory).ToString();
                }
            }

            return currentDirectory;
        }


        /// <summary>
        /// Extract the Port number from a URL of the format http://ServerName:PortNumber/SitePath
        /// </summary>
        /// <param name="url">URL of the above format</param>
        /// <returns>port number sub-string</returns>
        private static string GetPortNumber(string url)
        {
            string urlPattern = @"^(?<proto>\w+)://[^/]+?(?<port>:\d+)?/";
            Regex urlExpression = new Regex(urlPattern, RegexOptions.Compiled);
            Match urlMatch = urlExpression.Match(url);
            return urlMatch.Groups["port"].Value;
        }

        private static string GetSilverlightApplicationHostPath()
        {
            return ConfigHandler.GetValue("SilverlightAppHostRelativeLocation");
        }
        #endregion
    }


}
