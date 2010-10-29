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
using Microsoft.Practices.Prism.ViewModel;
using MVVM.Model;

namespace MVVM.ViewModels
{
    /// <summary>
    /// View model for a questionnaire.
    /// </summary>
    /// <remarks>
    /// This view model is the parent for the view models representing each question in the questionnaire.
    /// <para/>
    /// This model wraps properties from the underlying model (Name and Age) and properties that represent state for the view
    /// (UnansweredQuestions, CurrentState, and CanSubmit)
    /// </remarks>
    public class QuestionnaireViewModel : NotificationObject
    {
        private readonly IQuestionnaireRepository questionnaireRepository;
        private Questionnaire questionnaire;
        private string currentState;
        private bool hasAgeBindingError;

        public QuestionnaireViewModel()
            : this(new QuestionnaireRepository())
        {
        }

        public QuestionnaireViewModel(IQuestionnaireRepository questionnaireRepository)
        {
            this.questionnaireRepository = questionnaireRepository;
            this.Questions = new ObservableCollection<QuestionViewModel>();
            this.GetNewQuestionnaireInstance();
        }

        public ObservableCollection<QuestionViewModel> Questions { get; private set; }

        public Questionnaire Questionnaire
        {
            get
            {
                return this.questionnaire;
            }

            private set
            {
                this.questionnaire = value;
                this.RaisePropertyChanged(() => this.Questionnaire);
            }
        }

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
                return this.Questionnaire != null
                        && !this.HasAgeBindingError
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

        /// <summary>
        /// Submits the current questionnaire through the model.
        /// </summary>
        /// <remarks>
        /// This method updates the current state to "Submitting", which causes the view to 
        /// provide some kind of feedback, and submits the questionnaire.
        /// </remarks>
        public void Submit()
        {
            this.CurrentState = "Submitting";

            this.questionnaireRepository.SubmitQuestionnaireAsync(
                this.Questionnaire,
                result =>
                {
                    GetNewQuestionnaireInstance();
                });
        }

        public void Reset()
        {
            this.GetNewQuestionnaireInstance();
        }

        private void UpdateCurrentState(string value)
        {
            this.CurrentState = value;            
        }

        private void GetNewQuestionnaireInstance()
        {
            this.UpdateCurrentState("Loading");

            if (this.Questionnaire != null)
            {
                this.Questionnaire.PropertyChanged -= this.OnQuestionnairePropertyChanged;
            }

            this.questionnaireRepository.GetQuestionnaireAsync(
                (result) =>
                {
                    this.Questionnaire = result.Result;
                    this.Questionnaire.PropertyChanged += OnQuestionnairePropertyChanged;
                    this.ResetQuestionnaire();
                    this.UpdateCurrentState("Normal");
                });
        }

        private void ResetQuestionnaire()
        {
            this.Questions.Clear();
            foreach (var q in this.Questionnaire.Questions)
            {
                this.Questions.Add(this.CreateQuestionViewModel(q));
            }

            this.RaisePropertyChanged(() => this.CanSubmit);
        }

        private void OnQuestionnairePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Name")
            {
                this.RaisePropertyChanged(() => this.CanSubmit);
            }

            if (args.PropertyName == "Age")
            {
                this.hasAgeBindingError = false;
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

        private class QuestionViewModelFactory
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
