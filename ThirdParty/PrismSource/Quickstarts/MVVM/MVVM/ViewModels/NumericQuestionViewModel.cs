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
using MVVM.Model;

namespace MVVM.ViewModels
{
    /// <summary>
    /// View model for numeric questions.
    /// </summary>
    public class NumericQuestionViewModel : QuestionViewModel<NumericQuestion>
    {
        private bool hasBindingError;

        public NumericQuestionViewModel(NumericQuestion question)
            : base(question)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the binding for the response has failed.
        /// </summary>
        /// <remarks>
        /// <para>This property is used to let the View indicate that there has been an error with the binding, and its value is
        /// used to determine whether the View Model has a valid answer in addition to what the Model object indicates.</para>
        /// <para>Since the binding errors resulting from value conversion failures don't result in updates on the model, 
        /// neither the model nor the view model can be aware of these errors unless they are explicitly indicated.</para>
        /// <para>When setting this property to a value other than the current value, the 
        /// <see cref="QuestionViewModel.ResponseChanged"/> event is raised.</para>
        /// </remarks>
        public bool HasBindingError
        {
            get
            {
                return this.hasBindingError;
            }

            set
            {
                if (this.hasBindingError == value)
                {
                    return;
                }

                this.hasBindingError = value;

                // Cannot raise the event only when changing from true to false. 
                // If the value in the model does not change (e.g. the input from the user went from "11" to "abc" back to "11")
                // then the model will not raise the PropertyChanged event (since it has not changed from its point of view)
                this.OnResponseChanged();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the question represented by the View Model can be considered answered.
        /// </summary>
        /// <remarks>
        /// This View Model factors in any binding errors when calculating the value for this property.
        /// </remarks>
        public override bool ResponseComplete
        {
            get
            {
                return !this.HasBindingError && base.ResponseComplete;
            }
        }

        protected override void OnQuestionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Response")
            {
                // Silently reset the HasBindingError, as the notification for ResponseChanged will already be raised.
                this.hasBindingError = false;                
                this.OnResponseChanged();
            }
        }
    }
}
