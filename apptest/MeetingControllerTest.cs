using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using AutoMapper;
using MongoDB.Bson;
using Retrospective.Data.Model;




namespace apptest
{
    [Collection ("Controller Test collection")]
    public class MeetingControllerTest
    {
        
        AutoMapper.Mapper mapper= null;
        TestFixture fixture;

        Retrospective.Domain.MeetingManager manager;
        
        public MeetingControllerTest(TestFixture fixture)
        {
            this.fixture=fixture;

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<app.Domain.DomainProfile>();
                c.AddProfile<Retrospective.Domain.DomainProfile> ();
            });

            mapper= new Mapper(config);

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.MeetingManager> ();
            manager= new Retrospective.Domain.MeetingManager(logger, mapper,fixture.Database);
            
        }


        private void MockHttpContextValid (app.Controllers.MeetingController controller) {
            controller.ControllerContext = new ControllerContext ();
            controller.ControllerContext.HttpContext = new DefaultHttpContext ();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal (new ClaimsIdentity (new Claim[] {
                new Claim (ClaimTypes.Name, fixture.Owner.Id.ToString())
            }, "someAuthTypeName"));

        }


        [Fact]
        public void GetMeetingsForTeam () {



            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.MeetingController> ();

            var controller = new app.Controllers.MeetingController (logger, mapper, 
                manager);

            MockHttpContextValid(controller);

            var meetings = controller.Meetings (fixture.TeamId.ToString());

            Assert.True (meetings.Value.ToList ().Count () > 0);

        }

        [Fact]
        public void GetMeeting () {



            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.MeetingController> ();

            var controller = new app.Controllers.MeetingController (logger, mapper, 
                            manager);

            MockHttpContextValid(controller);

            var meeting = controller.Meeting (fixture.MeetingId.ToString());

            Assert.True (!String.IsNullOrEmpty(meeting.Value.Name));

        }


        [Fact]
        public void SaveMeeting () {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.MeetingController> ();

            var controller = new app.Controllers.MeetingController (logger, mapper, manager);
            MockHttpContextValid(controller);

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

            var controller = new app.Controllers.MeetingController (logger, mapper,  manager);
             MockHttpContextValid(controller);

            var meetingStart = controller.Meeting(fixture.MeetingId.ToString());

            meetingStart.Value.Name+= "more";

            var meetingEnd=controller.Meeting(meetingStart.Value);
            
            Assert.Equal( meetingStart.Value.Id, meetingEnd.Value.Id);
            Assert.Contains("more", meetingEnd.Value.Name);

        }

    }
}