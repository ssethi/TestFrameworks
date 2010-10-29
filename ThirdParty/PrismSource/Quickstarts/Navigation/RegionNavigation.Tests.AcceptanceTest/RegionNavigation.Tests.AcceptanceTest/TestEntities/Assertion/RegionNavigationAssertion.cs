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
using AcceptanceTestLibrary.UIAWrapper;
using AcceptanceTestLibrary.ApplicationHelper;
using System.Windows.Automation;
using RegionNavigation.Tests.AcceptanceTest.TestEntities.Page;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace RegionNavigation.Tests.AcceptanceTest.TestEntities.Assertion
{
    public static class RegionNavigationAssertion<TApp>
        where TApp : AppLauncherBase, new()
    {
        #region Silverlight

        public static void AssertRN_MailRegionOnLoad()
        {
            //Find Email radio button and click on it
            SelectEmailOption();     

            //Check for New button
            AutomationElement aeNewButton = RegionNavigationPage<TApp>.NewEmailButton;
            Assert.IsNotNull(aeNewButton, "New Email button is not loaded");

            //Check for Reply button
            AutomationElement aeReplyButton = RegionNavigationPage<TApp>.ReplyEmailButton;
            Assert.IsNotNull(aeReplyButton, "Reply Email button is not loaded");
            Assert.IsFalse(aeReplyButton.Current.IsEnabled, "Reply button is not disabled on load");

            //Check for delete button
            AutomationElement aeDeleteButton = RegionNavigationPage<TApp>.DeleteEmailButton;
            Assert.IsNotNull(aeDeleteButton, "Delete Email button is not loaded");

            //Check for messages list
            AutomationElement aeMessagesList = RegionNavigationPage<TApp>.MessagesList;
            Assert.IsNotNull(aeMessagesList, "Messages are not loaded");

            //Check for messages 
            AutomationElementCollection aeMessages = RegionNavigationPage<TApp>.ChildElements(aeMessagesList);
            Assert.IsTrue(aeMessages.Count > 0, "Messages are not loaded in Messages View");
       
        }

        public static void AssertRN_SelectAMessage()
        {
            SelectEmailOption();
            //Check for messages list
            AutomationElement aeMessagesList = RegionNavigationPage<TApp>.MessagesList;
            Assert.IsNotNull(aeMessagesList, "Messages are loaded");

            //Check for messages 
            AutomationElementCollection aeMessages = RegionNavigationPage<TApp>.ChildElements(aeMessagesList);
            Assert.IsTrue(aeMessages.Count > 0, "Messages are loaded in Messages View");
            aeMessages[1].Select();
            Thread.Sleep(2000);

            AutomationElement aeFromTB = RegionNavigationPage<TApp>.FromDataBlock;
            Assert.IsNotNull(aeFromTB, "From Details are not loaded");

            AutomationElement aeToTB = RegionNavigationPage<TApp>.ToBlock;
            Assert.IsNotNull(aeToTB, "To Block are not loaded");

            AutomationElement aeToDataTB = RegionNavigationPage<TApp>.ToBlockData;
            Assert.IsNotNull(aeToDataTB, "To Details are not loaded");

            AutomationElement aeSubjectTB = RegionNavigationPage<TApp>.SubjectBlock;
            Assert.IsNotNull(aeSubjectTB, "Subject Block are not loaded");

            AutomationElement aeSubjectDataTB = RegionNavigationPage<TApp>.SubjectDataBlock;
            Assert.IsNotNull(aeSubjectDataTB, "Subject Details are not loaded");

            AutomationElement aeEmailTB = RegionNavigationPage<TApp>.EmailTextBlock;
            Assert.IsNotNull(aeEmailTB, "Email Text is not loaded");


        }

        public static void AssertRN_LoadCalendar()
        {
            SelectCalendarOption();
            AutomationElement aeCalendar = RegionNavigationPage<TApp>.CalendarGrid;
            Assert.IsNotNull(aeCalendar, "Calendar is not loaded");

            //Check for calendar items
            AutomationElementCollection aeCalendarItems = RegionNavigationPage<TApp>.ChildElements(aeCalendar);
            Assert.IsTrue(aeCalendarItems.Count > 0, "Calendar items are loaded in Calendar View");
           
        }

        public static void AssertRN_ClickHyperlinkOnCalendar()
        {
            SelectCalendarOption();
            AutomationElementCollection aeHyperLinks = RegionNavigationPage<TApp>.Hyperlinks;
            aeHyperLinks[0].Click();
            Thread.Sleep(2000);

            Assert_EmailView();
        }

        public static void AssertRN_LoadContactDetails()
        {
            SelectContactDetailsOption();
            AutomationElement aeContactsList = RegionNavigationPage<TApp>.ContactsList;
            Assert.IsNotNull(aeContactsList, "Contacts are not loaded");

            //Check for Contacts 
            AutomationElementCollection aeContacts = RegionNavigationPage<TApp>.ChildElements(aeContactsList);
            Assert.IsTrue(aeContacts.Count > 0, "Contacts are loaded in Contacts View");

            AutomationElement aeEMailButton = RegionNavigationPage<TApp>.EMailButton;
            Assert.IsNotNull(aeEMailButton, "Email button is not loaded on contact details page");
           
        }

        public static void AssertRN_LoadContactAvatars()
        {
            SelectContactAvatarsOptions();
            AutomationElement aeContactsList = RegionNavigationPage<TApp>.ContactsList;
            Assert.IsNotNull(aeContactsList, "Contacts are not loaded");

            //Check for Contacts 
            AutomationElementCollection aeContacts = RegionNavigationPage<TApp>.ChildElements(aeContactsList);
            Assert.IsTrue(aeContacts.Count > 0, "Contacts are loaded in Contacts View");

            AutomationElement aeEMailButton = RegionNavigationPage<TApp>.EMailButton;
            Assert.IsNotNull(aeEMailButton, "Email button is not loaded on contact Avatars page");

        }

        public static void AssertRN_SendEmailFromContactDetailsPage()
        {
            SelectContactDetailsOption();

            AutomationElement aeContactsList = RegionNavigationPage<TApp>.ContactsList;
            Assert.IsNotNull(aeContactsList, "Contacts are not loaded");

            //Check for Contacts 
            AutomationElementCollection aeContacts = RegionNavigationPage<TApp>.ChildElements(aeContactsList);
            Assert.IsTrue(aeContacts.Count > 0, "Contacts are loaded in Contacts View");

            //Selct 2nd contact
            aeContacts[1].Select();
            Thread.Sleep(2000);

            Assert_SendEmailFromContacts();

           
        }

        public static void AssertRN_SendEmailFromContactsAvatarPage()
        {
            SelectContactAvatarsOptions();

            AutomationElement aeContactsList = RegionNavigationPage<TApp>.ContactsList;
            Assert.IsNotNull(aeContactsList, "Contacts are not loaded");

            //Check for Contacts 
            AutomationElementCollection aeContacts = RegionNavigationPage<TApp>.ChildElements(aeContactsList);
            Assert.IsTrue(aeContacts.Count > 0, "Contacts are loaded in Contacts View");

            //Selct 2nd contact
            aeContacts[1].Select();
            Thread.Sleep(2000);

            Assert_SendEmailFromContacts();


        }

        public static void AssertRN_SendNewEmail()
        {
            SelectEmailOption();

            AutomationElement aeNewButton = RegionNavigationPage<TApp>.NewEmailButton;
            aeNewButton.Click();
            Thread.Sleep(2000);

            AutomationElement aeTo = RegionNavigationPage<TApp>.ToTextBox;
            Assert.IsNotNull(aeTo, "To textbox is not loaded");
            Assert.AreEqual(aeTo.GetValue(), string.Empty, "To Text box value is not null");

            AutomationElement aeSubject = RegionNavigationPage<TApp>.SubjectTextBox;
            Assert.IsNotNull(aeSubject, "Subject textbox is not loaded");
            Assert.AreEqual(aeSubject.GetValue(), string.Empty, "Subject Text box value is not null");

            aeSubject.SetFocus();
            aeSubject.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Subject"));
            Thread.Sleep(2000);

            AutomationElement aeEmailText = RegionNavigationPage<TApp>.EmailTextBox;
            Assert.IsNotNull(aeEmailText, "Email TextBox is not loaded");
            Assert.AreEqual(aeEmailText.GetValue(), string.Empty, "Email Text box value is not null");

            aeEmailText.SetFocus();
            aeEmailText.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("EmailText"));
            Thread.Sleep(2000);

            AutomationElement aeSendEmailButton = RegionNavigationPage<TApp>.SendEmailButton;
            Assert.IsNotNull(aeSendEmailButton, "Send button is not loaded");

            aeSendEmailButton.Click();
            Thread.Sleep(2000);

            //Check



        }

        public static void AssertRN_ReplyAnEmail()
        {
            SelectEmailOption();

            //Check for messages list
            AutomationElement aeMessagesList = RegionNavigationPage<TApp>.MessagesList;
            Assert.IsNotNull(aeMessagesList, "Messages are not loaded");

            //Check for messages 
            AutomationElementCollection aeMessages = RegionNavigationPage<TApp>.ChildElements(aeMessagesList);
            aeMessages[0].Select();
            Thread.Sleep(2000);       

            AutomationElement aeReplyButton = RegionNavigationPage<TApp>.ReplyEmailButton;
            aeReplyButton.Click();
            Thread.Sleep(2000);

            AutomationElement aeTo = RegionNavigationPage<TApp>.ToTextBox;
            Assert.IsNotNull(aeTo, "To textbox is not loaded");
            Assert.AreNotEqual(aeTo.GetValue(), string.Empty, "To Text box value is null");

            AutomationElement aeSubject = RegionNavigationPage<TApp>.SubjectTextBox;
            Assert.IsNotNull(aeSubject, "Subject textbox is not loaded");
            Assert.AreNotEqual(aeSubject.GetValue(), string.Empty, "Subject Text box value is null");

            AutomationElement aeEmailText = RegionNavigationPage<TApp>.EmailTextBox;
            Assert.IsNotNull(aeEmailText, "Email TextBox is not loaded");
            Assert.AreNotEqual(aeEmailText.GetValue(), string.Empty, "Email Text box value is null");

            aeEmailText.SetFocus();
            aeEmailText.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("EmailText"));
            Thread.Sleep(2000);

            AutomationElement aeSendEmailButton = RegionNavigationPage<TApp>.SendEmailButton;
            Assert.IsNotNull(aeSendEmailButton, "Send button is not loaded");

            aeSendEmailButton.Click();
            Thread.Sleep(2000);

            //Check





        }

        public static void AssertRN_DeleteEmail()
        {
            SelectEmailOption();

            //Check for messages list
            AutomationElement aeMessagesList = RegionNavigationPage<TApp>.MessagesList;
            Assert.IsNotNull(aeMessagesList, "Messages are not loaded");

            //Check for messages 
            AutomationElementCollection aeMessages = RegionNavigationPage<TApp>.ChildElements(aeMessagesList);
            aeMessages[0].Select();
            Thread.Sleep(2000);

            AutomationElement aeDeleteButton = RegionNavigationPage<TApp>.DeleteEmailButton;
            aeDeleteButton.Click();
            Thread.Sleep(2000);


        }

        public static void AssertRN_OpenEmail()
        {
            SelectEmailOption();

            //Check for messages list
            AutomationElement aeMessagesList = RegionNavigationPage<TApp>.MessagesList;
            Assert.IsNotNull(aeMessagesList, "Messages are not loaded");

            //Check for messages 
            AutomationElementCollection aeMessages = RegionNavigationPage<TApp>.ChildElements(aeMessagesList);
            aeMessages[0].Select();
            Thread.Sleep(2000);

            AutomationElementCollection aeOpenButtons = RegionNavigationPage<TApp>.OpenButton;
            aeOpenButtons[0].Click();
            Thread.Sleep(2000);

            Assert_EmailView();
        }

        public static void Assert_EmailView()
        {
            AutomationElement aeFromTB = RegionNavigationPage<TApp>.FromTextBlockData;
            Assert.IsNotNull(aeFromTB, "From Details are not loaded");

            AutomationElement aeToTB = RegionNavigationPage<TApp>.TextBlockTo;
            Assert.IsNotNull(aeToTB, "To Block are not loaded");

            AutomationElement aeToDataTB = RegionNavigationPage<TApp>.TextBlockToData;
            Assert.IsNotNull(aeToDataTB, "To Details are not loaded");

            AutomationElement aeSubjectTB = RegionNavigationPage<TApp>.TextBlockSubject;
            Assert.IsNotNull(aeSubjectTB, "Subject Block are not loaded");

            AutomationElement aeSubjectDataTB = RegionNavigationPage<TApp>.TextBlockSubjectData;
            Assert.IsNotNull(aeSubjectDataTB, "Subject Details are not loaded");

            AutomationElement aeEmailTB = RegionNavigationPage<TApp>.TextBlockEmailData;
            Assert.IsNotNull(aeEmailTB, "Email Text is not loaded");

        }
        public static void Assert_SendEmailFromContacts()
        {
            AutomationElement aeEMailButton = RegionNavigationPage<TApp>.Email;
            Assert.IsNotNull(aeEMailButton, "Email button is not loaded on contact Avatars page");

            //Click Email button
            aeEMailButton.Click();
            Thread.Sleep(2000);

            AutomationElement aeTo = RegionNavigationPage<TApp>.ToTextBox;
            Assert.IsNotNull(aeTo, "To textbox is not loaded");
            Assert.IsNotNull(aeTo.GetValue(), "To Text box value is null");

            AutomationElement aeSubject = RegionNavigationPage<TApp>.SubjectTextBox;
            Assert.IsNotNull(aeSubject, "Subject textbox is not loaded");

            aeSubject.SetFocus();
            aeSubject.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("Subject"));
            Thread.Sleep(2000);

            AutomationElement aeEmailText = RegionNavigationPage<TApp>.EmailTextBox;
            Assert.IsNotNull(aeEmailText, "Email TextBox is not loaded");

            aeEmailText.SetFocus();
            aeEmailText.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("EmailText"));
            Thread.Sleep(2000);

            AutomationElement aeSendEmailButton = RegionNavigationPage<TApp>.SendEmailButton;
            Assert.IsNotNull(aeSendEmailButton, "Send button is not loaded");

            aeSendEmailButton.Click();
            Thread.Sleep(2000);

            //Check


        }      
        #endregion

       
      

        public static void SelectEmailOption()
        {
            //Find Email radio button and click on it
            AutomationElement aeMailButton = RegionNavigationPage<TApp>.EMailButton;
            aeMailButton.Select();
            System.Windows.Forms.Cursor.Position =
                new System.Drawing.Point((int)aeMailButton.GetClickablePoint().X, (int)aeMailButton.GetClickablePoint().Y);
            System.Threading.Thread.Sleep(1000);
            MouseEvents.Click();
            Thread.Sleep(2000);  
     
        }

        public static void SelectCalendarOption()
        {
            //Find Calendar radio button and click on it
            AutomationElement aeCalendarButton = RegionNavigationPage<TApp>.CalendarButton;
            aeCalendarButton.Select();
            System.Windows.Forms.Cursor.Position =
                new System.Drawing.Point((int)aeCalendarButton.GetClickablePoint().X, (int)aeCalendarButton.GetClickablePoint().Y);
            System.Threading.Thread.Sleep(1000);
            MouseEvents.Click();
            Thread.Sleep(2000);

        }

        public static void SelectContactDetailsOption()
        {

            //Find Calendar radio button and click on it
            AutomationElement aeContactDetailsButton = RegionNavigationPage<TApp>.ContactDetailsButton;
            aeContactDetailsButton.Select();
            System.Windows.Forms.Cursor.Position =
                new System.Drawing.Point((int)aeContactDetailsButton.GetClickablePoint().X, (int)aeContactDetailsButton.GetClickablePoint().Y);
            System.Threading.Thread.Sleep(1000);
            MouseEvents.Click();
            Thread.Sleep(2000);
        }

        public static void SelectContactAvatarsOptions()
        {
            //Find Calendar radio button and click on it
            AutomationElement aeContactAvatarsButton = RegionNavigationPage<TApp>.ContactAvatarsButton;
            aeContactAvatarsButton.Select();
            System.Windows.Forms.Cursor.Position =
                new System.Drawing.Point((int)aeContactAvatarsButton.GetClickablePoint().X, (int)aeContactAvatarsButton.GetClickablePoint().Y);
            System.Threading.Thread.Sleep(1000);
            MouseEvents.Click();
            Thread.Sleep(2000);

        }
    
        public static AutomationElement RegionNavigationPage { get; set; }
    }
}
