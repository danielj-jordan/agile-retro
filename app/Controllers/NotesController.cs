using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DBModel = Retrospective.Data.Model;
using AutoMapper;
using MongoDB.Bson;
using Retrospective.Data;

namespace app.Controllers {
    
    [Route ("api/[controller]")]
    [ApiController]
    public class NotesController : Controller {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        private readonly Database database;

        public NotesController (ILogger<NotesController> logger,
            IMapper mapper, Database database) {
            _logger = logger;
            _mapper = mapper;
            this.database = database;

        }

        [HttpGet ("[action]/{retroId}")]
        public IEnumerable<Comment> Notes (string retroId) {

            DataComment commentdata = new DataComment (this.database);

            var test = commentdata.GetComments (retroId);
            _logger.LogDebug ("db returning {0} comments", test.Count);

            var comments = _mapper.Map<List<DBModel.Comment>, List<app.Model.Comment>> (commentdata.GetComments (retroId));

            _logger.LogDebug ("returning {0} comments", comments.Count);
            return (IEnumerable<Comment>) comments;
        }

        [HttpGet ("[action]/{retroId}")]
        public IEnumerable<Category> Categories (string retroId) {
            var session = this.database.Meetings.Get (new ObjectId (retroId));
             _logger.LogDebug ("returning {0} categories", session.Categories.ToList().Count);
            return _mapper.Map<List<DBModel.Category>, List<app.Model.Category>> (session.Categories.ToList ());

        }

        [HttpPost ("[action]/{retroId}")]
        public Comment NewNote (string retroId,[FromBody] Comment input) {
            //create a new note and assign an id

            DBModel.Comment newNote = new DBModel.Comment();
            newNote.CategoryNumber = input.CategoryNum;
            newNote.Text = input.Text;
            newNote.RetrospectiveId= new ObjectId(retroId);

            var comment = this.database.Comments.SaveComment(newNote);

            _logger.LogDebug ("text:{0}", newNote.Text);

            return _mapper.Map<DBModel.Comment, app.Model.Comment>(comment);
        }

        [HttpPut ("[action]/{retroId}")]
        public Comment Note (string retroId, [FromBody] Comment input) {
            //update an existing note
            _logger.LogDebug ("saving note id:{0} text:{1}", input.CommentId, input.Text);
           
            DBModel.Comment newNote = new DBModel.Comment();
            newNote.CategoryNumber = input.CategoryNum;
            newNote.Text = input.Text;
            newNote.RetrospectiveId= new ObjectId(retroId);
            newNote.Id= new ObjectId(input.CommentId);

            var comment = this.database.Comments.SaveComment(newNote);

            _logger.LogDebug ("text:{0}", newNote.Text);

            return _mapper.Map<DBModel.Comment, app.Model.Comment>(comment);

        }

        [HttpDelete ("[action]/{commentId}")]
        public bool DeleteNote (string commentId) {
            _logger.LogDebug ("deleting note id:{0}", commentId);
            this.database.Comments.Delete(new ObjectId(commentId));
            return true;

        }

    }
}