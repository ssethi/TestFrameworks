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
using System.ComponentModel;
using MVVM.Model;

namespace MVVM.ViewModels
{
    public abstract class QuestionViewModel<T> : QuestionViewModel
       where T : Question
    {
        private readonly T question;
        
        protected QuestionViewModel(T question)
        {
            if (question == null)
            {
                throw new ArgumentNullException("question");
            }

            this.question = question;
            this.question.PropertyChanged += this.OnQuestionPropertyChanged;
        }

        public T Question
        {
            get { return this.question; }
        }

        public override bool ResponseComplete
        {
            get { return this.question.IsComplete; }
        }

        protected virtual void OnQuestionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
              
    }
}
