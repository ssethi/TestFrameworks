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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace MVVM.Questionnaires.Model
{
    public static class CommonValidation
    {
        public static ValidationResult ValidationMultipleSelectionResponseChoices(IEnumerable<string> value, ValidationContext context)
        {
            var question = (MultipleSelectionQuestion)context.ObjectInstance;
            if (value != null)
            {
                var maxSelections = question.MaxSelections;

                if (maxSelections.HasValue)
                {
                    if (value.Count() > maxSelections.Value)
                    {
                        return new ValidationResult(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                "Response must have at most {0} selections",
                                maxSelections.Value),
                            new[] { context.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidationNumericQuestionResponseRange(int? value, ValidationContext context)
        {
            var question = (NumericQuestion)context.ObjectInstance;
            if (value.HasValue)
            {
                if (question.MaxValue.HasValue)
                {
                    if (value.Value < 0 || value.Value > question.MaxValue.Value)
                    {
                        return new ValidationResult(
                            string.Format(CultureInfo.CurrentCulture, "Response must be a number between 0 and {0}", question.MaxValue.Value),
                            new[] { context.MemberName });
                    }
                }
                else
                {
                    if (value.Value < 0)
                    {
                        return new ValidationResult(
                            "Response must be a number greater or equal to 0",
                            new[] { context.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidationOpenQuestionResponseLength(string value, ValidationContext context)
        {
            var question = (OpenQuestion)context.ObjectInstance;
            if (value != null && (value.Length > question.MaxLength))
            {
                return new ValidationResult("Response is too long", new[] { context.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
