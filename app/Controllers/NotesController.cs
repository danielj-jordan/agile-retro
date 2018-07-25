using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app.Model;
using DBModel=Retrospective.Data.Model;
using AutoMapper;
using Retrospective.Data;


namespace app.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public NotesController(ILogger<NotesController> logger,
        IMapper mapper)
        {
            _logger=logger;
            _mapper=mapper;

        }

        [HttpGet("[action]")]
        public IEnumerable<Comment> Notes()
        {
            String dbName="test";
            string retroId="000000000000000000000000";
            IDatabase database = new Database(dbName);
            DataComment commentdata = new DataComment(database);
            
            var test =commentdata.GetComments(retroId);
            _logger.LogDebug("db returning {0} comments", test.Count);

            var comments =_mapper.Map<List<DBModel.Comment>,List<app.Model.Comment> >( commentdata.GetComments(retroId));


            /*
            var comments = new List<Comment>();
            comments.Add(new Comment{CommentId="1", CategoryId=2, Text="this sucks", UpdateUser="Snooper"});
            comments.Add(new Comment{CommentId="2", CategoryId=2, Text="No it does not", UpdateUser="Marcie"});
            */

            _logger.LogDebug("returning {0} comments", comments.Count);
            return (IEnumerable<Comment>)comments;
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


            var temp =new System.Random();
            newNote.CommentId= (temp.Next(100,10000)).ToString();

            _logger.LogDebug("text:{0}", newNote.Text);                               

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



    }
}
