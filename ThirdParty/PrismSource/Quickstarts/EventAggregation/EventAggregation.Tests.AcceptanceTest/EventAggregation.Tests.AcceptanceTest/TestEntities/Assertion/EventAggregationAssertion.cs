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
using AcceptanceTestLibrary.UIAWrapper;
using AcceptanceTestLibrary.ApplicationHelper;
using System.Windows.Automation;
using EventAggregation.Tests.AcceptanceTest.TestEntities.Page;
using System.Globalization;
using System.Threading;

namespace EventAggregation.Tests.AcceptanceTest.TestEntities.Assertion
{
    public static class EventAggregationAssertion<TApp>
        where TApp : AppLauncherBase, new()
    {
        #region Silverlight
        public static void AssertAddFundToCustomer()
        {
            AutomationElement customerCombobox = EventAggregationPage<TApp>.CustomerCombo1;
            //1. Get the handle of the Customer combo box and select Customer1
            Assert.IsNotNull(customerCombobox, "Could not find Customer combobox");
            customerCombobox.SetFocus();
            customerCombobox.Expand();
            Thread.Sleep(2000);

            AutomationElementCollection customerComboItems = EventAggregationPage<TApp>.CustomerComboItems;                       
            customerComboItems[0].Select();
            Thread.Sleep(2000);    


            //2. Get the handle of the Fund combo box and select FundA
            AutomationElement fundCombobox = EventAggregationPage<TApp>.FundCombo;
            Assert.IsNotNull(fundCombobox, "Could not find Fund combobox");
            fundCombobox.SetFocus();
            fundCombobox.Expand();

            AutomationElementCollection fundComboItems = EventAggregationPage<TApp>.FundComboItems;
            Assert.IsNotNull(fundComboItems, "Could not find items in fund combobox");          
            fundComboItems[0].Select();
            Thread.Sleep(2000);              

            

            //3. Get the handle of the Add button and click on it
            AutomationElement addButton = EventAggregationPage<TApp>.AddFundButton;
            Assert.IsNotNull(addButton, "Could not find Add button");

            addButton.Click();
            Thread.Sleep(2000);
            //4. Get the handle of Activity Label and check the content
            Assert.AreEqual(EventAggregationPage<TApp>.ActivityLabelElement.Current.Name, GetDataFromTestDataFile("Customer1ActivityLabelText"));
            bool isTextFound = false;
            foreach (AutomationElement textBox in EventAggregationPage<TApp>.AllTextBoxes)
            {
                if (textBox.Current.Name.Equals(GetDataFromTestDataFile("DefaultFund"), StringComparison.CurrentCulture))
                {
                    isTextFound = true;
                    break;
                }
            }

            Assert.IsTrue(isTextFound, "FundA is not added");

        }

        public static void AssertAddMultipleFundsToCustomer()
        {

            AutomationElement customerCombobox = EventAggregationPage<TApp>.CustomerCombo1;
            //1. Get the handle of the Customer combo box and select Customer1
            Assert.IsNotNull(customerCombobox, "Could not find Customer combobox");

            customerCombobox.Expand();
            System.Threading.Thread.Sleep(3000);

            AutomationElementCollection customerComboItems = EventAggregationPage<TApp>.CustomerComboItems;          
            Assert.IsNotNull(customerComboItems, "Could not find items in customer combobox");         
            customerComboItems[0].Select();
            Thread.Sleep(2000);   


            //2. Get the handle of the Fund combo box and select FundA
            AutomationElement fundCombobox = EventAggregationPage<TApp>.FundCombo;
            Assert.IsNotNull(fundCombobox, "Could not find Fund combobox");

            fundCombobox.Expand();
            System.Threading.Thread.Sleep(3000);

            AutomationElementCollection fundComboItems = EventAggregationPage<TApp>.FundComboItems;            
            Assert.IsNotNull(fundComboItems, "Could not find items in fund combobox");
            fundComboItems[0].Select();
            Thread.Sleep(2000);                


            //3. Get the handle of the Add button and click on it
            AutomationElement addButton = EventAggregationPage<TApp>.AddFundButton;
            Assert.IsNotNull(addButton, "Could not find Add button");

            addButton.Click();
            Thread.Sleep(2000);
            bool isTextFound = false;
            foreach (AutomationElement textBox in EventAggregationPage<TApp>.AllTextBoxes)
            {
                if (textBox.Current.Name.Equals(GetDataFromTestDataFile("DefaultFund"), StringComparison.CurrentCulture))
                {
                    isTextFound = true;
                    break;
                }
            }

            Assert.IsTrue(isTextFound, "FundA is not added");
        }

        private static AutomationElement GetHandleByAutomationId(string controlId)
        {
            AutomationElement win = EventAggregationPage<TApp>.Window;

            //find control using AutomationElement of window 
            return win.GetHandleById<TApp>(controlId);
        }

        #endregion

        #region Desktop
        public static void DesktopAssertAddMultipleFundsToCustomer()
        {
            //Add Default fund to default customer
            AutomationElementCollection aeCustItems = EventAggregationPage<TApp>.CustomerComboItems;
            aeCustItems[1].Select();       
           
            Thread.Sleep(2000);
            AutomationElementCollection aeFundItems = EventAggregationPage<TApp>.FundComboItems;
            aeFundItems[1].Select();            
            Thread.Sleep(2000);
            EventAggregationPage<TApp>.AddButton.Click();

            //Select another fund to default customer
            Thread.Sleep(2000);
            EventAggregationPage<TApp>.FundComboItems[2].Select();            
            Thread.Sleep(2000);
            EventAggregationPage<TApp>.AddButton.Click();

            //Assert Activity View
            Assert.AreEqual(EventAggregationPage<TApp>.ActivityLabel.Current.Name, GetDataFromTestDataFile("Customer1ActivityLabelText"));

            Assert.IsNotNull(EventAggregationPage<TApp>.GetFundsLabel("DefaultFund"));
            Assert.IsNotNull(EventAggregationPage<TApp>.GetFundsLabel("AnotherFund"));
        }


        public static void DesktopAssertAddRepeatedFundsToCustomer()
        {

            int numberOfAddClicks = 3;

            //Add Default fund to default customer repeatedly.
            EventAggregationPage<TApp>.CustomerComboBox.Expand();
            Thread.Sleep(2000);
            EventAggregationPage<TApp>.CustomerComboItems[1].Select();          
            Thread.Sleep(2000);
            EventAggregationPage<TApp>.FundComboItems[1].Select();           
            Thread.Sleep(2000);
            EventAggregationPage<TApp>.AddButton.Click();
            Thread.Sleep(2000);
            EventAggregationPage<TApp>.AddButton.Click();
            Thread.Sleep(2000);
            EventAggregationPage<TApp>.AddButton.Click();

            Assert.AreEqual(EventAggregationPage<TApp>.ActivityLabel.Current.Name, GetDataFromTestDataFile("Customer1ActivityLabelText"));

            //For every Add button click, Check if the selected fund is added to the selected customer repeatedly.           
            Assert.AreEqual(EventAggregationPage<TApp>.GetAllFundsLabels("DefaultFund").Count-1, numberOfAddClicks, "Funds is not added for all button clicks");
            
        }

        public static void DesktopAssertEachCustomerShouldHaveAnActivityView()
        {
            AutomationElementCollection aeCustomers = EventAggregationPage<TApp>.CustomerComboItems;
            //For every customer in the customer combo box,check if a corresponding article view is displayed
            for (int count = 1; count < aeCustomers.Count -1; count++)
            {
                Assert.IsNotNull(EventAggregationPage<TApp>.GetFundsLabelByAutomationId(GetDataFromResourceFile("ActivityLabelTextValue") + " " + aeCustomers[count].Current.Name, GetDataFromResourceFile("ActivityLabel")));
            }
                
        }

        public static void DesktopAssertSelectedFundIsAddedOnlyToTheSelectedCustomer()
        {
            string[] selectedCustomer = new string[2];
            string[] selectedFund = new string[2];

            const int CUSTOMERS_IN_DROPDOWN = 2;

            for (int loopCounter = 0; loopCounter < CUSTOMERS_IN_DROPDOWN; loopCounter++)
            {
                AutomationElement customerCombo = EventAggregationPage<TApp>.CustomerComboBox;
                customerCombo.SetFocus();
                System.Windows.Forms.SendKeys.SendWait(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Customer" + loopCounter.ToString(CultureInfo.InvariantCulture)));
                selectedCustomer[loopCounter] = customerCombo.Current.Name;
               
                AutomationElement fundCombo = EventAggregationPage<TApp>.FundComboBox;
                fundCombo.SetFocus();
                System.Windows.Forms.SendKeys.SendWait(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Fund" + loopCounter.ToString(CultureInfo.InvariantCulture)));
                selectedFund[loopCounter] = fundCombo.Current.Name;

                EventAggregationPage<TApp>.AddButton.Click();
            }

            //int counter = 0;
            //foreach (AutomationElement element in EventAggregationPage<TApp>.ElementsInActivityView)
            //{
            //    if (counter < CUSTOMERS_IN_DROPDOWN)
            //    {
            //        // Find the text box and the fund text associated.
            //        AutomationElement textBox = element.SearchInRawTreeByName(GetDataFromResourceFile("ActivityLabel"));
            //        string textBoxValue = textBox.Current.Name;

            //        string expectedValue = GetDataFromResourceFile("ActivityLabelTextValue") + " " + selectedCustomer[counter].ToString();
            //        Assert.AreEqual(expectedValue, textBoxValue);

            //        // Find the fund values
            //        AutomationElement fund = element.SearchInRawTreeByName(selectedFund[counter].ToString());
            //        Assert.AreEqual(fund.Current.Name, selectedFund[counter].ToString());
            //        counter++;
            //    }
            //}
        }

        private static string GetDataFromResourceFile(string keyName)
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue(keyName);
        }

        private static string GetDataFromTestDataFile(string keyName)
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue(keyName);
        }
        #endregion
    }
}
