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
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.TestEntityBase;
using UIComposition.Tests.AcceptanceTest.TestInfrastructure;
using System.Windows.Automation;
using AcceptanceTestLibrary.ApplicationHelper;

namespace UIComposition.Tests.AcceptanceTest.TestEntities.Page
{
    public static class UICompositionPage<TApp>
        where TApp : AppLauncherBase, new()
    {
        #region Silverlight
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement GarageImage
        {
            get { return PageBase<TApp>.FindControlByAutomationId("GarageImgAutomation"); }
        }

        //public static AutomationElementCollection EmployeesListGrid1
        //{

        //    get
        //    {
        //        AutomationElement elementList = null;
        //        // Set up the CacheRequest.
        //        CacheRequest cacheRequest = new CacheRequest();
        //        cacheRequest.Add(AutomationElement.ControlTypeProperty);
        //        cacheRequest.TreeScope = TreeScope.Element | TreeScope.Children;

        //        using (cacheRequest.Activate())
        //        {
        //            PropertyCondition c = new PropertyCondition(
        //                AutomationElement.ControlTypeProperty, ControlType.DataGrid);
        //            PropertyCondition c1 = new PropertyCondition(AutomationElement.AutomationIdProperty,
        //                "EmployeesListGrid");

        //            AndCondition andCond = new AndCondition(c, c1);

        //            elementList = Window.FindFirst(TreeScope.Descendants, andCond);
        //        }
        //        return elementList.CachedChildren;
        //    }
               
        //}
        public static AutomationElement EmployeesListGrid
        {
            get
            {
                return PageBase<TApp>.FindControlByAutomationId("EmployeesList");
            }
        }

      
        public static AutomationElement EmployeeSummaryTabControl
        {
            get { return PageBase<TApp>.FindControlByAutomationId("EmployeeSummaryTabControl"); }
        }
        public static AutomationElement SilverLightFirstNameTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("FirstNameText"); }
        }

        public static AutomationElement SilverLightPhoneTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("PhoneText"); }
        }

        public static AutomationElement SilverLightLastNameTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("LastNameText"); }
        }

        public static AutomationElement SilverLightEmailTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("EmailText"); }
        }

        public static AutomationElement SilverLightProjectsGrid
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ProjectsList"); }
        }

        public static AutomationElementCollection AllTextBoxes
        {
            get
            {
                PropertyCondition findText = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text);
                return Window.FindAll(TreeScope.Descendants, findText);
            }
        }

        public static AutomationElementCollection EmployeesGridItems
        {
            get
            {
              
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.DataItem);                
                return EmployeesListGrid.FindAll(TreeScope.Descendants, cond1);
            }
        }

        public static AutomationElementCollection EmployeeTabItems
        {
            get
            {

                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                return EmployeeSummaryTabControl.FindAll(TreeScope.Descendants, cond1);
            }
        }
        #endregion

        #region Desktop
          

        public static AutomationElementCollection EmployeesList
        {
            get { return PageBase<TApp>.FindControlByType("EmployeesList"); }
        }

        public static AutomationElementCollection EmployeeDetailsTab
        {
            get { return PageBase<TApp>.FindControlByType("DetailsTabControl"); }
        }

        public static AutomationElement FirstNameLabel
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty, TestDataInfrastructure.GetTestInputData("FirstNameLabelText"));
                return Window.FindFirst(TreeScope.Descendants, cond);
            }
        }

        public static AutomationElement LastNameLabel
        {
            get
            {              
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty, TestDataInfrastructure.GetTestInputData("LastNameLabelText"));          
                return Window.FindFirst(TreeScope.Descendants, cond);
            }
        }

        public static AutomationElement PhoneLabel
        {
            get
            {               
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty, TestDataInfrastructure.GetTestInputData("PhoneLabelText"));
                return Window.FindFirst(TreeScope.Descendants, cond);
            }
        }

        public static AutomationElement EmailLabel
        {
            get
            {               
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty, TestDataInfrastructure.GetTestInputData("EmailLabelText"));              
                return Window.FindFirst(TreeScope.Descendants, cond);
            }
        }

        public static AutomationElement FirstNameTextbox
        {
            get { return Window.FindFirst(TreeScope.Descendants, 
                new PropertyCondition(AutomationElement.AutomationIdProperty, 
                    TestDataInfrastructure.GetControlId("FirstNameTextBox"), PropertyConditionFlags.IgnoreCase)); }
        }

        public static AutomationElement LastNameTextBox
        {           
            get
            {               
                return PageBase<TApp>.FindControlByAutomationId("LastNameTextBox"); 
            }
        }
        public static AutomationElement PhoneTextBox
        {
            get
            {
                //return Window.FindFirst(TreeScope.Element,
                //    new PropertyCondition(AutomationElement.AutomationIdProperty,
                //  TestDataInfrastructure.GetControlId("PhoneTextBox"), PropertyConditionFlags.IgnoreCase));
                return PageBase<TApp>.FindControlByAutomationId("PhoneTextBox");
            }
            
        }
        public static AutomationElement EmailTextBox
        {            
            get
            {
                //return Window.FindFirst(TreeScope.Element,
                //    new PropertyCondition(AutomationElement.AutomationIdProperty,
                //  TestDataInfrastructure.GetControlId("EmailTextBox"), PropertyConditionFlags.IgnoreCase));
                return PageBase<TApp>.FindControlByAutomationId("EmailTextBox");
            }
        }

        public static AutomationElement ProjectsLabel
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty, TestDataInfrastructure.GetTestInputData("PartOfFollowingProjectsLabel"));              
                return Window.FindFirst(TreeScope.Descendants, cond);
            }
        }

        public static AutomationElementCollection ProjectsList
        {
            get
            {
                AutomationElement elementList = null;
                // Set up the CacheRequest.
                CacheRequest cacheRequest = new CacheRequest();
                cacheRequest.Add(AutomationElement.ControlTypeProperty);
                cacheRequest.TreeScope = TreeScope.Element | TreeScope.Children;

                using (cacheRequest.Activate())
                {
                   PropertyCondition c = new PropertyCondition(
                       AutomationElement.ControlTypeProperty, ControlType.DataGrid);
                   PropertyCondition c1 = new PropertyCondition(AutomationElement.AutomationIdProperty, 
                       TestDataInfrastructure.GetControlId("CurrentProjectsList"));

                   AndCondition andCond = new AndCondition(c, c1);

                    elementList = Window.FindFirst(TreeScope.Descendants, andCond);
                }
                return elementList.CachedChildren;
              
            }
        }
      

        private static string GetDataFromResourceFile(string keyName)
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue(keyName);
        }

        #endregion
    }
}
