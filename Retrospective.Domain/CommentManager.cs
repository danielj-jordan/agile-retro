using System;
using System.Collections.Generic;
using System.Linq;
using DBModel = Retrospective.Data.Model;
using DomainModel = Retrospective.Domain.Model;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Retrospective.Data;
using Retrospective.Domain.ModelExtensions;

namespace Retrospective.Domain
{
  public class CommentManager : BaseManager
  {
    private readonly ILogger<CommentManager> logger;
    private readonly IDatabase database;

    public CommentManager(ILogger<CommentManager> logger, IDatabase database) : base(logger,  database)
    {
      this.logger = logger;
      this.database = database;
    }

    public List<DomainModel.Comment> GetComments(string activeUser, string meetingId)
    {
      var meeting = this.GetMeeting(meetingId);
      var team = this.GetTeam(meeting.TeamId);
      if (!IsTeamMember(activeUser, team))
      {
        throw new Exception.AccessDenied();
      }

      return database.Comments.GetComments(meetingId).Select(c => c.ToDomainModel()).ToList();
    }

    public List<DomainModel.Category> GetCategories(string activeUser, string meetingId)
    {
      var meeting = this.GetMeeting(meetingId);
      var team = this.GetTeam(meeting.TeamId);
      if (!IsTeamMember(activeUser, team))
      {
        throw new Exception.AccessDenied();
      }

      return meeting.Categories.ToList();
    }

    public void DeleteComment(string activeUser, string commentId)
    {
      var comment = this.GetComment(commentId);
      var meeting = this.GetMeeting(comment.MeetingId);
      var team = this.GetTeam(meeting.TeamId);
      if (!IsTeamMember(activeUser, team))
      {
        throw new Exception.AccessDenied();
      }

      database.Comments.Delete(new ObjectId(commentId));
    }

    public DomainModel.Comment SaveComment(string activeUser, DomainModel.Comment comment)
    {
      var meeting = this.GetMeeting(comment.MeetingId);
      var team = this.GetTeam(meeting.TeamId);

      if (!IsTeamMember(activeUser, team))
      {
        throw new Exception.AccessDenied();
      }

      comment.LastUpdateUserId= activeUser;
      comment.LastUpdateDate=DateTime.UtcNow;

      return database.Comments.SaveComment(comment.ToDBModel()).ToDomainModel();
      
    }

    public DomainModel.Comment UpdateCommentText(
      string activeUserId,
      DomainModel.Comment comment
    )
    {
      var meeting = this.GetMeeting(comment.MeetingId);
      var team = this.GetTeam(meeting.TeamId);
      if (!IsTeamMember(activeUserId, team))
      {
        throw new Exception.AccessDenied();
      }

      //retrieve existing comment
      var existingComment = this.GetComment(comment.CommentId);

      //update the comment
      existingComment.Text = comment.Text;
      existingComment.LastUpdateUserId = activeUserId;
      existingComment.LastUpdateDate = DateTime.UtcNow;

      if (existingComment.CategoryNumber != comment.CategoryNumber)
      {
        this.logger.LogInformation("changing category from {0} to {1} for commentId {2}",
          existingComment.CategoryNumber,
          comment.CategoryNumber,
          comment.CommentId);

        existingComment.CategoryNumber = comment.CategoryNumber;
      }

      return database.Comments.SaveComment(existingComment.ToDBModel()).ToDomainModel();
    }


    public DomainModel.Comment VoteUp(string activeUser, string commentId)
    {
      var comment = this.GetComment(commentId);
      var meeting = this.GetMeeting(comment.MeetingId);
      var team = this.GetTeam(meeting.TeamId);

      if (!IsTeamMember(activeUser, team))
      {
        logger.LogInformation("activeUser {0} is not a member of {1}", activeUser, team.TeamId);
        throw new Exception.AccessDenied();
      }

      if (comment.VotedUp == null)
      {
        comment.VotedUp = new List<string>();
      }

      //check if activeuser already voted up
      if (!comment.VotedUp.Contains(activeUser))
      {
        comment.VotedUp.Add(activeUser);
      }

      logger.LogDebug("votes count {0}", comment.VotedUp.Count);

      return database.Comments.SaveComment(comment.ToDBModel()).ToDomainModel();
    }

    public DomainModel.Comment VoteDown(string activeUserId, string commentId)
    {
      var comment = this.GetComment(commentId);
      var meeting = this.GetMeeting(comment.MeetingId);
      var team = this.GetTeam(meeting.TeamId);

      if (!IsTeamMember(activeUserId, team))
      {
        throw new Exception.AccessDenied();
      }

      if (comment.VotedUp.Contains(activeUserId))
      {
        comment.VotedUp.Remove(activeUserId);
      }

      return database.Comments.SaveComment(comment.ToDBModel()).ToDomainModel();
    }
  }
}