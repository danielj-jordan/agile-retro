using System;
using Xunit;
using Microsoft.Extensions.Logging;
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
        
        public NotesControllerTest(TestFixture fixture)
        {
            this.fixture=fixture;

            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<app.Domain.DomainProfile>();
            });

            mapper= new Mapper(config);
            
        }

        [Fact]
        public void GetAllNotes()
        {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper, fixture.Database);

            var notes = controller.Notes(fixture.SessionId.ToString());
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

            var controller = new app.Controllers.NotesController(logger, mapper, fixture.Database);

            var categories = controller.Categories(fixture.SessionId.ToString());
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

            var controller = new app.Controllers.NotesController(logger, mapper, fixture.Database);

            var beginCount= fixture.Database.Comments.GetComments(fixture.SessionId.ToString()).Count;


            var categories = controller.DeleteNote(fixture.DeleteNote.ToString());

             var endCount= fixture.Database.Comments.GetComments(fixture.SessionId.ToString()).Count;

            Assert.True(endCount<beginCount, "comment was not deleted");
            

        } 

        [Fact]
        public void CreateNote(){

            var mock = new Mock<ILogger<app.Controllers.NotesController>>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper, fixture.Database);

            var beginCount= fixture.Database.Comments.GetComments(fixture.SessionId.ToString()).Count;

            app.Model.Comment comment = new app.Model.Comment();
            comment.Text="this is a new comment";
            comment.CategoryId=2;
            

            var categories = controller.NewNote(fixture.SessionId.ToString(), comment);

             var endCount= fixture.Database.Comments.GetComments(fixture.SessionId.ToString()).Count;

            Assert.True(endCount>beginCount, "comment was not created");
            

        }



        [Fact]
        public void UpdateNote(){

            var mock = new Mock<ILogger<app.Controllers.NotesController>>();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper, fixture.Database);

            var beginCount= fixture.Database.Comments.GetComments(fixture.SessionId.ToString()).Count;


            app.Model.Comment comment = new app.Model.Comment();
            comment.Text="this is a new comment for existing note";
            comment.CategoryId=2;
            comment.CommentId= this.fixture.UpdateNote.ToString();

            var categories = controller.Note(fixture.SessionId.ToString(), comment);

             var endCount= fixture.Database.Comments.GetComments(fixture.SessionId.ToString()).Count;

            Assert.True(endCount==beginCount, "comment was updated");
            

        }
    }     
  }
