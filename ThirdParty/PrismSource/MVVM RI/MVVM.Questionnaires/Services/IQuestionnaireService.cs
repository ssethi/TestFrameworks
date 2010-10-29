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

namespace MVVM.Questionnaires.Services
{
    public interface IQuestionnaireService
    {
        IAsyncResult BeginGetQuestionnaireTemplates(AsyncCallback callback, object userState);

        IEnumerable<QuestionnaireTemplate> EndGetQuestionnaireTemplates(IAsyncResult asyncResult);

        IAsyncResult BeginGetQuestionnaireTemplateSummary(QuestionnaireTemplate questionnaireTemplate, AsyncCallback callback, object userState);

        QuestionnaireTemplateSummary EndGetQuestionnaireTemplateSummary(IAsyncResult asyncResult);

        IAsyncResult BeginSubmitQuestionnaire(Questionnaire questionnaire, AsyncCallback callback, object userState);

        void EndSubmitQuestionnaire(IAsyncResult asyncResult);
    }
}
