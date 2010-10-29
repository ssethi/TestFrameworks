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
using System.Globalization;
using System.Windows.Media.Imaging;

namespace RealEstateListingViewer.Services
{
    public partial class RealEstateService
    {
        /// <summary>
        /// Return the images.   In silverlight, we want to download the image from a webserver.
        /// You can either store the images on the server, or build some
        //  http handler to server the images. 
        /// </summary>
        /// <returns></returns>
        private static BitmapImage GetImage()
        {
            Uri imageUri;
            Uri source = App.Current.Host.Source;

            if (source.ToString().StartsWith("file://", StringComparison.OrdinalIgnoreCase))
            {
                imageUri = new Uri("../Images/House.jpg", UriKind.Relative);
            }
            else
            {
                source = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/", source.Scheme, source.Host, source.Port));
                imageUri = new Uri(source, "Images/house.jpg");
            }
            return new BitmapImage(imageUri);
        }
    }
}
