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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateListingViewer.Models;
using RealEstateListingViewer.Services;

namespace RealEstateListingViewer.Tests
{
    [TestClass]
    public class RealEstateServiceTestFixture
    {
        [TestMethod]
        public void CanGetProperty()
        {
            RealEstateService target = new RealEstateService();

            RealEstate realEstate = target.GetRealEstate();

            Assert.AreEqual(0.5d, realEstate.Acreage);
            Assert.AreEqual("132 Main Street", realEstate.Address);
            Assert.AreEqual("Redmond", realEstate.County);
            Assert.AreEqual("WA", realEstate.State);
            Assert.AreEqual(315000d, realEstate.Price);
            Assert.AreEqual(98052, realEstate.ZipCode);
            Assert.AreEqual(0.5d, realEstate.Acreage);
            Assert.AreEqual(2d, realEstate.GarageSize);
            Assert.AreEqual("This property redefines beauty, with ocean views, mountain views, beach views, tree views, and views of views.  In short it is spectacular.", realEstate.Description);
            Assert.AreEqual(2.5d, realEstate.Bathrooms);
            Assert.AreEqual(5d, realEstate.Bedrooms);
            Assert.AreEqual(2d, realEstate.GarageSize);
        }

    }
}
