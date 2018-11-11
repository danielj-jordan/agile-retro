using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using AutoMapper;
using DomainModel = Retrospective.Domain.Model;
using app.Model;
using Retrospective.Domain;

namespace app.Controllers {
    
    [Route ("api/[controller]")]
    [ApiController]
    public class NotesController : Controller {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

       private readonly CommentManager manager;

        public NotesController (ILogger<NotesController> logger,
            IMapper mapper, CommentManager manager) {
            _logger = logger;
            _mapper = mapper;
            this.manager = manager;

        }

         private string GetActiveUser(){
            return HttpContext.User.Identity.Name;
        }

        [HttpGet ("[action]/{retroId}")]
        public IEnumerable<Comment> Notes (string retroId) {

            var comments = _mapper.Map<List<DomainModel.Comment>, List<app.Model.Comment>> (manager.GetComments (this.GetActiveUser(),retroId));

            _logger.LogDebug ("returning {0} comments", comments.Count);
            return (IEnumerable<Comment>) comments;
        }

        [HttpGet ("[action]/{retroId}")]
        public IEnumerable<Category> Categories (string retroId) {
            var categories = manager.GetCategories (this.GetActiveUser(), retroId);
             _logger.LogDebug ("returning {0} categories", categories.Count);
            return _mapper.Map<List<DomainModel.Category>, List<app.Model.Category>> (categories);

        }

        [HttpPost ("[action]/{retroId}")]
        public Comment NewNote (string retroId,[FromBody] Comment input) {
            //create a new note and assign an id

            DomainModel.Comment newNote = new DomainModel.Comment();
            newNote.CategoryNumber = input.CategoryNum;
            newNote.Text = input.Text;
            newNote.MeetingId= retroId;

            var comment = manager.SaveComment(this.GetActiveUser(),newNote);

            _logger.LogDebug ("text:{0}", newNote.Text);

            return _mapper.Map<DomainModel.Comment, app.Model.Comment>(comment);
        }

        [HttpPut ("[action]/{retroId}")]
        public Comment Note (string retroId, [FromBody] Comment input) {
            //update an existing note
            _logger.LogDebug ("saving note id:{0} text:{1}", input.CommentId, input.Text);
           
            DomainModel.Comment newNote = new DomainModel.Comment();
            newNote.CategoryNumber = input.CategoryNum;
            newNote.Text = input.Text;
            newNote.MeetingId= retroId;
            newNote.CommentId= input.CommentId;

            var comment = manager.SaveComment(this.GetActiveUser(),newNote);

            _logger.LogDebug ("text:{0}", newNote.Text);

            return _mapper.Map<DomainModel.Comment, app.Model.Comment>(comment);

        }

        [HttpDelete ("[action]/{commentId}")]
        public bool DeleteNote (string commentId) {
            _logger.LogDebug ("deleting note id:{0}", commentId);
            manager.DeleteComment(this.GetActiveUser(),commentId);
            return true;

        }

    }
}