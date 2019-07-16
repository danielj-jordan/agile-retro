using System;
using System.Linq;
using MongoDB.Bson;
using DomainModel = Retrospective.Domain.Model;
using DBModel = Retrospective.Data.Model;

namespace Retrospective.Domain.ModelExtensions
{
  public static class CommentExtensions
  {
    public static DBModel.Comment ToDBModel(
    this Retrospective.Domain.Model.Comment comment)
    {
      DBModel.Comment dbComment = new DBModel.Comment
      {
        Id = !string.IsNullOrWhiteSpace(comment.CommentId)? ObjectId.Parse(comment.CommentId): (ObjectId?)null,
        MeetingId = ObjectId.Parse(comment.MeetingId),
        CategoryNumber = comment.CategoryNumber,
        Text = comment.Text,
        LastUpdateUserId = ObjectId.Parse(comment.LastUpdateUserId) ,
        LastUpdateDate = comment.LastUpdateDate,
        VotedUp = comment.VotedUp?.Select(v => ObjectId.Parse(v)).ToArray()
      };

      return dbComment;
    }


    public static DomainModel.Comment ToDomainModel(
     this DBModel.Comment comment)
    {
      DomainModel.Comment domainComment = new DomainModel.Comment
      {
        CommentId = comment.Id.ToString(),
        MeetingId = comment.MeetingId.ToString(),
        CategoryNumber = comment.CategoryNumber,
        Text = comment.Text,
        LastUpdateUserId = comment.LastUpdateUserId.ToString(),
        LastUpdateDate = comment.LastUpdateDate,
        VotedUp = comment.VotedUp?.Select(v => v.ToString()).ToList<string>()
      };

      return domainComment;
    }
  }
}