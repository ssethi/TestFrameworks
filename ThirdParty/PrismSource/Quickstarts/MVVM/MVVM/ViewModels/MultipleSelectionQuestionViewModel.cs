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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MVVM.Model;

namespace MVVM.ViewModels
{
    /// <summary>
    /// View model for multiple selection questions.
    /// </summary>
    /// <remarks>
    /// The model for a multiple selection question expects a list of selections as its answer, but the desired 
    /// View for this question consists of a series of check boxes, one for each option. In order to support this
    /// UI, this view model is a hierarchical view model, exposing a collection of child view models representing 
    /// each option. However, rather than mapping the checked state on each child view model to presence of an option
    /// into the list of selections, the view model exposes the list of selections and expects the view to 
    /// populate it appropriately, which the view does by using the 
    /// <see cref="MVVM.Infrastructure.Behaviors.SynchronizeSelectedItems">SynchronizeSelectedItems</see> class.
    /// </remarks>
    public class MultipleSelectionQuestionViewModel : QuestionViewModel<MultipleSelectionQuestion>
    {
        private readonly ObservableCollection<string> selections;

        public MultipleSelectionQuestionViewModel(MultipleSelectionQuestion question)
            : base(PreAnswerQuestion(question))
        {
            this.selections = new ObservableCollection<string>();
            this.selections.CollectionChanged += this.OnSelectionsChanged;
        }

        /// <summary>
        /// Gets the list of selections.
        /// </summary>
        public IList<string> Selections
        {
            get { return this.selections; }
        }

        protected override void OnQuestionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Response")
            {
                this.OnResponseChanged();
            }
        }

        private static MultipleSelectionQuestion PreAnswerQuestion(MultipleSelectionQuestion question)
        {
            if (question != null && question.Response == null)
            {
                question.Response = new string[0];
            }

            return question;
        }

        /// <summary>
        /// Overwrites the response value in the model, triggering validation and change updates.
        /// </summary>
        private void OnSelectionsChanged(object sender, EventArgs args)
        {
            this.Question.Response = this.selections.ToArray();
        }
    }
}