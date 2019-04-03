using System.Collections.Generic;
using MongoDB.Bson;
using Retrospective.Data.Model;

namespace Retrospective.Data
{
  public interface IDataComment
  {
    Comment GetComment(ObjectId commentId);

    Comment SaveComment(Comment comment);

    List<Comment> GetComments(string retrospectiveId);

    void Delete(ObjectId commentId);

  }
}