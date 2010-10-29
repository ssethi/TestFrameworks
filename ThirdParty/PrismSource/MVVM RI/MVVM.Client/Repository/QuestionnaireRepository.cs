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
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using MVVM.Questionnaires.Model;
using MVVM.Questionnaires.Services;

namespace MVVM.Repository
{
    /// <summary>
    /// The QuestionnaireRepository provides a slightly nicer interface for querying the underlying <see cref="QuestionnaireService"/> 
    /// and post results back to the calling thread's <see cref="SynchronizationContext"/>.
    /// </summary>
    /// <seealso cref="IOperationResult"/>
    /// <seealso cref="IOperationResult'T"/>
    [Export(typeof(IQuestionnaireRepository))]
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

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Packs exception in result object.")]
        public void GetQuestionnaireTemplatesAsync(Action<IOperationResult<IEnumerable<QuestionnaireTemplate>>> callback)
        {
            this.service.BeginGetQuestionnaireTemplates(
                (ar) =>
                {
                    OperationResult<IEnumerable<QuestionnaireTemplate>> operationResult = new OperationResult<IEnumerable<QuestionnaireTemplate>>();
                    try
                    {
                        operationResult.Result = service.EndGetQuestionnaireTemplates(ar);
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

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Packs exception in result object.")]
        public void SubmitQuestionnaireAsync(Questionnaire questionnaire, Action<IOperationResult> callback)
        {
            this.service.BeginSubmitQuestionnaire(
                questionnaire,
                (ar) =>
                {
                    var operationResult = new OperationResult();

                    try
                    {
                        this.service.EndSubmitQuestionnaire(ar);
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

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Packs exception in result object.")]
        public void GetQuestionnaireTemplateSummaryAsync(QuestionnaireTemplate questionnaireTemplate, Action<IOperationResult<QuestionnaireTemplateSummary>> callback)
        {
            this.service.BeginGetQuestionnaireTemplateSummary(
                questionnaireTemplate,
                (ar) =>
                {
                    var operationResult = new OperationResult<QuestionnaireTemplateSummary>();

                    try
                    {
                        operationResult.Result = this.service.EndGetQuestionnaireTemplateSummary(ar);
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
