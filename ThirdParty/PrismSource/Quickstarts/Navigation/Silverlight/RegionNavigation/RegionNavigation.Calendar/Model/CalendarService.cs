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
using System.Threading;
using RegionNavigation.Infrastructure;

namespace RegionNavigation.Calendar.Model
{
    [Export(typeof(ICalendarService))]
    public class CalendarService : ICalendarService
    {
        private readonly List<Meeting> meetings;

        public CalendarService()
        {
            var offset = DateTimeOffset.Now.Offset;

            this.meetings =
                new List<Meeting>
                {
                    new Meeting
                    {
                        Subject = "Marketing plan",
                        StartTime = new DateTimeOffset(2010, 8, 1, 15, 0, 0, offset),
                        EndTime = new DateTimeOffset(2010, 8, 1, 16, 0, 0, offset),
                        EmailId = Guid.Parse("5C5BC399-F03F-4301-B314-2D70C1FF2306")
                    },
                    new Meeting
                    {
                        Subject = "Product brainstorming",
                        StartTime = new DateTimeOffset(2010, 8, 1, 12, 0, 0, offset),
                        EndTime = new DateTimeOffset(2010, 8, 1, 14, 30, 0, offset),
                        EmailId = Guid.Parse("D84FF2F9-144C-4357-8DC7-785394FC99A6")
                    },
                    new Meeting
                    {
                        Subject = "Marketing plan",
                        StartTime = new DateTimeOffset(2010, 8, 2, 15, 0, 0, offset),
                        EndTime = new DateTimeOffset(2010, 8, 2, 16, 0, 0, offset),
                        EmailId = Guid.Parse("DE7C62F9-E15E-4C9D-8500-BD3210C529B8")
                    },
                    new Meeting
                    {
                        Subject = "Planning meeting",
                        StartTime = new DateTimeOffset(2010, 8, 5, 9, 0, 0, offset),
                        EndTime = new DateTimeOffset(2010, 8, 5, 11, 0, 0, offset),
                        EmailId = Guid.Parse("687E0458-A3B2-4688-8CEE-BD0E63A01C10")
                    }
                };
        }

        public IAsyncResult BeginGetMeetings(AsyncCallback callback, object userState)
        {
            var asyncResult = new AsyncResult<IEnumerable<Meeting>>(callback, userState);
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    asyncResult.SetComplete(new ReadOnlyCollection<Meeting>(this.meetings), false);
                });

            return asyncResult;
        }

        public IEnumerable<Meeting> EndGetMeetings(IAsyncResult asyncResult)
        {
            var localAsyncResult = AsyncResult<IEnumerable<Meeting>>.End(asyncResult);

            return localAsyncResult.Result;
        }
    }
}
