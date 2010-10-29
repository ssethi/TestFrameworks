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
using System.IO;
using System.Windows.Media.Imaging;
using RealEstateListingViewer.Models;

namespace RealEstateListingViewer.Services
{
    public partial class RealEstateService : IRealEstateService
    {

        public RealEstate GetRealEstate()
        {
            var property = new RealEstate
                               {
                                   Address = "132 Main Street",
                                   County = "Redmond",
                                   State = "WA",
                                   Price = 315000d,
                                   ZipCode = 98052,
                                   Bedrooms = 5d,
                                   Bathrooms = 2.5d,
                                   GarageSize = 2d,
                                   Acreage = 0.5d,
                                   Description =
                                       "This property redefines beauty, with ocean views, mountain views, beach views, tree views, and views of views.  In short it is spectacular.",
                                   Image = GetImage()
                               };

            property.CriteriaMatching.Add(new RealEstateFeatureMatching
            {
                FeatureDescription = "Bedrooms",
                CriteriaMatchingPercentage = 100d,
                CriteriaWeight = 1d
            });
            property.CriteriaMatching.Add(new RealEstateFeatureMatching
            {
                FeatureDescription = "Garage",
                CriteriaMatchingPercentage = 80d,
                CriteriaWeight = 0.5d
            });
            property.CriteriaMatching.Add(new RealEstateFeatureMatching
            {
                FeatureDescription = "Price",
                CriteriaMatchingPercentage = 20d,
                CriteriaWeight = 1.5d
            });
            property.CriteriaMatching.Add(new RealEstateFeatureMatching
            {
                FeatureDescription = "Acreage",
                CriteriaMatchingPercentage = 50d,
                CriteriaWeight = 0.9d
            });
            property.CriteriaMatching.Add(new RealEstateFeatureMatching
            {
                FeatureDescription = "BathRooms",
                CriteriaMatchingPercentage = 90d,
                CriteriaWeight = 0.2d
            });

            return property;
        }

    }
}