using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using AutoMapper;
using app.Domain;
using Retrospective.Data.Model;
using MongoDB.Bson;


namespace apptest
{
    [Collection ("Controller Test collection")]
    public class NotesControllerTest
    {
        AutoMapper.Mapper mapper= null;
        TestFixture fixture;

        Retrospective.Domain.CommentManager manager;
        
        public NotesControllerTest(TestFixture fixture)
        {
            this.fixture=fixture;
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<app.Domain.DomainProfile>();
            });
            mapper= new Mapper(config);
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.CommentManager> ();
            manager= new Retrospective.Domain.CommentManager(logger, fixture.Database);
        }

    private void MockHttpContextValid(app.Controllers.NotesController controller, User user)
    {
      controller.ControllerContext = new ControllerContext();
      controller.ControllerContext.HttpContext = new DefaultHttpContext();
      controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
      {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
      }, "someAuthTypeName"));

    }

        [Fact]
        public void GetAllNotes()
        {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper, manager);
            MockHttpContextValid(controller, fixture.Owner);

            var notes = controller.Notes(fixture.MeetingId.ToString());
            var count=0;
            foreach(var note in notes)
            {
                    count++;
            }
            Assert.True(count>0, "there should be some notes.");

        }


        [Fact]
        public void GetAllCategories()
        {
            var mock = new Mock<ILogger<app.Controllers.NotesController>>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper, manager);
            MockHttpContextValid(controller, fixture.Owner);

            var categories = controller.Categories(fixture.MeetingId.ToString());
            var count=0;
            foreach (var category in categories)
            {
                count++;
            }
            Assert.True(count>0, "there should be some categories");
            

        }        
     

        [Fact]
        public void DeleteNote()
        {
            var mock = new Mock<ILogger<app.Controllers.NotesController>>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper, manager);
            MockHttpContextValid(controller, fixture.Owner);

            var beginCount= fixture.Database.Comments.GetComments(fixture.MeetingId.ToString()).Count;


            var categories = controller.DeleteNote(fixture.DeleteNote.ToString());

             var endCount= fixture.Database.Comments.GetComments(fixture.MeetingId.ToString()).Count;

            Assert.True(endCount<beginCount, "comment was not deleted");
            

        } 

        [Fact]
        public void CreateNote(){

            var mock = new Mock<ILogger<app.Controllers.NotesController>>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper, manager);
            MockHttpContextValid(controller,fixture.Owner);

            var beginCount= fixture.Database.Comments.GetComments(fixture.MeetingId.ToString()).Count;

            app.Model.Comment comment = new app.Model.Comment();
            comment.Text="this is a new comment";
            comment.CategoryNum=2;
            comment.MeetingId=fixture.MeetingId.ToString();
            comment.UpdateUserId=fixture.SampleUser.Id.ToString();
            

            var categories = controller.NewNote(fixture.MeetingId.ToString(), comment);

            var endCount= fixture.Database.Comments.GetComments(fixture.MeetingId.ToString()).Count;

            Assert.True(endCount>beginCount, "comment was not created");
            

        }



        [Fact]
        public void UpdateNote(){

            var mock = new Mock<ILogger<app.Controllers.NotesController>>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper, manager);
            MockHttpContextValid(controller, fixture.Owner);

            var beginCount= fixture.Database.Comments.GetComments(fixture.MeetingId.ToString()).Count;


            app.Model.Comment comment = new app.Model.Comment();
            comment.Text="this is a new comment for existing note";
            comment.CategoryNum=2;
            comment.CommentId= this.fixture.UpdateNote.ToString();

            var categories = controller.Note(fixture.MeetingId.ToString(), comment);

            var endCount= fixture.Database.Comments.GetComments(fixture.MeetingId.ToString()).Count;

            Assert.True(endCount==beginCount, "comment was updated");
            

        }
    }     
  }
