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
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using MVVM.Client.Infrastructure;
using MVVM.Client.Infrastructure.StateManagement;
using MVVM.Questionnaires.Model;
using MVVM.Questionnaires.Services;
using MVVM.Repository;

namespace MVVM.Client.ViewModels
{
    /// <summary>
    /// View model for a questionnaire.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This view model is the parent for the view models representing each question in the questionnaire.
    /// </para>
    /// </remarks>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class QuestionnaireViewModel : NotificationObject
    {
        private readonly IQuestionnaireRepository questionnaireRepository;
        private readonly ISingleViewUIService uiService;
        private readonly InteractionRequest<Notification> submitErrorInteractionRequest;
        private readonly InteractionRequest<Confirmation> cancelConfirmationInteractionRequest;
        private string currentState;
        private bool hasAgeBindingError;
        private bool hasLocalChanges;

        [ImportingConstructor]
        public QuestionnaireViewModel(
           IState<QuestionnaireTemplate> questionnaireTemplate,
           IQuestionnaireRepository questionnaireRepository,
           ISingleViewUIService uiService)
        {
            this.questionnaireRepository = questionnaireRepository;
            this.uiService = uiService;

            this.submitErrorInteractionRequest = new InteractionRequest<Notification>();
            this.cancelConfirmationInteractionRequest = new InteractionRequest<Confirmation>();

            this.Questions = new ObservableCollection<QuestionViewModel>();
            this.currentState = "Normal";

            this.Questionnaire = new Questionnaire(questionnaireTemplate.Value);
            this.Questionnaire.PropertyChanged += this.OnQuestionnairePropertyChanged;

            foreach (var question in this.Questionnaire.Questions)
            {
                this.Questions.Add(this.CreateQuestionViewModel(question));
            }
        }

        public ObservableCollection<QuestionViewModel> Questions { get; private set; }

        public Questionnaire Questionnaire { get; private set; }

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

        public bool CanSubmit
        {
            get
            {
                return !this.HasAgeBindingError
                        && this.Questionnaire.IsComplete
                        && this.Questions.All(q => q.ResponseComplete);
            }
        }

        public int TotalQuestions
        {
            get { return this.Questions.Count(); }
        }

        public int UnansweredQuestions
        {
            get { return this.Questions.Where(q => !q.ResponseComplete).Count(); }
        }

        /// <summary>
        /// Gets or sets an indication that the binding for the age has failed.
        /// </summary>
        /// <remarks>
        /// <para>This property is used to let the View indicate that there has been an error with the binding, and its value is
        /// used to determine whether the View Model has a valid age in addition to what the Model object indicates.</para>
        /// <para>Since the binding errors resulting from value conversion failures result in no updates on the model, 
        /// neither the model nor the view model can be aware of these errors unless they are explicitly indicated.</para>
        /// <para>When setting this property to a value other than the current value, the 
        /// <see cref="PropertyChanged"/> event is raised for the <see cref="CanSubmit"/> property.</para>
        /// </remarks>
        public bool HasAgeBindingError
        {
            get
            {
                return this.hasAgeBindingError;
            }

            set
            {
                if (this.hasAgeBindingError == value)
                {
                    return;
                }

                this.hasAgeBindingError = value;

                this.RaisePropertyChanged(() => this.CanSubmit);
            }
        }

        public bool HasChanges
        {
            get
            {
                return this.hasLocalChanges || this.Questions.Any(q => q.HasChanges);
            }
        }

        /// <summary>
        /// Gets the interaction request for notifying errors while submitting a questionnaire.
        /// </summary>
        public IInteractionRequest SubmitErrorInteractionRequest
        {
            get
            {
                return this.submitErrorInteractionRequest;
            }
        }

        /// <summary>
        /// Gets the interaction request for confirming a cancelled questionnaire.
        /// </summary>
        public IInteractionRequest CancelConfirmationInteractionRequest
        {
            get
            {
                return this.cancelConfirmationInteractionRequest;
            }
        }

        /// <summary>
        /// This is here to intentially force the service to fail on submission allowing
        /// demonstration of error handling in the ViewModel.  This is not something
        /// you would normally do in your ViewModel code.
        /// 
        /// This sets a static on the service (also something only there to demonstrate failures on submission).
        /// </summary>
        public bool ForceSubmitFailure
        {
            get
            {
                return QuestionnaireService.FailOnSubmit;
            }

            set
            {
                QuestionnaireService.FailOnSubmit = value;
            }
        }

        /// <summary>
        /// Submits the current questionnaire through the model and navigates to the list of available questionnaires.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method updates the current state to "Submitting", which will cause the view to 
        /// provide some kind of feedback, and submits the questionnaire.
        /// </para>
        /// <para>
        /// If an error occurs while submitting then the view model requests the ui service to show a suitable dialog 
        /// with an error message and remains in the current view.
        /// </para>
        /// </remarks>
        public void Submit()
        {
            this.CurrentState = "Submitting";

            this.questionnaireRepository.SubmitQuestionnaireAsync(
                this.Questionnaire,
                (result) =>
                {
                    if (result.Error == null)
                    {
                        this.NavigateToQuestionnaireList();
                    }
                    else
                    {
                        this.CurrentState = "Normal";

                        // Raise the interaction request to notify the user
                        this.submitErrorInteractionRequest.Raise(
                            new Notification { Content = result.Error, Title = "Error submitting the questionnaire" },
                            se => { });
                    }
                });
        }

        /// <summary>
        /// Cancels the current questionnaire and navigates to the list of available questionnaires.
        /// </summary>
        /// <remarks>
        /// If the questionnaire is completed to some extent, the view model requests a confirmation before navigating
        /// out.
        /// </remarks>
        public void Cancel()
        {
            if (this.HasChanges)
            {
                // Raise the interaction request to confirm
                this.cancelConfirmationInteractionRequest.Raise(
                    new Confirmation { Content = "Are you sure you want to abandon this questionnaire?", Title = "Confirm" },
                    confirmation =>
                    {
                        if (confirmation.Confirmed)
                        {
                            this.NavigateToQuestionnaireList();
                        }
                    });
            }
            else
            {
                this.NavigateToQuestionnaireList();
            }
        }

        private void NavigateToQuestionnaireList()
        {
            // Asks the UI service to go to the "questionnaire list" view. No context is necessary.
            this.uiService.ShowView(ViewNames.QuestionnaireTemplatesList);
        }

        private void OnQuestionnairePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Name")
            {
                this.hasLocalChanges = true;
                this.RaisePropertyChanged(() => this.CanSubmit);
            }

            if (args.PropertyName == "Age")
            {
                // silently resets the binding errors flag.
                this.hasAgeBindingError = false;
                this.hasLocalChanges = true;
                this.RaisePropertyChanged(() => this.CanSubmit);
            }
        }

