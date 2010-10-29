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
using System.Globalization;

namespace MVVM.Model
{
    public partial class NumericQuestion
    {
        public NumericQuestion(string question)
            : base(question)
        {
        }

        public override bool IsComplete
        {
            get
            {
                return base.IsComplete && this.Response.HasValue;
            }
        }

        protected override void ValidateProperty(string propertyName, object value)
        {
            if (propertyName == "Response")
            {
                var errors = new List<string>();

                if (value == null)
                {
                    errors.Add("The value cannot be null");
                }
                else
                {
                    var response = value as int?;
                    if (this.MaxValue.HasValue)
                    {
                        if (response.Value < 0 || response.Value > this.MaxValue.Value)
                        {
                            errors.Add(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    "Response must be a number between 0 and {0}",
                                    this.MaxValue.Value));
                        }
                    }
                    else
                    {
                        if (response.Value < 0)
                        {
                            errors.Add("Response must be a number greater or equal to 0");
                        }
                    }
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
