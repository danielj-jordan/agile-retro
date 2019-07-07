using Retrospective.Data.Model;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Retrospective.Data
{
  public interface IDataUser
  {

    /// <summary>
    /// Save a single user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    User Save(User user);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    User Get(ObjectId id);


    User Get(string id);


    /// <summary>
    /// Returns a single user by the email address
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    List<User> FindUserByEmail(string email);


/* 

    /// <summary>
    /// Retrieve all the users on a team
    /// </summary>
    /// <param name="teamObjectId"></param>
    /// <returns></returns>
    List<User> GetTeamUsers(ObjectId teamObjectId);


    List<User> GetTeamUsers(string teamId);


*/

  }
}