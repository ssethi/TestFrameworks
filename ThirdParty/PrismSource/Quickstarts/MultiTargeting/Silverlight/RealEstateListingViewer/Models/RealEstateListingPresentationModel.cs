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
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using PieChartLibrary;
using RealEstateListingViewer.Services;
using RealEstateListingViewer.Views;

namespace RealEstateListingViewer.Models
{
    public class RealEstateListingPresentationModel
    {
        public RealEstateListingPresentationModel()
            : this(new RealEstateListingView(), new RealEstateService())
        {
        }

        public RealEstateListingPresentationModel(IRealEstateListingView view, IRealEstateService realEstateService)
        {
            RealEstate realEstate = realEstateService.GetRealEstate();

            Address = realEstate.Address;
            County = realEstate.County;
            State = realEstate.State;
            Price = realEstate.Price;
            ZipCode = realEstate.ZipCode;
            Bedrooms = realEstate.Bedrooms;
            Bathrooms = realEstate.Bathrooms;
            GarageSize = realEstate.GarageSize;
            Acreage = realEstate.Acreage;
            Description = realEstate.Description;
            PropertyImage = realEstate.Image;

            this.Sections = new ObservableCollection<PieSection>();
            foreach(var matching in realEstate.CriteriaMatching)
            {
                this.Sections.Add(new PieSection() { SectionWeight = matching.CriteriaWeight, MatchingPercentage = matching.CriteriaMatchingPercentage, Description = matching.FeatureDescription });
            }
            
            this.View = view;
            this.View.SetModel(this);
        }

        public IRealEstateListingView View
        {
            get; private set;
        }

        public string Address { get; private set; }

        public string County { get; private set; }

        public string State { get; private set; }

        public int ZipCode { get; private set; }

        public double Price { get; private set; }

        public double Acreage { get; private set; }

        public double Bedrooms { get; private set; }

        public double Bathrooms { get; private set; }

        public double GarageSize { get; private set; }

        public string Description { get; private set; }

        //public string Status { get; private set; }

        public BitmapImage PropertyImage { get; private set; }

        public ObservableCollection<PieSection> Sections { get; private set; }
    }
}