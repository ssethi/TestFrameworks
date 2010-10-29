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

using System.IO;
using System.Diagnostics;
using System.Threading;

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Text;
using System.Windows.Automation.Provider;

using AcceptanceTestLibrary.ApplicationHelper;
using AcceptanceTestLibrary.Common;
using MultiTargeting.Tests.AcceptanceTest.TestEntities.Page;
using MultiTargeting.Tests.AcceptanceTest.TestEntities.Assertion;
using MultiTargeting.Tests.AcceptanceTest.TestEntities.Action;
using AcceptanceTestLibrary.ApplicationObserver;
using AcceptanceTestLibrary.Common.Desktop;
using AcceptanceTestLibrary.TestEntityBase;
using System.Reflection;

namespace MultiTargeting.Tests.AcceptanceTest.Desktop
{
    /// <summary>
    /// Acceptance test fixture for WPF application
    /// </summary>
#if DEBUG
    [DeploymentItem(@"..\Desktop\RealEstateListingViewer\bin\Debug", "WPF")] 
    [DeploymentItem(@".\MultiTargeting.Tests.AcceptanceTest\bin\Debug")]
#else
    [DeploymentItem(@"..\Desktop\RealEstateListingViewer\bin\Release", "WPF")]
    [DeploymentItem(@".\MultiTargeting.Tests.AcceptanceTest\bin\Release")]
#endif
    
    [TestClass]
    public class RealEstateListingViewerDesktopFixture : FixtureBase<WpfAppLauncher>
    {
        #region Additional test attributes
        
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            //get the first window as WPF window is just one and the list feature has been 
            //introduced for Silverlight support
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            RealEstateListingViewerPage<WpfAppLauncher>.Window =
                base.LaunchApplication(currentOutputPath + GetDesktopApplication(), GetDesktopApplicationProcess())[0];
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            PageBase<WpfAppLauncher>.DisposeWindow();
            Process p = WpfAppLauncher.GetCurrentAppProcess();
            base.UnloadApplication(p);
        }
        
        #endregion

        #region Test Methods

        #region Application Launch Test
        [TestMethod]
        public void DesktopApplicationLoadTest()
        {
            //check if window handle object is not null
            Assert.IsNotNull(RealEstateListingViewerPage<WpfAppLauncher>.Window, "RealEstateListingViewer is not launched.");
        }
        #endregion

        #region Control Load Test
        [TestMethod]
        public void DesktopControlsLoadTest()
        {
            InvokeImageAssert();

            //Loading of Controls
            InvokeControlsAssert();
        }
        #endregion

        #region Control Content TEST
        [TestMethod]
        public void DesktopControlsContentTest()
        {
            InvokeControlsContentAssert();
        }

        #endregion

        #endregion

        #region Private Methods

        private static void InvokeImageAssert()
        {
            //Loading of Images
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertGarageImage();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertBathroomsImage();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertBedroomsImage();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertAcerageImage();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertPropertyImage();
        }

        private static void InvokeControlsAssert()
        {
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertAddressTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertCountyTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertStateTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertZipCodeTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertBathroomsTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertBedroomsTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertGarageSizeTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertAcerageTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertPriceTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertDescriptionLabelTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertDescriptionTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertAddressBelowHouseImageTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertCountyBelowHouseImageTextBlockLoading();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertStateBelowHouseImageTextBlockLoading();
        }

        private static void InvokeControlsContentAssert()
        {
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertAddressTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertCountyTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertStateTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertZipCodeTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertBathroomsTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertBedroomsTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertGarageSizeTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertAcerageTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertPriceTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertDescriptionLabelTextBlockContentWPF();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertDescriptionTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertAddressBelowHouseImageTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertCountyBelowHouseImageTextBlockContent();
            RealEstateListingViewerAssertion<WpfAppLauncher>.AssertStateBelowHouseImageTextBlockContent();
        }

        private static string GetDesktopApplicationProcess()
        {
            return ConfigHandler.GetValue("WpfAppProcessName");
        }

        private static string GetDesktopApplication()
        {
            return ConfigHandler.GetValue("WpfAppLocation");
        }
        #endregion
    }
}
