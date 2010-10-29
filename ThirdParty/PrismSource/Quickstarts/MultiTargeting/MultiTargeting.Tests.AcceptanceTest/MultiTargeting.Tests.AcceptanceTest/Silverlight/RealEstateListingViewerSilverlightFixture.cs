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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading;
using System.IO;
using System.Diagnostics;

using System.Windows;
using System.Windows.Forms;

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Text;
using System.Windows.Automation.Provider;

using AcceptanceTestLibrary.Common;
using MultiTargeting.Tests.AcceptanceTest.TestEntities.Page;
using MultiTargeting.Tests.AcceptanceTest.TestEntities.Assertion;
using AcceptanceTestLibrary.ApplicationObserver;
using AcceptanceTestLibrary.Common.Silverlight;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using AcceptanceTestLibrary.ApplicationHelper;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using AcceptanceTestLibrary.TestEntityBase;

namespace MultiTargeting.Tests.AcceptanceTest.Silverlight
{
    /// <summary>
    /// Summary description for SilverlightAcceptanceTest
    /// </summary>
#if DEBUG
    [DeploymentItem(@"..\Silverlight\RealEstateListingViewer\bin\Debug", "Silverlight")]
    [DeploymentItem(@".\MultiTargeting.Tests.AcceptanceTest\bin\Debug")]
#else
    [DeploymentItem(@"..\Silverlight\RealEstateListingViewer\bin\Release", "Silverlight")]
    [DeploymentItem(@".\MultiTargeting.Tests.AcceptanceTest\bin\Release")]
#endif

    [TestClass]
    public class RealEstateListingViewerSilverlightFixture : FixtureBase<SilverlightAppLauncher>
    {
        #region Additional test attributes

        [ClassInitialize()]
        public static void RealEstateViewerSilverlightClassInitialize(TestContext testContext)
        {
            
        }

        [ClassCleanup()]
        public static void RealEstateViewerSilverlightClassCleanup()
        {

        }
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            //Launch Cassini server
            const int BACKTRACKLENGTH = 4;

            //Parameter list as follows.
            //1. Port number of host application 
            //2. Absolute path of the Silverlight Host.
            base.StartWebServer(
                GetPortNumber(GetSilverlightApplication()),
                GetAbsolutePath(BACKTRACKLENGTH) + GetSilverlightApplicationHostPath());

            //launch the application and assign the handle to Window property in the Page class
            RealEstateListingViewerPage<SilverlightAppLauncher>.Window = 
                base.LaunchApplication(GetSilverlightApplication(), GetBrowserTitle())[0];
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            base.StopWebServer();

            PageBase<SilverlightAppLauncher>.DisposeWindow();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());
        }
        
        #endregion

        #region Application Launch Test

        [TestMethod]
        public void SilverlightApplicationLoadTest()
        {
            Assert.IsNotNull(RealEstateListingViewerPage<SilverlightAppLauncher>.Window);
        }

        #endregion

        #region Test Methods

        #region Control Load Test
        [TestMethod]
        public void SilverlightControlsLoadTest()
        {
            InvokeImageAssert();

            //Loading of Controls
            InvokeControlsAssert();
        }
        #endregion

        #region Control Content TEST
        [TestMethod]
        public void SilverlightControlsContentTest()
        {
            InvokeControlsContentAssert();
        }
        #endregion

        #endregion

        #region Private Methods

        private static void InvokeImageAssert()
        {
            //Loading of Images
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertGarageImage();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertBathroomsImage();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertBedroomsImage();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertAcerageImage();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertPropertyImage();
        }

        private static void InvokeControlsAssert()
        {
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertAddressTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertCountyTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertStateTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertZipCodeTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertBathroomsTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertBedroomsTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertGarageSizeTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertAcerageTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertPriceTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertDescriptionLabelTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertDescriptionTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertAddressBelowHouseImageTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertCountyBelowHouseImageTextBlockLoading();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertStateBelowHouseImageTextBlockLoading();
        }

        private static void InvokeControlsContentAssert()
        {
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertAddressTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertCountyTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertStateTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertZipCodeTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertBathroomsTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertBedroomsTextBlockContent();

            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertGarageSizeTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertAcerageTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertPriceTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertDescriptionLabelTextBlockContentSilverlight();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertDescriptionTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertAddressBelowHouseImageTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertCountyBelowHouseImageTextBlockContent();
            RealEstateListingViewerAssertion<SilverlightAppLauncher>.AssertStateBelowHouseImageTextBlockContent();
        }

        private static string GetSilverlightApplication()
        {
            return ConfigHandler.GetValue("SilverlightAppLocation");
        }

        private static string GetSilverlightApplicationHostPath()
        {
            return ConfigHandler.GetValue("SilverlightAppHostRelativeLocation");
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

        #endregion
    }
}
