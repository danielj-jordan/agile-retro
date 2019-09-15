using System;
using System.Collections.Generic;
using DBModel = Retrospective.Data.Model;
using DomainModel = Retrospective.Domain.Model;
using Retrospective.Data;
using Retrospective.Domain.ModelExtensions;

namespace Retrospective.Domain
{
  public interface ICommentManager : IBaseManager
  {
    List<DomainModel.Comment> GetComments(string activeUser, string meetingId);
    List<DomainModel.Category> GetCategories(string activeUser, string meetingId);

    void DeleteComment(string activeUser, string commentId);
    DomainModel.Comment SaveComment(string activeUser, DomainModel.Comment comment);

    DomainModel.Comment UpdateCommentText(
        string activeUserId,
        DomainModel.Comment comment
        );

    DomainModel.Comment VoteUp(string activeUser, string commentId);

    DomainModel.Comment VoteDown(string activeUserId, string commentId);

  }
}