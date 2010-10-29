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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.Common.Silverlight;
using MVVMRI.Tests.AcceptanceTest.TestEntities.Page;
using System.Threading;
using AcceptanceTestLibrary.TestEntityBase;
using AcceptanceTestLibrary.ApplicationHelper;
using System.IO;
using System;
using System.Linq;
using System.Windows.Automation;
using AcceptanceTestLibrary.UIAWrapper;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MVVMRI.Tests.AcceptanceTest.Silverlight
{

#if DEBUG
    [DeploymentItem(@".\MVVMRI.Tests.AcceptanceTest\bin\Debug")]
    [DeploymentItem(@"..\MVVM.Client\Bin\Debug", "SL")]
#else
    [DeploymentItem(@".\MVVMRI.Tests.AcceptanceTest\bin\Release")]
    [DeploymentItem(@"..\MVVM.Client\Bin\Release", "SL")]
#endif

    [TestClass]
    public class MVVMRISilverlightTests : FixtureBase<SilverlightAppLauncher>
    {

        private const int BACKTRACKLENGTH = 4;

        #region Additional test attributes
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            //Parameter list as follows.
            //1. Port number of host application 2. Absolute path of the Silverlight Host.

            base.StartWebServer(GetPortNumber(GetSilverlightApplication()), GetAbsolutePath(BACKTRACKLENGTH) + GetSilverlightApplicationHostPath());
            MVVMRIPage<SilverlightAppLauncher>.Window = base.LaunchApplication(GetSilverlightApplication(), GetBrowserTitle())[0];
            Thread.Sleep(5000);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PageBase<SilverlightAppLauncher>.DisposeWindow();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());
            base.StopWebServer();
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Tests if the Silverlight MVVM QS is launched.
        /// </summary>
        [TestMethod]
        public void SilverlightMVVMRILaunchTest()
        {
            Assert.IsNotNull(MVVMRIPage<SilverlightAppLauncher>.Window, "MVVM RI application is not launched.");
        }


        [TestMethod]
        public void TakeSurveyTest()
        {
            AutomationElement TakeSurveyButton = MVVMRIPage<SilverlightAppLauncher>.TakeSurvey;
            Assert.IsNotNull(TakeSurveyButton, "Take Survey button is not loaded");
            TakeSurveyButton.Click();

            //See if any control is accessible
            MVVMRIPage<SilverlightAppLauncher>.Window = GetSurveyWindow();

            #region Name validation
            AutomationElement aeNameTextBox = MVVMRIPage<SilverlightAppLauncher>.NameTextBox;
            Assert.IsNotNull(aeNameTextBox, "Name field is not loaded");
            aeNameTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Name"));
            Thread.Sleep(2000);
            Assert.AreEqual(aeNameTextBox.GetValue().ToString(), new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Name"));
            #endregion

            #region Age Validation
            //Positive test case to validate Age
            AutomationElement aeAgeTextBox = MVVMRIPage<SilverlightAppLauncher>.AgeTextBox;
            Assert.IsNotNull(aeAgeTextBox, "Age Field is not loaded");
            aeAgeTextBox.SetFocus();
            aeAgeTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidAge"));
            Thread.Sleep(2000);
            Assert.AreEqual(aeAgeTextBox.GetValue().ToString(), new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidAge"));
            AutomationElement aeValidateAgeP = MVVMRIPage<SilverlightAppLauncher>.AgeValidation;
            Assert.IsNull(aeValidateAgeP, "Validation Error for Age");
            //AutomationElement aeValidation = BasicMVVMPage<SilverlightAppLauncher>.ValidationSummary;
            //Assert.IsTrue(aeValidation.Current.Name == new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("DefaultValidationText"), "Age  - Validation error");
                      
            #endregion
          
            #region Open Question1
            String validText = new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidText");
            AutomationElementCollection aeOpenQuestions = MVVMRIPage<SilverlightAppLauncher>.OpenQuestions;
            Assert.IsNotNull(aeOpenQuestions[0], "Open Question 1 is loaded");
            aeOpenQuestions[0].SetFocus();
            aeOpenQuestions[0].SetValue(validText);
            Thread.Sleep(2000);
            Assert.AreEqual(aeOpenQuestions[0].GetValue().ToString(), validText);
            #endregion
          
            #region Numeric question
            AutomationElement aeNumQuestion = MVVMRIPage<SilverlightAppLauncher>.NumericQuestion;
            Assert.IsNotNull(aeNumQuestion, "Numeric Question is not loaded");
            aeNumQuestion.SetFocus();
            aeNumQuestion.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidNumber"));
            Thread.Sleep(2000);
            AutomationElement aeValidatePositiveNumQ = MVVMRIPage<SilverlightAppLauncher>.ValidationNumericQ;
            Assert.IsNull(aeValidatePositiveNumQ, "Validation Error for Numeric Question");

            //Negative Test Case
            aeNumQuestion.SetFocus();
            aeNumQuestion.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("InvalidNumber"));
            Thread.Sleep(2000);
            AutomationElement aeValidateNumQ = MVVMRIPage<SilverlightAppLauncher>.ValidationNumericQ;
            //Assert.IsNotNull(aeValidateNumQ, "No Validation for Numeric Question");
            #endregion
         
            #region multiple choice question
            AutomationElementCollection aeQuestion2 = MVVMRIPage<SilverlightAppLauncher>.CheckBox;
            Assert.IsNotNull(aeQuestion2[1], "Choice 2 is not loaded");
            aeQuestion2[1].SetFocus();
            System.Windows.Forms.SendKeys.SendWait(" ");
            Thread.Sleep(1000);
            aeQuestion2[2].SetFocus();
            System.Windows.Forms.SendKeys.SendWait(" ");
            Thread.Sleep(1000);
            aeQuestion2[3].SetFocus();
            System.Windows.Forms.SendKeys.SendWait(" ");
            Thread.Sleep(1000);
            #endregion
            
            #region Open question2
            String textwithSpecChar = new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("TextWithSpecChars");
            AutomationElementCollection aeOpenQuestions2 = MVVMRIPage<SilverlightAppLauncher>.OpenQuestions;
            Assert.IsNotNull(aeOpenQuestions2[1], "Open Question 2 is not loaded");
            aeOpenQuestions2[1].SetFocus();
            aeOpenQuestions2[1].SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("TextWithSpecChars"));
            Thread.Sleep(2000);
            Assert.AreEqual(aeOpenQuestions2[1].GetValue().ToString(), textwithSpecChar);
            #endregion
        }

        [TestMethod]
        public void SubmitQuestionaire()
        {
            AutomationElement TakeSurveyButton = MVVMRIPage<SilverlightAppLauncher>.TakeSurvey;
            Assert.IsNotNull(TakeSurveyButton, "Take Survey button is not loaded");
            TakeSurveyButton.Click();

            //See if any control is accessible
            MVVMRIPage<SilverlightAppLauncher>.Window = GetSurveyWindow();

            //Set values for the controls

            //Submit button validation
            FillAllDetails();
            AutomationElement aeSubmit = MVVMRIPage<SilverlightAppLauncher>.Submit;
            Assert.IsTrue(aeSubmit.Current.IsEnabled, "Submit Button is not enabled");
            aeSubmit.Click();
            Thread.Sleep(2000);

            
        }

        [TestMethod]
        public void ResetQuestionaire()
        {

            AutomationElement TakeSurveyButton = MVVMRIPage<SilverlightAppLauncher>.TakeSurvey;
            Assert.IsNotNull(TakeSurveyButton, "Take Survey button is not loaded");
            TakeSurveyButton.Click();

            //See if any control is accessible
            MVVMRIPage<SilverlightAppLauncher>.Window = GetSurveyWindow();
            //Reset button validation
            FillAllDetails();
            AutomationElement aeReset = MVVMRIPage<SilverlightAppLauncher>.Reset;
            Assert.IsTrue(aeReset.Current.IsEnabled, "Reset Button is not enabled");
            aeReset.Click();
            Thread.Sleep(1000);
            AutomationElement aeOk = MVVMRIPage<SilverlightAppLauncher>.Ok;
            Assert.IsTrue(aeOk.Current.IsEnabled, "OK Button is not loaded");
            aeOk.Click();
            Thread.Sleep(1000);          

        }


        [TestMethod]
        public void TakeDrawBridgeSurvey()
        {
            AutomationElementCollection TakeSurveyButtonList = MVVMRIPage<SilverlightAppLauncher>.TakeSurveyList;
            Assert.IsNotNull(TakeSurveyButtonList, "Take Survey buttons are not loaded");
            TakeSurveyButtonList[1].Click();
            Thread.Sleep(1000);

            //See if any control is accessible
            MVVMRIPage<SilverlightAppLauncher>.Window = GetSurveyWindow();

            #region Name validation
            AutomationElement aeNameTextBox = MVVMRIPage<SilverlightAppLauncher>.NameTextBox;
            Assert.IsNotNull(aeNameTextBox, "Name field is not loaded");
            aeNameTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Name"));
            Thread.Sleep(2000);
            Assert.AreEqual(aeNameTextBox.GetValue().ToString(), new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Name"));
            #endregion

            #region Age Validation
            //Positive test case to validate Age
            AutomationElement aeAgeTextBox = MVVMRIPage<SilverlightAppLauncher>.AgeTextBox;
            Assert.IsNotNull(aeAgeTextBox, "Age Field is not loaded");
            aeAgeTextBox.SetFocus();
            aeAgeTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidAge"));
            Thread.Sleep(2000);
            Assert.AreEqual(aeAgeTextBox.GetValue().ToString(), new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidAge"));
            AutomationElement aeValidateAgeP = MVVMRIPage<SilverlightAppLauncher>.AgeValidation;
            Assert.IsNull(aeValidateAgeP, "Validation Error for Age");

            #endregion

            #region Numeric question
            AutomationElement aeNumQuestion = MVVMRIPage<SilverlightAppLauncher>.NumericQuestion;
            Assert.IsNotNull(aeNumQuestion, "Numeric Question is not loaded");

            //Negative Test Case - Enter Text in Age field
            aeNumQuestion.SetFocus();
            aeNumQuestion.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidText"));
            Thread.Sleep(2000);
            AutomationElement aeValidateNumQ = MVVMRIPage<SilverlightAppLauncher>.ValidationNumericQDrawbridge;
            Assert.IsNotNull(aeValidateNumQ, "No Validation for Numeric Question");

            //Positive Question
            aeNumQuestion.SetFocus();
            aeNumQuestion.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidNumber"));
            Thread.Sleep(2000);
            AutomationElement aeValidatePositiveNumQ = MVVMRIPage<SilverlightAppLauncher>.ValidationNumericQ;
            Assert.IsNull(aeValidatePositiveNumQ, "Validation Error for Numeric Question");
            #endregion

            #region multiple choice question
            AutomationElementCollection aeMulQuestion = MVVMRIPage<SilverlightAppLauncher>.CheckBox;
            Assert.IsNotNull(aeMulQuestion[1], "Choice 2 is not loaded");

            //Negative Test case
            aeMulQuestion[1].SetFocus();
            System.Windows.Forms.SendKeys.SendWait(" ");
            Thread.Sleep(1000);
            aeMulQuestion[2].SetFocus();
            System.Windows.Forms.SendKeys.SendWait(" ");
            Thread.Sleep(1000);
            AutomationElement aeValidateMulQDrawbridge = MVVMRIPage<SilverlightAppLauncher>.ValidationMulQDrawBridge;
            Assert.IsNotNull(aeValidateMulQDrawbridge, "No Validation for Multiple Choice Question");

            //Positive Test Case
            aeMulQuestion[2].SetFocus();
            System.Windows.Forms.SendKeys.SendWait(" ");
            Thread.Sleep(1000);
            AutomationElement aeValidateMulQDrawbridge1 = MVVMRIPage<SilverlightAppLauncher>.ValidationMulQDrawBridge;
            Assert.IsNull(aeValidateMulQDrawbridge1, "Validation Error for Multiple Choice Question");

            #endregion

            AutomationElement aeSubmit = MVVMRIPage<SilverlightAppLauncher>.Submit;
            Assert.IsTrue(aeSubmit.Current.IsEnabled, "Submit Button is not enabled");
            aeSubmit.Click();
            Thread.Sleep(2000);

        }

        [TestMethod]
        public void ResetDrawbridgeQuestionaire()
        {

            AutomationElementCollection TakeSurveyButtonList = MVVMRIPage<SilverlightAppLauncher>.TakeSurveyList;
            Assert.IsNotNull(TakeSurveyButtonList, "Take Survey buttons are not loaded");
            TakeSurveyButtonList[1].Click();
            Thread.Sleep(1000);

            //See if any control is accessible
            MVVMRIPage<SilverlightAppLauncher>.Window = GetSurveyWindow();
            //Reset button validation
            FillAllDetailsForDrawbridge();
            AutomationElement aeReset = MVVMRIPage<SilverlightAppLauncher>.Reset;
            Assert.IsTrue(aeReset.Current.IsEnabled, "Reset Button is not enabled");
            aeReset.Click();
            Thread.Sleep(1000);
            AutomationElement aeOk = MVVMRIPage<SilverlightAppLauncher>.Ok;
            Assert.IsTrue(aeOk.Current.IsEnabled, "OK Button is not loaded");
            aeOk.Click();
            Thread.Sleep(1000);


        }


        #endregion

        #region Helper Methods

        private void FillAllDetails()
        {
            AutomationElement aeNameTextBox = MVVMRIPage<SilverlightAppLauncher>.NameTextBox;
            aeNameTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Name"));
            Thread.Sleep(1000);

            AutomationElement aeAgeTextBox = MVVMRIPage<SilverlightAppLauncher>.AgeTextBox;
            aeAgeTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidAge"));
            Thread.Sleep(1000);


            String validText = new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidText");
            AutomationElementCollection aeQuestion1 = MVVMRIPage<SilverlightAppLauncher>.OpenQuestions;
            aeQuestion1[0].SetValue(validText);
            Thread.Sleep(1000);

            AutomationElement aeQuestion4 = MVVMRIPage<SilverlightAppLauncher>.NumericQuestion;
            aeQuestion4.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidNumber"));
            Thread.Sleep(1000);

            AutomationElementCollection aeQuestion2 = MVVMRIPage<SilverlightAppLauncher>.CheckBox;
            Assert.IsNotNull(aeQuestion2[1], "Choice 2 is not loaded");
            aeQuestion2[1].SetFocus();
            System.Windows.Forms.SendKeys.SendWait(" ");
            Thread.Sleep(1000);

            String textwithSpecChar = new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("TextWithSpecChars");
            AutomationElementCollection aeOpenQuestions2 = MVVMRIPage<SilverlightAppLauncher>.OpenQuestions;
            aeOpenQuestions2[1].SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("TextWithSpecChars"));
            Thread.Sleep(1000);
        }

        private void FillAllDetailsForDrawbridge()
        {
            AutomationElement aeNameTextBox = MVVMRIPage<SilverlightAppLauncher>.NameTextBox;
            aeNameTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Name"));
            Thread.Sleep(1000);

            AutomationElement aeAgeTextBox = MVVMRIPage<SilverlightAppLauncher>.AgeTextBox;
            aeAgeTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidAge"));
            Thread.Sleep(1000);

            
            AutomationElement aeNumQ = MVVMRIPage<SilverlightAppLauncher>.NumericQuestion;
            aeNumQ.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidNumber"));
            Thread.Sleep(1000);

            AutomationElementCollection aeMulQ = MVVMRIPage<SilverlightAppLauncher>.CheckBox;
            Assert.IsNotNull(aeMulQ[1], "Choice 2 is not loaded");
            aeMulQ[1].SetFocus();
            System.Windows.Forms.SendKeys.SendWait(" ");
            Thread.Sleep(1000);            
        }


        //private void CheckAllFieldsCleared()
        //{
        //    AutomationElement aeNameTextBox = MVVMRIPage<SilverlightAppLauncher>.NameTextBox;
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeNameTextBox.GetValue().Equals(""), "Name Field is not cleared");

        //    AutomationElement aeAgeTextBox = MVVMRIPage<SilverlightAppLauncher>.AgeTextBox;
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeAgeTextBox.GetValue().Equals(""), "Age Field is not cleared");

        //    AutomationElementCollection aeOpenQuestions = MVVMRIPage<SilverlightAppLauncher>.OpenQuestions;
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeOpenQuestions[0].GetValue().Equals(""), "Open Question 1 Field is not cleared");
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeOpenQuestions[1].GetValue().Equals(""), "Open Question 2 Field is not cleared");

        //    AutomationElement aeQuestion4 = MVVMRIPage<SilverlightAppLauncher>.NumericQuestion;
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeQuestion4.GetValue().Equals(""), "Question 4 Field is not cleared");
        //}

        private static AutomationElement GetSurveyWindow()
        {
            //Capture the newly opened window
            Process currentProcess = Process.GetProcesses().First<Process>(proc =>
                    proc.ProcessName.Equals(ConfigHandler.GetValue("IEAppProcessName")) &&
                        proc.MainWindowTitle.StartsWith(GetBrowserTitle()));

            if (!(currentProcess.HasExited || currentProcess.MainWindowHandle == IntPtr.Zero))
            {
                return (AutomationElement.FromHandle(currentProcess.MainWindowHandle));
            }
            return null;
        }
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

        //private static string GetBrowserTitle()
        //{
        //    return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SilverlightAppTitle");
        //}

        //private void FillAllDetails()
        //{
        //    AutomationElement aeNameTextBox = MVVMRIPage<SilverlightAppLauncher>.NameTextBox;
        //    aeNameTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Name"));
        //    Thread.Sleep(1000);

        //    AutomationElement aeAgeTextBox = MVVMRIPage<SilverlightAppLauncher>.AgeTextBox;
        //    aeAgeTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidAge"));
        //    Thread.Sleep(1000);


        //    String validText = new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidText");
        //    AutomationElementCollection aeQuestion1 = MVVMRIPage<SilverlightAppLauncher>.OpenQuestions;
        //    aeQuestion1[0].SetValue(validText);
        //    Thread.Sleep(1000);

        //    AutomationElement aeQuestion4 = MVVMRIPage<SilverlightAppLauncher>.NumericQuestion;
        //    aeQuestion4.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("ValidNumber"));
        //    Thread.Sleep(1000);

        //    AutomationElementCollection aeQuestion2 = MVVMRIPage<SilverlightAppLauncher>.CheckBox;
        //    Assert.IsNotNull(aeQuestion2[1], "Choice 2 is not loaded");
        //    aeQuestion2[1].SetFocus();
        //    System.Windows.Forms.SendKeys.SendWait(" ");
        //    Thread.Sleep(1000);

        //    String textwithSpecChar = new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("TextWithSpecChars");
        //    AutomationElementCollection aeOpenQuestions2 = MVVMRIPage<SilverlightAppLauncher>.OpenQuestions;
        //    aeOpenQuestions2[1].SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("TextWithSpecChars"));
        //    Thread.Sleep(1000);
        //}

        //private void CheckAllFieldsCleared()
        //{
        //    AutomationElement aeNameTextBox = MVVMRIPage<SilverlightAppLauncher>.NameTextBox;
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeNameTextBox.GetValue().Equals(""), "Name Field is not cleared");

        //    AutomationElement aeAgeTextBox = MVVMRIPage<SilverlightAppLauncher>.AgeTextBox;
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeAgeTextBox.GetValue().Equals(""), "Age Field is not cleared");

        //    AutomationElementCollection aeOpenQuestions = MVVMRIPage<SilverlightAppLauncher>.OpenQuestions;
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeOpenQuestions[0].GetValue().Equals(""), "Open Question 1 Field is not cleared");
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeOpenQuestions[1].GetValue().Equals(""), "Open Question 2 Field is not cleared");

        //    AutomationElement aeQuestion4 = MVVMRIPage<SilverlightAppLauncher>.NumericQuestion;
        //    Thread.Sleep(1000);
        //    Assert.IsTrue(aeQuestion4.GetValue().Equals(""), "Question 4 Field is not cleared");

        //    //AutomationElementCollection aeQuestion2 = MVVMRIPage<SilverlightAppLauncher>.CheckBox;
        //    //foreach (AutomationElement ae in aeQuestion2)
        //    //{
        //    //    Assert.IsTrue(ae.GetValue().Equals(""), "Question 2 Checkboxes are not cleared");
        //    //}            

         
           
        //}
        #endregion

        #region Helper Methods
        //private static string GetSilverlightApplication()
        //{
        //    return ConfigHandler.GetValue("SilverlightAppLocation");
        //}

        //private static string GetSilverlightApplicationPath(int backTrackLength)
        //{
        //    string currentDirectory = Directory.GetCurrentDirectory();
        //    if (!String.IsNullOrEmpty(currentDirectory) && Directory.Exists(currentDirectory))
        //    {
        //        for (int iIndex = 0; iIndex < backTrackLength; iIndex++)
        //        {
        //            currentDirectory = Directory.GetParent(currentDirectory).ToString();
        //        }
        //    }
        //    return currentDirectory + GetSilverlightApplication();
        //}

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
            return ConfigHandler.GetValue("SilverlightAppLocationRelativeLocation");
        }
        #endregion
    }
}
