using System;

namespace app.ModelExtensions
{
  public static class UserExtension
  {
    public static app.Model.User ToViewModel(
     this Retrospective.Domain.Model.User user)
    {
      var viewModelUser = new app.Model.User
      {
        UserId = user.UserId,
        Name=user.Name,
        Email=user.Email,
        IsDemoUser=user.IsDemoUser,
        AuthenticationID=user.AuthenticationID,
        AuthenticationSource=user.AuthenticationSource,
        LastLoggedIn=user.LastLoggedIn

      };
      return viewModelUser;
    }

    public static Retrospective.Domain.Model.User ToDomainModel(
        this app.Model.User user)
    {
      var domainUser = new Retrospective.Domain.Model.User
      {
        UserId = user.UserId,
        Name=user.Name,
        Email=user.Email,
        IsDemoUser=user.IsDemoUser,
        AuthenticationID=user.AuthenticationID,
        AuthenticationSource=user.AuthenticationSource,
        LastLoggedIn=user.LastLoggedIn
      };
      return domainUser;
    }
  }
}
