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
    public partial class Questionnaire
    {
        public Questionnaire(params Question[] questions)
        {
            foreach (var question in questions)
            {
                this.Questions.Add(question);
            }
        }

        public bool IsComplete
        {
            get
            {
                return !this.HasErrors
                        && !string.IsNullOrEmpty(this.Name)
                        && this.Age.HasValue;
            }
        }

        protected override void ValidateProperty(string propertyName, object value)
        {
            if (propertyName == "Age")
            {
                var response = value as int?;
                var errors = new List<string>();

                if (!response.HasValue)
                {
                    errors.Add("Age must be completed");
                }
                else if (this.MaxAge.HasValue && (this.MaxAge.Value < response || this.MinAge > response))
                {
                    errors.Add(string.Format("Age should be between {0} and {1}.", this.MinAge, this.MaxAge.Value));
                }
                else if (this.MinAge > response)
                {
                    errors.Add(string.Format("Age should be more than {0}.", this.MinAge));
                }

                this.ErrorsContainer.SetErrors(propertyName, errors);
            }
            else if (propertyName == "Name")
            {
                var response = value as string;
                var errors = new List<string>();

                if (string.IsNullOrEmpty(response))
                {
                    errors.Add("Name should be completed.");
                }
                else if (this.NameMaxLength.HasValue && response.Length > this.NameMaxLength.Value)
                {
                    errors.Add(string.Format("Name should have less than {0} characters.", this.NameMaxLength.Value));
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
