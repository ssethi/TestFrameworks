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
using System.Threading;
using MVVM.Questionnaires.Model;

namespace MVVM.Questionnaires.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        // This is to support demonstration of a failed submit.
        public static bool FailOnSubmit { get; set; }

        public IAsyncResult BeginGetQuestionnaireTemplates(AsyncCallback callback, object userState)
        {
            var asyncResult = new AsyncResult<IEnumerable<QuestionnaireTemplate>>(callback, userState);
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    asyncResult.SetComplete(CreateQuestionnaireTemplates(), false);
                });

            return asyncResult;
        }

        public IEnumerable<QuestionnaireTemplate> EndGetQuestionnaireTemplates(IAsyncResult asyncResult)
        {
            var localAsyncResult = AsyncResult<IEnumerable<QuestionnaireTemplate>>.End(asyncResult);

            return localAsyncResult.Result;
        }

        public IAsyncResult BeginGetQuestionnaireTemplateSummary(QuestionnaireTemplate questionnaireTemplate, AsyncCallback callback, object userState)
        {
            var asyncResult = new AsyncResult<QuestionnaireTemplateSummary>(callback, userState);
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    Thread.Sleep(1500);
                    asyncResult.SetComplete(CreateQuestionnaireTemplateSummary(questionnaireTemplate), false);
                });

            return asyncResult;
        }

        public QuestionnaireTemplateSummary EndGetQuestionnaireTemplateSummary(IAsyncResult asyncResult)
        {
            var localAsyncResult = AsyncResult<QuestionnaireTemplateSummary>.End(asyncResult);

            return localAsyncResult.Result;
        }

        public IAsyncResult BeginSubmitQuestionnaire(Questionnaire questionnaire, AsyncCallback callback, object userState)
        {
            var asyncResult = new AsyncResult<object>(callback, userState);
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    // simulated processing time
                    Thread.Sleep(2000);

                    if (!FailOnSubmit)
                    {
                        asyncResult.SetComplete((object)null, false);
                    }
                    else
                    {
                        asyncResult.SetComplete(new InvalidOperationException("Not authorized"), false);
                    }
                });

            return asyncResult;
        }

        public void EndSubmitQuestionnaire(IAsyncResult asyncResult)
        {
            AsyncResult<object>.End(asyncResult);
        }

        private static IEnumerable<QuestionnaireTemplate> CreateQuestionnaireTemplates()
        {
            return new[]
                {
                    new QuestionnaireTemplate
                    {
                        Title = "Food Questionnaire",
                        Questions =
                            {
                                new NumericQuestionTemplate 
                                { 
                                    QuestionText = "How many times do you eat out a month?", 
                                    MaxValue = 15 
                                },
                                new MultipleSelectionQuestionTemplate 
                                { 
                                    QuestionText = "Which are your favorite entries? (Max. 2)", 
                                    Range = new[] { "Pizza", "Pasta", "Steak", "Ribs" }, 
                                    MaxSelections = 2 
                                },
                                new OpenQuestionTemplate 
                                { 
                                    QuestionText = "What appetizers would you add to the menu?", 
                                    MaxLength = 250 
                                },
                                new OpenQuestionTemplate 
                                { 
                                    QuestionText = "Do you have any feedback for the management?", 
                                    MaxLength = 250 
                                },
                            },
                    },

                    new QuestionnaireTemplate
                    {
                        Title = "Drawbridge Survey",
                        Questions =
                            {
                                new NumericQuestionTemplate()
                                { 
                                    QuestionText = "What is the average airspeed of a swallow?"                                    
                                },
                                new MultipleSelectionQuestionTemplate 
                                { 
                                    QuestionText = "What is your favorite color? (Pick 1)", 
                                    Range = new[] { "Red", "Green", "Blue", "Puce" }, 
                                    MaxSelections = 1 
                                },
                            },
                    }
                };
        }

        private static QuestionnaireTemplateSummary CreateQuestionnaireTemplateSummary(QuestionnaireTemplate questionnaireTemplate)
        {
            var summary =
                new QuestionnaireTemplateSummary
                {
                    Title = questionnaireTemplate.Title,
                    Description = "Description",
                    EstimatedTime = questionnaireTemplate.Questions.Count * 2.5f,
                    NumberOfQuestions = questionnaireTemplate.Questions.Count,
                    TimesTaken = 0
                };

            return summary;
        }
    }
}
