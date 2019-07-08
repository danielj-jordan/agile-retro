using System.Collections.Generic;
using MongoDB.Bson;
using Retrospective.Data.Model;

namespace Retrospective.Data
{
  public interface IDataComment
  {
    Comment GetComment(ObjectId commentId);

    Comment GetComment(string commentId);
    Comment SaveComment(Comment comment);

    List<Comment> GetComments(string meetingId);

    List<Comment> GetComments(ObjectId meetingId);

    void Delete(ObjectId commentId);

  }
}