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
using System.Collections.Generic;

namespace MVVM.Model
{
    public partial class OpenQuestion
    {
        public OpenQuestion(string questionText)
            : base(questionText)
        {
        }

        public override bool IsComplete
        {
            get
            {
                return base.IsComplete && !string.IsNullOrEmpty(this.Response);
            }
        }

        protected override void ValidateProperty(string propertyName, object value)
        {
            if (propertyName == "Response")
            {
                var errors = new List<string>();

                var response = value as string;

                if (string.IsNullOrEmpty(response))
                {
                    errors.Add("The value cannot be null or empty");
                }
                else if (response.Length > this.MaxLength)
                {
                    errors.Add("Response is too long");
                }

                this.ErrorsContainer.SetErrors(propertyName, errors);
            }
            else
            {
                base.ValidateProperty(propertyName, value);
            }
        }
    }
}
