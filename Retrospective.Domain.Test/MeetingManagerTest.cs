using System;
using System.Collections.Generic;
using AutoMapper;
using Xunit;

namespace Retrospective.Domain.Test {
    [Collection ("Domain Test collection")]
    public class MeetingManagerTest {

        AutoMapper.Mapper mapper = null;
        TestFixture fixture;

        public MeetingManagerTest (TestFixture fixture) {
            this.fixture = fixture;
            var config = new MapperConfiguration (c => {
                c.AddProfile<Retrospective.Domain.DomainProfile> ();
            });

            mapper = new Mapper (config);
        }

        [Fact]
        public void GetMeetingsForTeam () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            List<Retrospective.Domain.Model.Meeting> meetings = manager.GetMeetingsForTeam (fixture.SampleUser, fixture.TeamId.ToString ());
            Assert.Equal (meetings[0].TeamId, fixture.TeamId.ToString ());

        }

        [Fact]
        public void GetMeetingsForTeamFail_NotMember () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.GetMeetingsForTeam ("", fixture.TeamId.ToString ());
                }
            );

        }

        [Fact]
        public void GetMeeting () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            List<Retrospective.Domain.Model.Meeting> meetings = manager.GetMeetingsForTeam (fixture.SampleUser, fixture.TeamId.ToString ());

            Retrospective.Domain.Model.Meeting meeting = manager.GetMeeting (fixture.SampleUser, meetings[0].Id);
            Assert.Equal (meeting.TeamId, fixture.TeamId.ToString ());

        }

        [Fact]
        public void GetMeetingFail_NotMember () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            List<Retrospective.Domain.Model.Meeting> meetings = manager.GetMeetingsForTeam (fixture.SampleUser, fixture.TeamId.ToString ());

            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.GetMeeting ("", meetings[0].Id);
                });
        }

        [Fact]
        public void SaveMeeting_NewMeeting () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            var newMeeting = new Model.Meeting ();
            newMeeting.TeamId = fixture.TeamId.ToString ();
            newMeeting.Name = "test meeting";
            newMeeting.Categories = new Model.Category[0];

            var saved = manager.SaveMeeting (fixture.Owner, newMeeting);
            Assert.True (!String.IsNullOrEmpty (saved.Id));

        }

        [Fact]
        public void SaveMeeting_ExistingMeeting () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            List<Retrospective.Domain.Model.Meeting> meetings = manager.GetMeetingsForTeam (fixture.SampleUser, fixture.TeamId.ToString ());

            var beginMeeting = manager.GetMeeting (fixture.Owner, meetings[0].Id);

            beginMeeting.Name += " more";
            var endMeeting = manager.SaveMeeting (fixture.Owner, beginMeeting);

            Assert.Equal (beginMeeting.Id, endMeeting.Id);
            Assert.Equal (beginMeeting.TeamId, endMeeting.TeamId);
            Assert.Contains ("more", endMeeting.Name);

        }

        [Fact]
        public void SaveMeeting_NewMeetingFail_NotOwner () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            var newMeeting = new Model.Meeting ();
            newMeeting.TeamId = fixture.TeamId.ToString ();
            newMeeting.Name = "test meeting";
            newMeeting.Categories = new Model.Category[0];

            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.SaveMeeting (fixture.SampleUser, newMeeting);
                });

        }

        [Fact]
        public void SaveMeeting_ExistingMeetingFail_NotOwner () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            List<Retrospective.Domain.Model.Meeting> meetings = manager.GetMeetingsForTeam (fixture.SampleUser, fixture.TeamId.ToString ());

            var beginMeeting = manager.GetMeeting (fixture.Owner, meetings[0].Id);
            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.SaveMeeting (fixture.SampleUser, beginMeeting);
                });
        }

        [Fact]
        public void SaveMeeting_ExistingMeetingFail_ChangeTeam () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            var manager = new Retrospective.Domain.MeetingManager (logger, mapper, fixture.Database);

            List<Retrospective.Domain.Model.Meeting> meetings = manager.GetMeetingsForTeam (fixture.SampleUser, fixture.TeamId.ToString ());

            var beginMeeting = manager.GetMeeting (fixture.Owner, meetings[0].Id);
            beginMeeting.TeamId = "123";
            Assert.Throws<Exception.AccessDenied> (
                () => {

                    manager.SaveMeeting (fixture.Owner, beginMeeting);
                });
        }
    }

}