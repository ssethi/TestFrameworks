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
using AcceptanceTestLibrary.Common.Silverlight;
using AcceptanceTestLibrary.TestEntityBase;
using AcceptanceTestLibrary.ApplicationHelper;
using System.Reflection;
using System.Threading;
using System.IO;
using StateBasedNavigation.Tests.AcceptanceTest.TestEntities.Assertion;
using StateBasedNavigation.Tests.AcceptanceTest.TestEntities.Page;

namespace StateBasedNavigation.Tests.AcceptanceTest.Silverlight
{
#if DEBUG
    [DeploymentItem(@".\State-Based Navigation.Tests.AcceptanceTest\bin\Debug")]
    [DeploymentItem(@"..\State-Based Navigation\Bin\Debug", "SL")]
#else
    [DeploymentItem(@".\State-Based Navigation.Tests.AcceptanceTest\bin\Release")]
    [DeploymentItem(@"..\State-Based Navigation\Bin\Release", "SL")]
#endif

    [TestClass]
    public class StateBasedNavigation_Silverlight_Tests: FixtureBase<SilverlightAppLauncher>
    {
        private const int BACKTRACKLENGTH = 4;

        #region Additional test attributes

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            StateBasedNavigationPage<SilverlightAppLauncher>.Window = base.LaunchApplication(currentOutputPath + GetSilverlightApplication(), GetBrowserTitle())[0];
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
        /// StateBasedNavigation Launch
        /// </summary>
        [TestMethod]
        public void StateBasedNavigation_NavigationLaunch()
        {
            Assert.IsNotNull(StateBasedNavigationPage<SilverlightAppLauncher>.Window, "Navigation Prototype application is not launched.");
        }

        /// <summary>
        /// Validations on State Based Navigation App launch
        /// </summary>
        [TestMethod]
        public void StateBasedNavigation_OnLoad()
        {
            StateBasedNavigation_Assertion<SilverlightAppLauncher>.StateBasedNavigation_OnLoad();         
        }
        /// <summary>
        ///  Validations on clicking avatars button
        /// </summary>
        [TestMethod]        
        public void StateBasedNavigation_ClickAvatars()
        {
            StateBasedNavigation_Assertion<SilverlightAppLauncher>.StateBasedNavigation_ClickAvatars();      
        }
        /// <summary>
        ///  Validations on selecting Unavailable From Combobox
        /// </summary>
        [TestMethod]
        public void StateBasedNavigation_SelectUnavailable()
        {
            StateBasedNavigation_Assertion<SilverlightAppLauncher>.StateBasedNavigation_SelectUnavailable(); 
        }
        /// <summary>
        /// Validations on clicking "Show Details" 
        /// </summary>
        [TestMethod]
        public void StateBasedNavigation_ClickDetails()
        {
            StateBasedNavigation_Assertion<SilverlightAppLauncher>.StateBasedNavigation_ClickDetails(); 
        }

        /// <summary>
        /// Validations on clicking "Show Details" in Avatars View
        /// </summary>
        [TestMethod]
        public void StateBasedNavigation_ClickDetailsInAvatarsView()
        {
            StateBasedNavigation_Assertion<SilverlightAppLauncher>.StateBasedNavigation_ClickAvatars();  
            StateBasedNavigation_Assertion<SilverlightAppLauncher>.StateBasedNavigation_ClickDetailsInAvatarView();
        }
        /// <summary>
        ///  Validations On clicking "Send Message" button
        /// </summary>
        [TestMethod]
        public void StateBasedNavigation_SendMessage()
        {
            StateBasedNavigation_Assertion<SilverlightAppLauncher>.StateBasedNavigation_SendMessage(); 
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
        
        #endregion
    }
}
