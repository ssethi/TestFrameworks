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
using System.Threading;

namespace MVVM.Model
{
    public class QuestionnaireService : IQuestionnaireService
    {
        public IAsyncResult BeginGetQuestionnaire(AsyncCallback callback, object asyncState)
        {
            AsyncResult<Questionnaire> asyncResult = new AsyncResult<Questionnaire>(callback, asyncState);

            ThreadPool.QueueUserWorkItem(
                state =>
                {
                    var questionnaire = DoGetQuestionnaire();
                    asyncResult.SetComplete(questionnaire, false);
                });

            return asyncResult;
        }

        public Questionnaire EndGetQuestionnaire(IAsyncResult asyncResult)
        {
            var result = (AsyncResult<Questionnaire>)asyncResult;
            AsyncResult<Questionnaire>.End(asyncResult);

            return result.Result;
        }

        public IAsyncResult BeginSubmitResponses(Questionnaire questionnaire, AsyncCallback callback, object asyncState)
        {
            var result = new AsyncResult<object>(callback, asyncState);

            ThreadPool.QueueUserWorkItem(
                state =>
                {
                    DoSubmitResponses(questionnaire);
                    result.SetComplete(null, false);
                });

            return result;
        }

        public void EndSubmitResponses(IAsyncResult asyncResult)
        {
            AsyncResult<object>.End((AsyncResult<object>)asyncResult);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.LayoutRules", "SA1500:CurlyBracketsForMultiLineStatementsMustNotShareLine", Justification = "Readability")]
        private Questionnaire DoGetQuestionnaire()
        {
            // simulate loading time
            Thread.Sleep(1000);

            var questionnaire =
               new Questionnaire(
                    new NumericQuestion("How many times do you eat out per month?") { MaxValue = 15 },
                    new MultipleSelectionQuestion(
                            "Which are your favorite entries? (Max. 2)",
                            new[] { "Pizza", "Pasta", "Steak", "Ribs" })
                                {
                                    MaxSelections = 2
                                },
                    new OpenQuestion("What appetizers would you add to the menu?") { MaxLength = 250 },
                    new OpenQuestion("Do you have any feedback for the management?") { MaxLength = 250 })
               {
                   NameMaxLength = 50,
                   MaxAge = 120,
                   MinAge = 16
               };

            return questionnaire;
        }

        private void DoSubmitResponses(Questionnaire questionnaire)
        {
            // simulate submitting time
            Thread.Sleep(1000);
        }
    }
}
