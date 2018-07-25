using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using app.Domain;
using Retrospective.Data.Model;


namespace apptest
{
    public class NotesControllerTests
    {
        AutoMapper.Mapper mapper= null;
        
        public NotesControllerTests()
        {
                
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<app.Domain.DomainProfile>();
            });

            mapper= new Mapper(config);
            
        }

        [Fact]
        public void GetAllNotes()
        {


            //var mock = new Mock<ILogger<app.Controllers.NotesController>>();

            //var mockDB = new Mock<
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.NotesController(logger, mapper);

            var notes = controller.Notes();
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

            var controller = new app.Controllers.NotesController(logger, mapper);

            var categories = controller.Categories();
            var count=0;
            foreach (var category in categories)
            {
                count++;
            }
            Assert.True(count>0, "there should be some categories");
            

        }        
     }
  }
