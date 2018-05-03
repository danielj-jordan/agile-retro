using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;

namespace apptest
{
    public class NotesControllerTests
    {
        

        [Fact]
        public void GetAllNotes()
        {

            var mock = new Mock<ILogger<app.Controllers.NotesController>>();
            var controller = new app.Controllers.NotesController(mock.Object);

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
            var controller = new app.Controllers.NotesController(mock.Object);

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
