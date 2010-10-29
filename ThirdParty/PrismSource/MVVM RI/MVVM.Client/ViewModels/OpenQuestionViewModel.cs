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
using System.ComponentModel;
using MVVM.Questionnaires.Model;

namespace MVVM.Client.ViewModels
{
    /// <summary>
    /// View model for open questions.
    /// </summary>
    public class OpenQuestionViewModel : QuestionViewModel<OpenQuestion>
    {
        public OpenQuestionViewModel(OpenQuestion question)
            : base(question)
        {
        }

        /// <summary>
        /// Gets the available length.
        /// </summary>
        /// <remarks>
        /// This is a calculated property, and notifications of its change are raised when the response property on
        /// the model is changed.
        /// </remarks>
        public int AvailableLength
        {
            get
            {
                return this.Question.MaxLength - (this.Question.Response ?? string.Empty).Length;
            }
        }

        protected override void OnQuestionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Response")
            {
                this.SetHasChanges();
                this.RaisePropertyChanged(() => this.AvailableLength);
                this.OnResponseChanged();
            }
        }
    }
}
