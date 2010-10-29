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
using System.Windows.Automation;
using AcceptanceTestLibrary.TestEntityBase;
using AcceptanceTestLibrary.ApplicationHelper;

namespace Commanding.Tests.AcceptanceTest.TestEntities.Page
{
    public static class CommandingPage<TApp>
        where TApp : AppLauncherBase, new()
    {
       
        //private static Application app;
        //public static void LaunchApplication(string applicationPath, string windowTitle)
        //{
        //    try
        //    {
        //        app = Application.Launch(applicationPath);
        //        DesktopWindow = app.GetWindow(windowTitle, Core.Factory.InitializeOption.NoCache);
        //    }
        //    catch (Exception)
        //    {
        //        DisposeApplication();
        //    }
        //}



        #region Desktop
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }   

        public static AutomationElement SaveAllToolBarButton
        {
            get { return PageBase<TApp>.FindControlByAutomationId("SaveAllToolBarButton"); }
        }
        public static AutomationElement SaveButton
        {
            get { return PageBase<TApp>.FindControlByAutomationId("SaveButton"); }
        }
        public static AutomationElementCollection OrderListView
        {
            get { return PageBase<TApp>.FindControlByType("OrderListView"); }
        }
        public static AutomationElement DateLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("DateLabel"); }
        }
        public static AutomationElement OrderNameLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("OrderNameLabel"); }
        }
        public static AutomationElement PriceLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("PriceLabel"); }
        }
        public static AutomationElement QuantityLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("QuantityLabel"); }
        }
        public static AutomationElement ShippingLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ShippingLabel"); }
        }
        public static AutomationElement TotalLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TotalLabel"); }
        }
        public static AutomationElement DeliveryDateTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("DeliveryDateTextBox");}
        }
        public static AutomationElement PriceTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("PriceTextBox");}
        }
        public static AutomationElement QuantityTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("QuantityTextBox");}
        }
        public static AutomationElement ShippingTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ShippingTextBox");}
        }
        public static AutomationElement TotalTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TotalTextBox");}
        }
        
        //public static void DisposeApplication()
        //{
        //    if (app != null)
        //    {
        //        app.Kill();
        //    }
        //}
        #endregion

        #region Silverlight
    
        public static AutomationElement aeSaveAllToolBarButton
        {
            get { return PageBase<TApp>.FindControlByAutomationId("SaveAllToolBarButton"); }
        }
        public static AutomationElement aeSaveButton
        {
           get { return PageBase<TApp>.FindControlByAutomationId("SaveButton"); }
        }
        public static AutomationElementCollection aeOrderListView
        {
            //get { return Window.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, GetDataFromResourceFile("OrderListView"))); }
            get { return PageBase<TApp>.FindControlByType("OrderListView"); }
        }

        public static AutomationElement aeDateLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("DateLabel"); }
        }

        public static AutomationElement aeOrderNameLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("OrderNameLabel"); }
        }

        public static AutomationElement aePriceLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("PriceLabel"); }
        }

        public static AutomationElement aeQuantityLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("QuantityLabel"); }
        }

        public static AutomationElement aeShippingLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ShippingLabel"); }
        }

        public static AutomationElement aeTotalLabel
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TotalLabel"); }
        }

        public static AutomationElement aeDeliveryDateTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("DeliveryDateTextBox"); }
        }

        public static AutomationElement aePriceTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("PriceTextBox"); }
        }

        public static AutomationElement aeQuantityTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("QuantityTextBox"); }
        }

        public static AutomationElement aeShippingTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ShippingTextBox"); }
        }

        public static AutomationElement aeTotalTextBox
        {
           get { return PageBase<TApp>.FindControlByAutomationId("TotalTextBox"); }
        }
        
        private static string GetDataFromResourceFile(string keyName)
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue(keyName);
        }
        #endregion 
    }
}
