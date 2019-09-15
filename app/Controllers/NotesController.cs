using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using DomainModel = Retrospective.Domain.Model;
using app.Model;
using Retrospective.Domain;
using app.ModelExtensions;

namespace app.Controllers {
    
    [Route ("api/[controller]")]
    [ApiController]
    public class NotesController : Controller {
        private readonly ILogger _logger;

       private readonly ICommentManager manager;
       private readonly IMeetingManager meetingManager;

        public NotesController (ILogger<NotesController> logger,
            ICommentManager manager, IMeetingManager meetingManager) {
            _logger = logger;
            this.manager = manager;
            this.meetingManager=meetingManager;

        }

         private string GetActiveUserId(){
             var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _logger.LogInformation("active user is {0}", user);
            return user;
        }

        [Authorize]
        [HttpGet ("[action]/{retroId}")]
        public IEnumerable<Comment> Notes (string retroId) {


            var comments = manager.GetComments(this.GetActiveUserId(), retroId).Select(
                c => c.ToViewModel(this.GetActiveUserId())
            ).ToList();

            //var comments = _mapper.Map<List<DomainModel.Comment>, List<app.Model.Comment>> (manager.GetComments (this.GetActiveUser(),retroId));

            _logger.LogDebug ("returning {0} comments", comments.Count);
            return (IEnumerable<Comment>) comments;



        }

        [Authorize]
        [HttpGet ("[action]/{retroId}")]
        public IEnumerable<Category> Categories (string retroId) {
            var categories = manager.GetCategories (this.GetActiveUserId(), retroId);
             _logger.LogDebug ("returning {0} categories", categories.Count);
             
            var meeting = meetingManager.GetMeeting(this.GetActiveUserId(), retroId);
            return meeting.ToViewModel().Categories;
            //return _mapper.Map<List<DomainModel.Category>, List<app.Model.Category>> (categories);

        }

        [Authorize]
        [HttpPost ("[action]/{retroId}")]
        public Comment NewNote (string retroId,[FromBody] Comment input) {
            //create a new note and assign an id

            DomainModel.Comment newNote = new DomainModel.Comment();
            newNote.CategoryNumber = input.CategoryNum;
            newNote.Text = input.Text;
            newNote.MeetingId= retroId;

            var comment = manager.SaveComment(this.GetActiveUserId(),newNote);

            _logger.LogDebug ("text:{0}", newNote.Text);

            return comment.ToViewModel(this.GetActiveUserId());
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
            comment = manager.SaveComment(this.GetActiveUserId(),newNote);
           }
           else
           {
               comment = manager.UpdateCommentText(this.GetActiveUserId(), newNote);
           }

            _logger.LogDebug ("text:{0}", newNote.Text);

            return comment.ToViewModel(this.GetActiveUserId());
        }

        [Authorize]
        [HttpDelete ("[action]/{commentId}")]
        public bool DeleteNote (string commentId) {
            _logger.LogDebug ("deleting note id:{0}", commentId);
            manager.DeleteComment(this.GetActiveUserId(),commentId);
            return true;

        }

        [Authorize]
        [HttpPut ("[action]/{commentId}")]
        public ActionResult VoteUp (string commentId) {
            _logger.LogDebug ("vote up note id:{0}", commentId);
            manager.VoteUp(this.GetActiveUserId(),commentId);
            return new EmptyResult();

        }

        [Authorize]
        [HttpPut ("[action]/{commentId}")]
        public ActionResult VoteDown (string commentId) {
            _logger.LogDebug ("vote down id:{0}", commentId);
            manager.VoteDown(this.GetActiveUserId(),commentId);
            return new EmptyResult() ;

        }

    }
}