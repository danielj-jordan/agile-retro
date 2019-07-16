using System;


namespace app.ModelExtensions
{
  public static class CommentExtension
  {
    public static app.Model.Comment ToViewModelComment(
         this Retrospective.Domain.Model.Comment comment,
         string activeUser)
    {
      var viewModelComment = new app.Model.Comment
      {
        CommentId = comment.CommentId,
        MeetingId = comment.MeetingId,
        CategoryNum = comment.CategoryNumber,
        Text = comment.Text,
        UpdateUserId = comment.LastUpdateUserId,
        VoteCount = comment.VotedUp!=null?comment.VotedUp.Count:0,
        ThisUserVoted = comment.VotedUp!=null? comment.VotedUp.Contains(activeUser):false
      };
      return viewModelComment;
    }

    public static Retrospective.Domain.Model.Comment ToDomainComment(
        this app.Model.Comment comment)
    {
      var domainComment = new Retrospective.Domain.Model.Comment
      {
        CommentId = comment.CommentId,
        MeetingId = comment.MeetingId,
        CategoryNumber = comment.CategoryNum,
        Text = comment.Text,
        LastUpdateUserId = comment.UpdateUserId,
        LastUpdateDate = DateTime.UtcNow,
        VotedUp = new System.Collections.Generic.List<string>()
      };

        return domainComment;
    }
  }
}