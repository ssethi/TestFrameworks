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
using System.Linq;

namespace MVVM.Model
{
    public partial class MultipleSelectionQuestion : Question
    {
        public MultipleSelectionQuestion(string question, IEnumerable<string> range)
            : base(question)
        {
            this.Range = new List<string>(range);
        }

        public override bool IsComplete
        {
            get
            {
                return base.IsComplete && this.Response != null;
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
                    var selectionsArray = ((ICollection<string>)value).ToArray();
                    if (this.MaxSelections.HasValue && selectionsArray.Length > this.MaxSelections.Value)
                    {
                        errors.Add(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                "There can be at most {0} answers selected",
                                this.MaxSelections.Value));
                    }

                    if (!selectionsArray.All(s => this.Range.Contains(s)))
                    {
                        errors.Add("All elements must belong to the Range");
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
