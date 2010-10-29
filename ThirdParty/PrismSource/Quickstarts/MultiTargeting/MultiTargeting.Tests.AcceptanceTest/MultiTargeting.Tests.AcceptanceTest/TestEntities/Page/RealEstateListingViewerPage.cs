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
using AcceptanceTestLibrary.ApplicationHelper;
using AcceptanceTestLibrary.TestEntityBase;

namespace MultiTargeting.Tests.AcceptanceTest.TestEntities.Page
{
    public static class RealEstateListingViewerPage<TApp>
        where TApp : AppLauncherBase, new()
    {
        public static AutomationElement Window
        {
            get { return PageBase<TApp>.Window; }
            set { PageBase<TApp>.Window = value; }
        }

        public static AutomationElement GarageImage
        {
            get { return PageBase<TApp>.FindControlByAutomationId("GarageImgAutomation"); }
        }

        public static AutomationElement BathroomsImage
        {
            get { return PageBase<TApp>.FindControlByAutomationId("BathroomsImgAutomation"); }
        }

        public static AutomationElement BedroomsImage
        {
            get { return PageBase<TApp>.FindControlByAutomationId("BedroomsImgAutomation"); }
        }

        public static AutomationElement AcerageImage
        {
            get { return PageBase<TApp>.FindControlByAutomationId("AcerageImgAutomation"); }
        }

        public static AutomationElement PropertyImage
        {
            get { return PageBase<TApp>.FindControlByAutomationId("PropertyImgAutomation"); }
        }

        public static AutomationElement AddressAboveHouseImageText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("AddressAboveHouseImageTxtAutomation"); }
        }

        public static AutomationElement CountyAboveHouseImageText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("CountyAboveHouseImageTxtAutomation"); }
        }

        public static AutomationElement StateAboveHouseImageText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("StateAboveHouseImageTxtAutomation"); }
        }

        public static AutomationElement ZipCodeText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("ZipCodeTxtAutomation"); }
        }

        public static AutomationElement BathroomsText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("BathroomsTxtAutomation"); }
        }

        public static AutomationElement GarageSizeText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("GarageSizeTxtAutomation"); }
        }

        public static AutomationElement BedroomsText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("BedroomsTxtAutomation"); }
        }

        public static AutomationElement AcerageText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("AcerageTxtAutomation"); }
        }

        public static AutomationElement PriceText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("PriceTxtAutomation"); }
        }

        public static AutomationElement DescriptionLabelText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("DescriptionLabelTxtAutomation"); }
        }

        public static AutomationElement DescriptionText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("DescriptionTxtAutomation"); }
        }

        public static AutomationElement AddressBelowHouseImageText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("AddressBelowHouseImageTxtAutomation"); }
        }

        public static AutomationElement CountyBelowHouseImageText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("CountyBelowHouseImageTxtAutomation"); }
        }

        public static AutomationElement StateBelowHouseImageText
        {
            get { return PageBase<TApp>.FindControlByAutomationId("StateBelowHouseImageTxtAutomation"); }
        }
    }
}
