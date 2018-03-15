using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly ILogger _logger;

        public NotesController(ILogger<NotesController> logger)
        {
            _logger=logger;

        }

        [HttpGet("[action]")]
        public IEnumerable<Comment> Notes()
        {

            var comments = new List<Comment>();
            comments.Add(new Comment{CommentId=1, CategoryId=2, Text="this sucks", UpdateUser="Snooper"});
            comments.Add(new Comment{CommentId=2, CategoryId=2, Text="No it does not", UpdateUser="Marcie"});


            _logger.LogDebug("returning {0} comments", comments.Count);
            return comments;
        }

        [HttpGet("[action]")]
        public IEnumerable<Category> Categories()
        {
            var categories= new List<Category>();
            categories.Add(new Category { CategoryId=1, Name ="this is a test"});
            categories.Add(new Category {CategoryId=2, Name ="waiting for another category"});
             _logger.LogDebug("returning {0} categories", categories.Count);
            return categories;

        }
        [HttpPost("[action]")]
        public Comment  NewNote(Comment input) 
        {
            //create a new note and assign an id
            Comment newNote= new Comment();
            newNote.CategoryId=input.CategoryId;
            newNote.Text=input.Text;
            newNote.CommentId=10;

            return newNote;
        }

        [HttpPut("[action]")]
        public Comment Note ([FromBody] Comment input)
        {
            //update an existing note
            _logger.LogDebug("saving note id:{0} text:{1}", input.CommentId, input.Text);
            return input;
        }

        [HttpDelete("[action]")]
        public bool Note (int commentId)
        {
            _logger.LogDebug("deleting note id:{0}", commentId);
            return true;

        }


        public class Comment
        {
            public int CommentId {get;set;}
            public int CategoryId { get; set; }
            public string Text { get; set; }
            public string UpdateUser { get; set; }

        }


        public class Category
        {
            public int CategoryId {get;set;}
            public string Name {get;set;}

        }
    }
}
