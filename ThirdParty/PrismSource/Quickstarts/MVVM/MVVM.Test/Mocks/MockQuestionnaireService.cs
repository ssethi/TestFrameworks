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
using MVVM.Model;

namespace MVVM.Test.Mocks
{
    public class MockQuestionnaireService : IQuestionnaireService
    {
        public Action<AsyncResult<object>> HandleBeginSubmitResponses { get; set; }
        public Action<AsyncResult<Questionnaire>> HandleBeginGetQuestionnaire { get; set; }

        #region IQuestionnaireService

        public IAsyncResult BeginSubmitResponses(Questionnaire questionnaire, AsyncCallback callback, object asyncState)
        {
            var result = new AsyncResult<object>(callback, asyncState);
            HandleBeginSubmitResponses(result);

            return result;
        }

        public virtual void EndSubmitResponses(IAsyncResult asyncResult)
        {
            AsyncResult<object>.End((AsyncResult<object>)asyncResult);
        }

        public IAsyncResult BeginGetQuestionnaire(AsyncCallback callback, object asyncState)
        {
            var result = new AsyncResult<Questionnaire>(callback, asyncState);
            HandleBeginGetQuestionnaire(result);

            return result;
        }

        public Questionnaire EndGetQuestionnaire(IAsyncResult asyncResult)
        {
            var result = (AsyncResult<Questionnaire>)asyncResult;
            AsyncResult<Questionnaire>.End(result);

            return result.Result;

        }
        #endregion

        public void ProceedSubmitResponses(AsyncResult<object> result)
        {
            result.SetComplete(null, false);
        }

        public void ProceedGetQuestionnaire(AsyncResult<Questionnaire> result, Questionnaire questionnaire)
        {
            result.SetComplete(questionnaire, false);
        }
    }
}
