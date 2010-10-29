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

namespace MVVM.Client.Infrastructure
{
    /// <summary>
    /// Extension methods on the string class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Attempts to convert a string into a 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        public static bool TryConvertValue(this string value, out int convertedValue)
        {
            convertedValue = 0;
            try
            {
                convertedValue = Convert.ToInt32(value, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
