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
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Practices.Prism.ViewModel;
using MVVM.Client.Infrastructure;
using MVVM.Questionnaires.Model;
using MVVM.Repository;

namespace MVVM.Client.ViewModels
{
    /// <summary>
    /// View model for the list of available questionnaire templates.
    /// </summary>
    /// <remarks>
    /// This view model retrieves the list of available questionnaire templates and navigates to the view to take a 
    /// questionnaire when the user indicates.
    /// </remarks>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AvailableQuestionnaireTemplatesListViewModel : NotificationObject
    {
        private readonly ISingleViewUIService uiService;
        private readonly IQuestionnaireRepository questionnaireRepository;

        [ImportingConstructor]
        public AvailableQuestionnaireTemplatesListViewModel(IQuestionnaireRepository questionnaireRepository, ISingleViewUIService uiService)
        {
            this.QuestionnaireTemplates = new ObservableCollection<QuestionnaireTemplate>();

            this.questionnaireRepository = questionnaireRepository;
            this.uiService = uiService;

            this.QuestionnaireTemplates = new ObservableCollection<QuestionnaireTemplate>();

            this.TakeSurveyCommand = new ActionCommand(o => this.TakeSurvey((QuestionnaireTemplate)o));

            this.QuestionnaireTemplateSummary = new QuestionnaireTemplateSummaryViewModel(this.questionnaireRepository);

            this.questionnaireRepository.GetQuestionnaireTemplatesAsync(
                (result) =>
                {
                    RefreshQuestionnaireTemplates(result.Result);
                });
        }

        public ObservableCollection<QuestionnaireTemplate> QuestionnaireTemplates { get; private set; }

        public ICommand TakeSurveyCommand { get; private set; }

        public QuestionnaireTemplateSummaryViewModel QuestionnaireTemplateSummary { get; private set; }

        private void TakeSurvey(QuestionnaireTemplate questionnaireTemplate)
        {
            // Asks the UI service to go to the "complete questionnaire" view, providing the selected template as
            // the context for the view.
            this.uiService.ShowView(ViewNames.CompleteQuestionnaire, questionnaireTemplate);
        }

        private void RefreshQuestionnaireTemplates(IEnumerable<QuestionnaireTemplate> templates)
        {
            this.QuestionnaireTemplates.Clear();
            foreach (var template in templates)
            {
                this.QuestionnaireTemplates.Add(template);
            }
        }
    }
}
