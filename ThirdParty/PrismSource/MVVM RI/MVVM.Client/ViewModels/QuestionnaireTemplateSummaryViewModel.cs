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
using Microsoft.Practices.Prism.ViewModel;
using MVVM.Questionnaires.Model;
using MVVM.Repository;

namespace MVVM.Client.ViewModels
{
    public class QuestionnaireTemplateSummaryViewModel : NotificationObject
    {
        private readonly IQuestionnaireRepository questionnaireRepository;
        private QuestionnaireTemplate selectedQuestionnaireTemplate;
        private QuestionnaireTemplateSummary questionnaireTemplateSummary;
        private string currentState;

        public QuestionnaireTemplateSummaryViewModel(IQuestionnaireRepository questionnaireRepository)
        {
            this.questionnaireRepository = questionnaireRepository;

            this.currentState = "Normal";
        }

        public QuestionnaireTemplate CurrentlySelectedQuestionnaireTemplate
        {
            get
            {
                return this.selectedQuestionnaireTemplate;
            }

            set
            {
                this.selectedQuestionnaireTemplate = value;
                this.UpdateQuestionnaireSummary();
            }
        }

        public QuestionnaireTemplateSummary QuestionnaireTemplateSummary
        {
            get
            {
                return this.questionnaireTemplateSummary;
            }

            set
            {
                if (this.questionnaireTemplateSummary != value)
                {
                    this.questionnaireTemplateSummary = value;
                    this.RaisePropertyChanged(() => this.QuestionnaireTemplateSummary);
                }
            }
        }

        /// <summary>
        /// Gets the current state.
        /// </summary>
        /// <remarks>
        /// The view uses this property to determine whether an animation should be started.
        /// </remarks>
        public string CurrentState
        {
            get
            {
                return this.currentState;
            }

            set
            {
                if (this.currentState == value)
                {
                    return;
                }

                this.currentState = value;
                this.RaisePropertyChanged(() => this.CurrentState);
            }
        }

        private void UpdateQuestionnaireSummary()
        {
            this.QuestionnaireTemplateSummary = null;

            var requestedQuestionnaireTemplate = this.selectedQuestionnaireTemplate;
            if (requestedQuestionnaireTemplate != null)
            {
                this.CurrentState = "RequestingSummary";
                this.questionnaireRepository.GetQuestionnaireTemplateSummaryAsync(
                    this.selectedQuestionnaireTemplate,
                    or =>
                    {
                        if (this.CurrentlySelectedQuestionnaireTemplate == requestedQuestionnaireTemplate)
                        {
                            var summary = or.Result;
                            this.CurrentState = "Normal";
                            this.QuestionnaireTemplateSummary = summary;
                        }
                    });
            }
            else
            {
                this.CurrentState = "Normal";
            }
        }
    }
}
