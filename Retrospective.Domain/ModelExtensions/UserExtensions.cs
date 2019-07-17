using System;
using System.Linq;
using MongoDB.Bson;
using DomainModel = Retrospective.Domain.Model;
using DBModel = Retrospective.Data.Model;

namespace Retrospective.Domain.ModelExtensions
{
  public static class UserExtensions
  {
    public static DBModel.User ToDBModel(
        this Retrospective.Domain.Model.User user)
    {
      DBModel.User dbUser = new DBModel.User
      {
        Id =  !string.IsNullOrWhiteSpace(user.UserId)? ObjectId.Parse(user.UserId): (ObjectId?)null,
        Name = user.Name,
        Email = user.Email,
        IsDemoUser = user.IsDemoUser,
        LastLoggedIn = user.LastLoggedIn,
        AuthenticationSource = user.AuthenticationSource,
        AuthenticationID = user.AuthenticationID
      };

      return dbUser;
    }


    public static DomainModel.User ToDomainModel(
     this DBModel.User user)
    {
      DomainModel.User domainUser = new DomainModel.User
      {
        UserId = user.Id.ToString(),
        Name = user.Name,
        Email = user.Email,
        IsDemoUser = user.IsDemoUser,
        LastLoggedIn = user.LastLoggedIn,
        AuthenticationSource = user.AuthenticationSource,
        AuthenticationID = user.AuthenticationID
      };

      return domainUser;
    }
  }
}