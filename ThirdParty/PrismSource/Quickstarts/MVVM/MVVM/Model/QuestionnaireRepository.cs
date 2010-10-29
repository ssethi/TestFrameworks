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
    /// <summary>
    /// The QuestionnaireRepository provides a slightly nicer interface for querying the underlying <see cref="QuestionnaireService"/> 
    /// and post results back to the calling thread's <see cref="SynchronizationContext"/>.
    /// </summary>
    /// <seealso cref="IOperationResult"/>
    /// <seealso cref="IOperationResult'T"/>
    public class QuestionnaireRepository : IQuestionnaireRepository
    {
        private IQuestionnaireService service;
        private SynchronizationContext synchronizationContext = System.Threading.SynchronizationContext.Current ?? new SynchronizationContext();

        public QuestionnaireRepository()
            : this(new QuestionnaireService())
        {
        }

        public QuestionnaireRepository(IQuestionnaireService service)
        {
            this.service = service;
        }

        public void GetQuestionnaireAsync(Action<IOperationResult<Questionnaire>> callback)
        {
            IAsyncResult asyncResult = this.service.BeginGetQuestionnaire(
                (ar) =>
                {
                    OperationResult<Questionnaire> operationResult = new OperationResult<Questionnaire>();
                    try
                    {
                        operationResult.Result = service.EndGetQuestionnaire(ar);
                    }
                    catch (Exception ex)
                    {
                        operationResult.Error = ex;
                    }

                    synchronizationContext.Post(
                        (state) =>
                        {
                            callback(operationResult);
                        },
                        null);
                },
                null);
        }

        public void SubmitQuestionnaireAsync(Questionnaire questionnaire, Action<IOperationResult> callback)
        {
            this.service.BeginSubmitResponses(
                questionnaire,
                (ar) =>
                {
                    var operationResult = new OperationResult();

                    try
                    {
                        this.service.EndSubmitResponses(ar);
                    }
                    catch (Exception ex)
                    {
                        operationResult.Error = ex;
                    }

                    synchronizationContext.Post(
                        (state) =>
                        {
                            callback(operationResult);
                        },
                        null);
                },
                null);
        }
    }
}
