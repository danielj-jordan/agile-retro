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
using app.ModelExtensions;

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
             var user = HttpContext.User.Identity.Name;
            _logger.LogInformation("active user is {0}", user);
            return user;
        }

        [Authorize]
        [HttpGet ("[action]/{retroId}")]
        public IEnumerable<Comment> Notes (string retroId) {


            var comments = manager.GetComments(this.GetActiveUser(), retroId).Select(
                c => c.ToViewModelComment(this.GetActiveUser())
            ).ToList();

            //var comments = _mapper.Map<List<DomainModel.Comment>, List<app.Model.Comment>> (manager.GetComments (this.GetActiveUser(),retroId));

            _logger.LogDebug ("returning {0} comments", comments.Count);
            return (IEnumerable<Comment>) comments;



        }

        [Authorize]
        [HttpGet ("[action]/{retroId}")]
        public IEnumerable<Category> Categories (string retroId) {
            var categories = manager.GetCategories (this.GetActiveUser(), retroId);
             _logger.LogDebug ("returning {0} categories", categories.Count);
            return _mapper.Map<List<DomainModel.Category>, List<app.Model.Category>> (categories);

        }

        [Authorize]
        [HttpPost ("[action]/{retroId}")]
        public Comment NewNote (string retroId,[FromBody] Comment input) {
            //create a new note and assign an id

            DomainModel.Comment newNote = new DomainModel.Comment();
            newNote.CategoryNumber = input.CategoryNum;
            newNote.Text = input.Text;
            newNote.MeetingId= retroId;

            var comment = manager.SaveComment(this.GetActiveUser(),newNote);

            _logger.LogDebug ("text:{0}", newNote.Text);

            return comment.ToViewModelComment(this.GetActiveUser());
            //return _mapper.Map<DomainModel.Comment, app.Model.Comment>(comment);
        }

        [Authorize]
        [HttpPut ("[action]/{retroId}")]
        public Comment Note (string retroId, [FromBody] Comment input) {
            //update an existing note
            _logger.LogDebug ("saving note id:{0} text:{1}", input.CommentId, input.Text);
           
            DomainModel.Comment newNote = new DomainModel.Comment();
            newNote.CategoryNumber = input.CategoryNum;
            newNote.Text = input.Text;
            newNote.MeetingId= retroId;
            newNote.CommentId= input.CommentId;

            DomainModel.Comment comment;
           if(string.IsNullOrWhiteSpace(input.CommentId))
           {
            comment = manager.SaveComment(this.GetActiveUser(),newNote);
           }
           else
           {
               comment = manager.UpdateCommentText(this.GetActiveUser(), newNote);
           }

            _logger.LogDebug ("text:{0}", newNote.Text);

            return comment.ToViewModelComment(this.GetActiveUser());
        }

        [Authorize]
        [HttpDelete ("[action]/{commentId}")]
        public bool DeleteNote (string commentId) {
            _logger.LogDebug ("deleting note id:{0}", commentId);
            manager.DeleteComment(this.GetActiveUser(),commentId);
            return true;

        }

        [Authorize]
        [HttpPut ("[action]/{commentId}")]
        public ActionResult VoteUp (string commentId) {
            _logger.LogDebug ("vote up note id:{0}", commentId);
            manager.VoteUp(this.GetActiveUser(),commentId);
            return new EmptyResult();

        }

        [Authorize]
        [HttpPut ("[action]/{commentId}")]
        public ActionResult VoteDown (string commentId) {
            _logger.LogDebug ("vote down id:{0}", commentId);
            manager.VoteDown(this.GetActiveUser(),commentId);
            return new EmptyResult() ;

        }

    }
}