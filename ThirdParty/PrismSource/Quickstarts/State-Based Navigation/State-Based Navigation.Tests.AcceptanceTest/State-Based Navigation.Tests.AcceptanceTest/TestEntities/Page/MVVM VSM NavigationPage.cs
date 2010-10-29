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
using AcceptanceTestLibrary.Common;
using System.Windows.Automation;
using AcceptanceTestLibrary.TestEntityBase;
using AcceptanceTestLibrary.ApplicationHelper;

namespace StateBasedNavigation.Tests.AcceptanceTest.TestEntities.Page
{
    public static class StateBasedNavigationPage<TApp>
         where TApp : AppLauncherBase, new()
    
    {
        #region SILVERLIGHT
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }
        public static AutomationElement ListButton
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("ListButtonContent"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.RadioButton);
                AndCondition andCond = new AndCondition(cond, cond1);
                return Window.FindFirst(TreeScope.Descendants, andCond);
            }
        }

        public static AutomationElement AvatarButton
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("AvatarButtonContent"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.RadioButton);
                AndCondition andCond = new AndCondition(cond, cond1);
                return Window.FindFirst(TreeScope.Descendants, andCond);
            }
        }

        public static AutomationElement SendMessageButton
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.AutomationIdProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SendButton"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AndCondition andCond = new AndCondition(cond, cond1);
                return Window.FindFirst(TreeScope.Descendants, andCond);
            }
        }
        public static AutomationElementCollection FriendList
        {
            get { return PageBase<TApp>.FindControlByType("FriendsList"); }
        }

        public static AutomationElementCollection ComboBoxItems
        {
            get
            {
                return PageBase<TApp>.FindControlByType("Combo");
            }
        }

        public static AutomationElement AvatarView
        {
            get
            {
                return PageBase<TApp>.FindControlByAutomationId("AvatarsView");
            }
        }


        public static AutomationElement DisconnectedImage
        {
            get
            {
                return PageBase<TApp>.FindControlByAutomationId("DisconnectedImage");
            }
        }

        public static AutomationElement ContactDetails
        {
            get
            {               
                return PageBase<TApp>.FindControlByContent("ContactDetails");
            }
        }

        public static AutomationElement DetailsHeading
        {
            get
            {
                return PageBase<TApp>.FindControlByAutomationId("DetailsHeading");
            }
        }

        public static AutomationElement FriendImage
        {
            get
            {
                return PageBase<TApp>.FindControlByAutomationId("FriendImage");
            }
        }

        public static AutomationElement SendMessageWindow
        {
            get
            {
                return PageBase<TApp>.FindControlByAutomationId("SendMessageWindow");
            }
        }

        public static AutomationElement SendMessageTextBox
        {
            get
            {
                return PageBase<TApp>.FindControlByAutomationId("MessageTextBox");
            }
        }

        public static AutomationElement SendButton
        {
            get
            {
                PropertyCondition cond = new PropertyCondition(AutomationElement.NameProperty,
                    new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SendButtonContent"));
                PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button);
                AndCondition andCond = new AndCondition(cond, cond1);
                return Window.FindFirst(TreeScope.Descendants, andCond);
            }
        }

        public static AutomationElementCollection AvatarViewFriends
        {
            get
            {
                // Set up the CacheRequest.
                CacheRequest cacheRequest = new CacheRequest();
                cacheRequest.Add(AutomationElement.ControlTypeProperty);
                cacheRequest.TreeScope = TreeScope.Element | TreeScope.Children;


                using (cacheRequest.Activate())
                {
                    PropertyCondition cond = new PropertyCondition(AutomationElement.AutomationIdProperty,
                       new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("AvatarsView"));
                    PropertyCondition cond1 = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.List);
                    AndCondition andCond = new AndCondition(cond, cond1);
                    return Window.FindFirst(TreeScope.Descendants, andCond).CachedChildren;
                   // return PageBase<TApp>.FindControlByAutomationId("AvatarsView");
                }
            }
        }

        public static AutomationElement SendingProgressbar
        {
            get
            {                
                PropertyCondition cond = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ProgressBar);               
                return Window.FindFirst(TreeScope.Descendants, cond);
            }
        }

        #endregion
    }
}
