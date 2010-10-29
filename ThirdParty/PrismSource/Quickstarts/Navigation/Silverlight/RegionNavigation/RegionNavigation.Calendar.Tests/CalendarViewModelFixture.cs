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
using Microsoft.Practices.Prism.Regions;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionNavigation.Calendar.Model;
using RegionNavigation.Calendar.ViewModels;
using RegionNavigation.Infrastructure;

namespace RegionNavigation.Calendar.Tests
{
    [TestClass]
    public class CalendarViewModelFixture : SilverlightTest
    {
        [TestMethod]
        public void WhenCreated_ThenRequestsMeetingsToService()
        {
            var calendarServiceMock = new Mock<ICalendarService>();
            var requested = false;
            calendarServiceMock
                .Setup(svc => svc.BeginGetMeetings(It.IsAny<AsyncCallback>(), null))
                .Callback(() => requested = true);

            var viewModel = new CalendarViewModel(calendarServiceMock.Object, new Mock<IRegionManager>().Object);

            Assert.IsTrue(requested);
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenMeetingsAreReturned_ThenViewModelIsPopulated()
        {
            var asyncResultMock = new Mock<IAsyncResult>();

            var calendarServiceMock = new Mock<ICalendarService>(MockBehavior.Strict);
            AsyncCallback callback = null;
            calendarServiceMock
                .Setup(svc => svc.BeginGetMeetings(It.IsAny<AsyncCallback>(), null))
                .Callback<AsyncCallback, object>((ac, o) => callback = ac)
                .Returns(asyncResultMock.Object);
            var meeting = new Meeting { };
            calendarServiceMock
                .Setup(svc => svc.EndGetMeetings(asyncResultMock.Object))
                .Returns(new[] { meeting });


            var viewModel = new CalendarViewModel(calendarServiceMock.Object, new Mock<IRegionManager>().Object);

            this.EnqueueConditional(() => callback != null);

            this.EnqueueCallback(
                () =>
                {
                    callback(asyncResultMock.Object);
                });

            this.EnqueueCallback(
                () =>
                {
                    CollectionAssert.AreEqual(viewModel.Meetings, new[] { meeting });
                    calendarServiceMock.VerifyAll();
                });

            this.EnqueueTestComplete();
        }


        [TestMethod]
        public void WhenExecutingTheGotToEmailCommand_ThenNavigatesToEmailView()
        {
            var meeting = new Meeting { EmailId = Guid.NewGuid() };

            var calendarServiceMock = new Mock<ICalendarService>();

            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri(@"EmailView?EmailId=" + meeting.EmailId.ToString("N"), UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName(RegionNames.MainContentRegion)).Returns(true);
            regionManagerMock.Setup(x => x.Regions[RegionNames.MainContentRegion]).Returns(regionMock.Object);

            var viewModel = new CalendarViewModel(calendarServiceMock.Object, regionManagerMock.Object);

            viewModel.OpenMeetingEmailCommand.Execute(meeting);

            regionMock.VerifyAll();
        }
    }
}
