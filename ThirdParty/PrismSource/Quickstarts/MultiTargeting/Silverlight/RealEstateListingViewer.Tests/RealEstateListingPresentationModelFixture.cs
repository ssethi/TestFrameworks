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
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateListingViewer.Models;
using RealEstateListingViewer.Services;
using RealEstateListingViewer.Views;

namespace RealEstateListingViewer.Tests
{
    [TestClass]
    public class RealEstateListingPresentationModelFixture
    {
        [TestMethod]
        public void PresentationModelUsesInjectedView()
        {
            var mockView = new MockView();
            var model = new RealEstateListingPresentationModel(mockView, new MockRealEstateService());

            Assert.AreSame(mockView, model.View);
        }

        [TestMethod]
        public void PresentationModelInitializesViewModel()
        {
            var mockView = new MockView();
            var model = new RealEstateListingPresentationModel(mockView, new MockRealEstateService());

            Assert.AreSame(model, mockView.Model);
        }

        [TestMethod]
        public void ShouldRetrievePropertyInfo()
        {
            var mockRealEstateService = new MockRealEstateService();
            var model = new RealEstateListingPresentationModel(new MockView(), mockRealEstateService);

            Assert.IsTrue(mockRealEstateService.GetRealEstateCalled);

            Assert.AreEqual(model.Acreage, mockRealEstateService.getRealEstateReturnValue.Acreage);
            Assert.AreEqual(model.Address, mockRealEstateService.getRealEstateReturnValue.Address);
            Assert.AreEqual(model.Bathrooms, mockRealEstateService.getRealEstateReturnValue.Bathrooms);
            Assert.AreEqual(model.Bedrooms, mockRealEstateService.getRealEstateReturnValue.Bedrooms);
            Assert.AreEqual(model.County, mockRealEstateService.getRealEstateReturnValue.County);
            Assert.AreEqual(model.Description, mockRealEstateService.getRealEstateReturnValue.Description);
            Assert.AreEqual(model.GarageSize, mockRealEstateService.getRealEstateReturnValue.GarageSize);
            Assert.AreEqual(model.Price, mockRealEstateService.getRealEstateReturnValue.Price);
            Assert.AreEqual(model.State, mockRealEstateService.getRealEstateReturnValue.State);
            Assert.AreEqual(model.ZipCode, mockRealEstateService.getRealEstateReturnValue.ZipCode);
        }

    }

    internal class MockRealEstateService : IRealEstateService
    {
        public bool GetRealEstateCalled = false;

        private RealEstateFeatureMatching realEstate = new RealEstateFeatureMatching()
                                                       {
                                                           CriteriaMatchingPercentage = 12
                                                           ,
                                                           CriteriaWeight = 10
                                                           ,
                                                           FeatureDescription = "I am a feature"
                                                       };

        public RealEstate getRealEstateReturnValue;
        public MockRealEstateService()
        {
            getRealEstateReturnValue = new RealEstate();
            this.getRealEstateReturnValue.Image = new BitmapImage(
                new Uri("RealEstateListingViewer.Tests;Component/Resources/MockImage.jpg", UriKind.Relative));

            this.getRealEstateReturnValue.CriteriaMatching.Add(new RealEstateFeatureMatching()
                                                       {
                                                           CriteriaMatchingPercentage = 12,
                                                           CriteriaWeight = 10,
                                                           FeatureDescription = "I am a feature"
                                                       });

            this.getRealEstateReturnValue.CriteriaMatching.Add(new RealEstateFeatureMatching()
            {
                CriteriaMatchingPercentage = 5
                ,
                CriteriaWeight = 2
                ,
                FeatureDescription = "I am a feature too"
            });
        }

        public RealEstate GetRealEstate()
        {
            GetRealEstateCalled = true;
            return getRealEstateReturnValue;
        }
    }

    internal class MockView : IRealEstateListingView
    {
        public RealEstateListingPresentationModel Model;

        public void SetModel(RealEstateListingPresentationModel model)
        {
            this.Model = model;
        }
    }
}
