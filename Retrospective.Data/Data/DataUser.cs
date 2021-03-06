using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Retrospective.Data
{


  public class DataUser : IDataUser
  {
    private string collection = "user";
    private IDatabase database;


    public DataUser(IDatabase database)
    {
      this.database = database;

    }



    /// <summary>
    /// Save a single user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public User Save(User user)
    {
      if (user.Id is null)
      {
        database.MongoDatabase.GetCollection<User>(collection).InsertOne(user);

        return user;
      }

      var filter = MongoDB.Driver.Builders<User>.Filter.Eq("Id", user.Id);
      var saved = database.MongoDatabase.GetCollection<User>(collection).ReplaceOne(filter, user);
      System.Console.WriteLine("write {0} user records {1} for user id; {2}", saved.ModifiedCount, saved.UpsertedId, user.Id);
      System.Console.WriteLine(saved.ToJson());

      return user;
    }


    /// <summary>
    /// Returns a single user by the email address
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public List<User> FindUserByEmail(string email)
    {
      var filter = MongoDB.Driver.Builders<User>.Filter.Eq("Email", email);
      var found = database.MongoDatabase.GetCollection<User>(collection).Find(filter).ToList<User>();
      return found;
    }

    public User Get(ObjectId id)
    {
      var filter = MongoDB.Driver.Builders<User>.Filter.Eq("Id", id);
      var found = database.MongoDatabase.GetCollection<User>(collection).Find(filter).FirstOrDefault();
      return found;
    }

    public User Get(string id)
    {
      return this.Get(new ObjectId(id));
    }


/* 
    /// <summary>
    /// Retrieve all the users on a team
    /// </summary>
    /// <param name="teamObjectId"></param>
    /// <returns></returns>
    public List<User> GetTeamUsers(ObjectId teamObjectId)
    {

      var filter = MongoDB.Driver.Builders<User>.Filter.Eq("Teams", teamObjectId);
      var found = database.MongoDatabase.GetCollection<User>(collection).Find(filter).ToList<User>();
      return found;


    }

    /// <summary>
    /// returns all users on a team
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public List<User> GetTeamUsers(string teamId)
    {
      return this.GetTeamUsers(new ObjectId(teamId));
    }
    */

  }
}
