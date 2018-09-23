using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using MongoDB.Bson;
using app.Domain;
using Retrospective.Data.Model;




namespace apptest
{
    [Collection ("Controller Test collection")]
    public class MeetingControllerTest
    {
        
        AutoMapper.Mapper mapper= null;
        TestFixture fixture;
        
        public MeetingControllerTest(TestFixture fixture)
        {
            this.fixture=fixture;

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<app.Domain.DomainProfile>();
            });

            mapper= new Mapper(config);
            
        }


        /* 
        [Fact]
        public void MeetingMapper(){
            app.Model.Meeting meeting = new app.Model.Meeting();
            meeting.Id="123";
            meeting.Name="test";
            meeting.TeamId="456";


            Retrospective.Data.Model.Meeting dbMeeting=  mapper.Map<app.Model.Meeting, Retrospective.Data.Model.Meeting>(meeting);

            Assert.Equal(dbMeeting.Id.ToString(),meeting.Id);

        }
        */


        [Fact]
        public void GetMeetingsForTeam () {



            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.MeetingController> ();

            var controller = new app.Controllers.MeetingController (logger, mapper, fixture.Database);

            var meetings = controller.Meetings (fixture.TeamId.ToString());

            Assert.True (meetings.Value.ToList ().Count () > 0);

        }

        [Fact]
        public void GetMeeting () {



            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.MeetingController> ();

            var controller = new app.Controllers.MeetingController (logger, mapper, fixture.Database);

            var meeting = controller.Meeting (fixture.SessionId.ToString());

            Assert.True (!String.IsNullOrEmpty(meeting.Value.Name));

        }


        [Fact]
        public void SaveMeeting () {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.MeetingController> ();

            var controller = new app.Controllers.MeetingController (logger, mapper, fixture.Database);

            app.Model.Meeting meeting = new app.Model.Meeting();
            meeting.TeamId=this.fixture.TeamId.ToString();
            meeting.Name="test meeting";
            meeting.Categories= new app.Model.Category[1];
            meeting.Categories[0]= new app.Model.Category();
            meeting.Categories[0].CategoryNum=1;
            meeting.Categories[0].Name="test category";

            var meetingResult = controller.Meeting (meeting);

            Assert.True (!String.IsNullOrEmpty(meetingResult.Value.Id));

        }

        [Fact]
        public void UpdateMeeting () {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.MeetingController> ();

            var controller = new app.Controllers.MeetingController (logger, mapper, fixture.Database);



            var meetingStart = controller.Meeting(fixture.SessionId.ToString());

            meetingStart.Value.Name+= "more";

            var meetingEnd=controller.Meeting(meetingStart.Value);

            
            Assert.Equal( meetingStart.Value.Id, meetingEnd.Value.Id);

            Assert.Contains("more", meetingEnd.Value.Name);

        }

    }
}