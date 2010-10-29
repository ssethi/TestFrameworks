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
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using RegionNavigation.Infrastructure;

namespace RegionNavigation.Email.Model
{
    [Export(typeof(IEmailService))]
    public class EmailService : IEmailService
    {
        private readonly List<EmailDocument> emailDocuments;

        public EmailService()
        {
            this.emailDocuments =
                new List<EmailDocument>
                {
                    new EmailDocument { From = "Chrstine Koch", To = "Robert Zare", Subject = "Important news", Text = "Text" },
                    new EmailDocument { From = "Chrstine Koch", To = "Robert Zare", Subject = "RSVP", Text = "Text" },

                    new EmailDocument(Guid.Parse("5C5BC399-F03F-4301-B314-2D70C1FF2306")) 
                        { From = "Chrstine Koch", To = "Robert Zare", Subject = "Marketing plan", Text = "Let's discuss the marketing plan." },
                    new EmailDocument(Guid.Parse("D84FF2F9-144C-4357-8DC7-785394FC99A6"))
                        { From = "Chrstine Koch", To = "Robert Zare", Subject = "Product brainstorming", Text = "Text" },
                    new EmailDocument(Guid.Parse("DE7C62F9-E15E-4C9D-8500-BD3210C529B8"))
                        { From = "Chrstine Koch", To = "Robert Zare", Subject = "Marketing plan", Text = "Let's discuss the marketing plan. Again." },
                    new EmailDocument(Guid.Parse("687E0458-A3B2-4688-8CEE-BD0E63A01C10"))
                        { From = "Chrstine Koch", To = "Robert Zare", Subject = "Planning meeting", Text = "This is the agenda..." },
                };
        }

        public IAsyncResult BeginGetEmailDocuments(AsyncCallback callback, object userState)
        {
            var asyncResult = new AsyncResult<IEnumerable<EmailDocument>>(callback, userState);
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    asyncResult.SetComplete(new ReadOnlyCollection<EmailDocument>(this.emailDocuments), false);
                });

            return asyncResult;
        }

        public IEnumerable<EmailDocument> EndGetEmailDocuments(IAsyncResult asyncResult)
        {
            var localAsyncResult = AsyncResult<IEnumerable<EmailDocument>>.End(asyncResult);

            return localAsyncResult.Result;
        }

        public IAsyncResult BeginSendEmailDocument(EmailDocument email, AsyncCallback callback, object userState)
        {
            var asyncResult = new AsyncResult<object>(callback, userState);
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    Thread.Sleep(500);
                    asyncResult.SetComplete(null, false);
                });

            return asyncResult;
        }

        public void EndSendEmailDocument(IAsyncResult asyncResult)
        {
            var localAsyncResult = AsyncResult<object>.End(asyncResult);
        }

        public EmailDocument GetEmailDocument(Guid id)
        {
            return this.emailDocuments.FirstOrDefault(e => e.Id == id);
        }
    }
}
