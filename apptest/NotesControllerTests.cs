using System;
using Xunit;

namespace apptest
{
    public class NotesControllerTests
    {
        

        [Fact]
        public void GetAllNotes()
        {
            var controller = new app.Controllers.NotesController();

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
            var controller = new app.Controllers.NotesController();

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
