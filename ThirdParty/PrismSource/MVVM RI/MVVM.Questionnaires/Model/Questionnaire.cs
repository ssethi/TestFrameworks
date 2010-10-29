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
using System.Linq;

namespace MVVM.Questionnaires.Model
{
    public partial class Questionnaire
    {
        public Questionnaire(QuestionnaireTemplate template)
        {
            this.Template = template;

            foreach (var question in this.Template.Questions.Select(qt => qt.CreateNewQuestion()))
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

        public string QuestionnaireTitle
        {
            get
            {
                return this.Template.Title;
            }
        }
    }
}