        private QuestionViewModel CreateQuestionViewModel(Question question)
        {
            QuestionViewModel vm = QuestionViewModelFactory.GetViewModelForQuestion(question);
            vm.ResponseChanged += this.OnQuestionResponseChanged;

            return vm;
        }

        private void OnQuestionResponseChanged(object sender, EventArgs args)
        {
            this.RaisePropertyChanged(() => this.UnansweredQuestions);
            this.RaisePropertyChanged(() => this.CanSubmit);
        }

        /// <summary>
        /// Factory class to create a question view model for a given question object.
        /// </summary>
        private static class QuestionViewModelFactory
        {
            private static Dictionary<Type, Func<Question, QuestionViewModel>> maps = new Dictionary<Type, Func<Question, QuestionViewModel>>()
            {
                { typeof(OpenQuestion), (q) => new OpenQuestionViewModel((OpenQuestion)q) },
                { typeof(MultipleSelectionQuestion), (q) => new MultipleSelectionQuestionViewModel((MultipleSelectionQuestion)q) },
                { typeof(NumericQuestion), (q) => new NumericQuestionViewModel((NumericQuestion)q) }
            };

            public static QuestionViewModel GetViewModelForQuestion(Question question)
            {
                Func<Question, QuestionViewModel> viewModelInstanceFactory = null;
                if (maps.TryGetValue(question.GetType(), out viewModelInstanceFactory))
                {
                    return viewModelInstanceFactory(question);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Could not locate a view model for question type");
                }
            }
        }
    }
}
