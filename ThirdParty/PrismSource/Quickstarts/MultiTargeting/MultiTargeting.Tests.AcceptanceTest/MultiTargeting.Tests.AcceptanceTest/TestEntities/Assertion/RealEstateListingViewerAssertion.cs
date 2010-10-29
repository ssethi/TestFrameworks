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
using MultiTargeting.Tests.AcceptanceTest.TestEntities.Page;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTestLibrary.ApplicationHelper;

namespace MultiTargeting.Tests.AcceptanceTest.TestEntities.Assertion
{
    public static class RealEstateListingViewerAssertion<TApp>
        where TApp : AppLauncherBase, new()
    {

        #region LOADING OF CONTROLS

        public static void AssertGarageImage()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.GarageImage, "Garage image is not loaded.");
        }

        public static void AssertBathroomsImage()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.BathroomsImage, "Bathroom image is not loaded.");
        }

        public static void AssertBedroomsImage()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.BedroomsImage, "Bedroom image is not loaded.");
        }

        public static void AssertAcerageImage()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.AcerageImage, "Acerage image is not loaded.");
        }

        public static void AssertPropertyImage()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.PropertyImage, "Property image is not loaded.");
        }

        public static void AssertAddressTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.AddressAboveHouseImageText, "Address TextBlock (Above House Image) is not loaded.");
        }

        public static void AssertCountyTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.CountyAboveHouseImageText, "County TextBlock (Above House Image) is not loaded.");
        }

        public static void AssertStateTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.StateAboveHouseImageText, "State TextBlock (Above House Image) is not loaded.");
        }

        public static void AssertZipCodeTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.ZipCodeText, "Zipcode TextBlock is not loaded.");
        }

        public static void AssertBathroomsTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.BathroomsText, "Bathrooms TextBlock is not loaded.");
        }

        public static void AssertGarageSizeTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.GarageSizeText, "Garage Size TextBlock is not loaded.");
        }

        public static void AssertBedroomsTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.BedroomsText, "Bedrooms TextBlock is not loaded.");
        }

        public static void AssertAcerageTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.AcerageText, "Acerage TextBlock is not loaded.");
        }

        public static void AssertPriceTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.PriceText, "Price TextBlock is not loaded.");
        }

        public static void AssertDescriptionLabelTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.DescriptionLabelText, "Description label is not loaded.");
        }

        public static void AssertDescriptionTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.DescriptionText, "Description TextBlock is not loaded.");
        }

        public static void AssertAddressBelowHouseImageTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.AddressBelowHouseImageText, "Address TextBlock below house image is not loaded.");
        }

        public static void AssertCountyBelowHouseImageTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.CountyBelowHouseImageText, "County TextBlock below house image is not loaded.");
        }

        public static void AssertStateBelowHouseImageTextBlockLoading()
        {
            InternalAssertControl(RealEstateListingViewerPage<TApp>.StateBelowHouseImageText, "State TextBlock below house image is not loaded.");
        }


        #endregion

        #region CONTENT OF CONTROLS

        public static void AssertAddressTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.AddressAboveHouseImageText, "Address", "Actual and expected address are not same.");
        }

        public static void AssertCountyTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.CountyAboveHouseImageText, "County", "Actual and expected county are not same.");
        }

        public static void AssertStateTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.StateAboveHouseImageText, "State", "Actual and expected state are not same.");
        }

        public static void AssertZipCodeTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.ZipCodeText, "ZipCode", "Actual and expected zipcode are not same.");
        }

        public static void AssertBathroomsTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.BathroomsText, "Bathrooms", "Actual and expected bathroom values are not same.");
        }

        public static void AssertGarageSizeTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.GarageSizeText, "GarageSize", "Actual and expected size of garage are not same.");
        }

        public static void AssertBedroomsTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.BedroomsText, "Bedrooms", "Actual and expected bedroom values are not same.");
        }

        public static void AssertAcerageTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.AcerageText, "Acerage", "Actual and expected acerage values are not same.");
        }

        public static void AssertPriceTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.PriceText, "Price", "Actual and expected price are not same.");
        }

        public static void AssertDescriptionLabelTextBlockContentSilverlight()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.DescriptionLabelText, "DescriptionLabelSilverlight", "Actual and expected description label values are not same.");
        }

        public static void AssertDescriptionLabelTextBlockContentWPF()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.DescriptionLabelText, "DescriptionLabelWpf", "Actual and expected description label values are not same.");
        }

        public static void AssertDescriptionTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.DescriptionText, "DescriptionText", "Actual and expected description text values are not same.");
        }

        public static void AssertAddressBelowHouseImageTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.AddressBelowHouseImageText, "Address", "Actual and expected address are not same.");
        }

        public static void AssertCountyBelowHouseImageTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.CountyBelowHouseImageText, "County", "Actual and expected county are not same.");
        }

        public static void AssertStateBelowHouseImageTextBlockContent()
        {
            InternalAssertContent(RealEstateListingViewerPage<TApp>.StateBelowHouseImageText, "State", "Actual and expected state are not same.");
        }

        
        #endregion

        private static void InternalAssertControl(AutomationElement control, string message)
        {
            Assert.IsNotNull(control, message);
        }

        private static void InternalAssertContent(AutomationElement aeControl, string nameValue, string message)
        {
            //find text block using AutomationElement of window
            string actualValue;
            string expectedValue;
            ResXConfigHandler resx = new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile"));

            actualValue = aeControl.Current.Name;
            expectedValue = resx.GetValue(nameValue);
            Assert.AreEqual(expectedValue, actualValue, true, message);
        }
    }
}
