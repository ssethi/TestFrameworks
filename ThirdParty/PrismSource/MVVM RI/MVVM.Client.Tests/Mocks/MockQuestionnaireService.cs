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
using MVVM.Questionnaires.Model;
using MVVM.Questionnaires.Services;

namespace MVVM.Client.Tests.Mocks
{
    public class MockQuestionnaireService : IQuestionnaireService
    {
        public Action<AsyncResult<object>> HandleBeginSubmitResponses { get; set; }
        public Action<AsyncResult<IEnumerable<QuestionnaireTemplate>>> HandleBeginGetQuestionnaireTemplates { get; set; }
        public Action<AsyncResult<QuestionnaireTemplateSummary>> HandleBeginGetQuestionnaireTemplateSummary { get; set; }

        #region IQuestionnaireService

        public IAsyncResult BeginSubmitQuestionnaire(Questionnaire questionnaire, AsyncCallback callback, object asyncState)
        {
            var result = new AsyncResult<object>(callback, asyncState);
            HandleBeginSubmitResponses(result);

            return result;
        }

        public virtual void EndSubmitQuestionnaire(IAsyncResult asyncResult)
        {
            AsyncResult<object>.End((AsyncResult<object>)asyncResult);
        }

        public IAsyncResult BeginGetQuestionnaireTemplates(AsyncCallback callback, object asyncState)
        {
            var result = new AsyncResult<IEnumerable<QuestionnaireTemplate>>(callback, asyncState);
            HandleBeginGetQuestionnaireTemplates(result);

            return result;
        }

        public IEnumerable<QuestionnaireTemplate> EndGetQuestionnaireTemplates(IAsyncResult asyncResult)
        {
            var result = AsyncResult<IEnumerable<QuestionnaireTemplate>>.End(asyncResult);

            return result.Result;
        }

        public IAsyncResult BeginGetQuestionnaireTemplateSummary(QuestionnaireTemplate questionnaireTemplate, AsyncCallback callback, object asyncState)
        {
            var result = new AsyncResult<QuestionnaireTemplateSummary>(callback, asyncState);
            HandleBeginGetQuestionnaireTemplateSummary(result);

            return result;
        }

        public QuestionnaireTemplateSummary EndGetQuestionnaireTemplateSummary(IAsyncResult asyncResult)
        {
            var result = AsyncResult<QuestionnaireTemplateSummary>.End(asyncResult);

            return result.Result;
        }

        #endregion

        public void ProceedSubmitResponses(AsyncResult<object> result)
        {
            result.SetComplete(null, false);
        }

        public void ProceedGetQuestionnaireTemplates(AsyncResult<IEnumerable<QuestionnaireTemplate>> result, IEnumerable<QuestionnaireTemplate> questionnaireTemplates)
        {
            result.SetComplete(questionnaireTemplates, false);
        }

        public void ProceedGetQuestionnaireTemplateSummary(AsyncResult<QuestionnaireTemplateSummary> result, QuestionnaireTemplateSummary questionnaireTemplateSummary)
        {
            result.SetComplete(questionnaireTemplateSummary, false);
        }
    }
}
