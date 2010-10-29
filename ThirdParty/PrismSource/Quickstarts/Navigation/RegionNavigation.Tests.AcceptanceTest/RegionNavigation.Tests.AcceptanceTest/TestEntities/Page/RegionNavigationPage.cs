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

namespace RegionNavigation.Tests.AcceptanceTest.TestEntities.Page
{
    public static class RegionNavigationPage<TApp>
         where TApp : AppLauncherBase, new()
    
    {
        #region SILVERLIGHT
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement EMailButton
        {
         get {return PageBase<TApp>.FindControlByAutomationId("EmailRadioButton");}
    
        }

        public static AutomationElement CalendarButton
        {
            get { return PageBase<TApp>.FindControlByAutomationId("CalendarRadioButton"); }

        }

        public static AutomationElement ContactDetailsButton
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ContactDetailsRadioButton"); }

        }

        public static AutomationElement ContactAvatarsButton
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ContactAvatarsRadioButton"); }

        }

        public static AutomationElement CalendarGrid
        {
            get { return PageBase<TApp>.FindControlByAutomationId("CalendarGrid"); }

        }

        public static AutomationElement NewEmailButton
        {
            get 
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("NewButton"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AndCondition andCond = new AndCondition(cond, cond1);
                AutomationElement aeElement = Window.FindFirst(TreeScope.Descendants, andCond);
                return aeElement;
            }
        }

        public static AutomationElement Email
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("Email"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AndCondition andCond = new AndCondition(cond, cond1);
                AutomationElement aeElement = Window.FindFirst(TreeScope.Descendants, andCond);
                return aeElement;
            }
        }

        public static AutomationElement ReplyEmailButton
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("ReplyButton"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AndCondition andCond = new AndCondition(cond, cond1);
                return Window.FindFirst(TreeScope.Descendants, andCond);
            }
        }

        public static AutomationElement DeleteEmailButton
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("DeleteButton"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AndCondition andCond = new AndCondition(cond, cond1);
                return Window.FindFirst(TreeScope.Descendants, andCond);
            }
        }

        public static AutomationElement SendEmailButton
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SendButton"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AndCondition andCond = new AndCondition(cond, cond1);
                return Window.FindFirst(TreeScope.Descendants, andCond);
            }
        }

        public static AutomationElementCollection Hyperlinks
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.AutomationIdProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("OpenMailHyperLink"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Hyperlink);
                AndCondition andCond = new AndCondition(cond, cond1);
                return Window.FindAll(TreeScope.Descendants, andCond);
            }
        }

        public static AutomationElement MessagesList
        {
            get { return PageBase<TApp>.FindControlByAutomationId("MessagesList"); }

        }

        public static AutomationElement ContactsList
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ContactsList"); }

        }

        public static AutomationElement SubjectTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("SubjectTextBox"); }

        }

        public static AutomationElement ToTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ToTextBox"); }

        }

        public static AutomationElement EmailTextBox
        {
            get { return PageBase<TApp>.FindControlByAutomationId("EmailTextBox"); }

        }

        public static AutomationElement FromTextBlockData
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TextBlockFromData"); }

        }

        public static AutomationElement TextBlockTo
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TextBlockTo"); }

        }

        public static AutomationElement TextBlockToData
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TextBlockToData"); }

        }

        public static AutomationElement TextBlockSubject
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TextBlockSubject"); }

        }

        public static AutomationElement TextBlockSubjectData
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TextBlockSubjectData"); }

        }

        public static AutomationElement TextBlockEmailData
        {
            get { return PageBase<TApp>.FindControlByAutomationId("TextBlockEmailData"); }

        }

        public static AutomationElement FromDataBlock
        {
            get { return PageBase<TApp>.FindControlByAutomationId("FromDataBlock"); }

        }

        public static AutomationElement ToBlock
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ToBlock"); }

        }

        public static AutomationElement ToBlockData
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ToBlockData"); }

        }

        public static AutomationElement SubjectBlock
        {
            get { return PageBase<TApp>.FindControlByAutomationId("SubjectBlock"); }

        }

        public static AutomationElement SubjectDataBlock
        {
            get { return PageBase<TApp>.FindControlByAutomationId("SubjectDataBlock"); }

        }

        public static AutomationElement EmailTextBlock
        {
            get { return PageBase<TApp>.FindControlByAutomationId("EmailTextBlock"); }

        }

        public static AutomationElementCollection OpenButton
        {
            get { return PageBase<TApp>.FindAllControlsByAutomationId("OpenButton"); }

        }    


        public static AutomationElementCollection ChildElements(AutomationElement parent)
        {
             return PageBase<TApp>.FindControlsByParent(parent); 
        }
        #endregion
    
    }
}
