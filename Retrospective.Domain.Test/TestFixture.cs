using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using Retrospective.Data;
using DBModel = Retrospective.Data.Model;
using DomainModel =  Retrospective.Domain.Model;
using Xunit;

namespace Retrospective.Domain.Test
{
  public class TestFixture : IDisposable
  {
    public ObjectId TeamId { get; private set; }
    public ObjectId SessionId { get; private set; }
    public ObjectId DeleteNote { get; private set; }
    public ObjectId UpdateNote { get; private set; }
    public DBModel.User SampleUser;
    public DBModel.User OwnerUser;

    public Retrospective.Data.Database Database { get; private set; }

    public TestFixture()
    {
      string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
      string databaseName = "test_domainlayer";

      if (String.IsNullOrEmpty(connectionString))
      {
        connectionString = "mongodb://127.0.0.1:27017";
        Environment.SetEnvironmentVariable("DB_CONNECTIONSTRING", connectionString);
      }

      //start with an empty database
      var client = new MongoClient(connectionString);
      client.DropDatabase(databaseName);

      //connect to the database
      Retrospective.Data.Database database = new Database(databaseName);
      this.Database = database;
      InitializeTestRecords();




    }

    private void InitializeTestRecords()
    {

      System.Console.WriteLine("setup TestFixture");
      //initialize a user record
      this.SampleUser = this.Database.Users.Save(
          new Retrospective.Data.Model.User
          {
            Name = "Joe Smith",
            Email = "nobody@here.com",
            AuthenticationID ="123",
            AuthenticationSource="Google",
            LastLoggedIn=DateTime.UtcNow,
            IsDemoUser=false
          }
      );


      this.OwnerUser =  this.Database.Users.Save(
        new DBModel.User{
            Name = "Owner User",
            Email = "owner@here.com",
            AuthenticationID ="123",
            AuthenticationSource="Google",
            LastLoggedIn=DateTime.UtcNow,
            IsDemoUser=false
        }
      );

      //initialize a team record
      Retrospective.Data.Model.Team newTeam =
          new Retrospective.Data.Model.Team
          {
            Name = "test team A"
          };

      List<DBModel.TeamMember> members = new List<DBModel.TeamMember>();
      members.Add(new DBModel.TeamMember
      {
        UserId =  (ObjectId)this.OwnerUser.Id,
        StartDate = DateTime.Now,
        Role = DBModel.TeamRole.Owner
      });

      members.Add(new DBModel.TeamMember
      {
        UserId = (ObjectId) this.SampleUser.Id,
        StartDate = DateTime.Now,
        Role = DBModel.TeamRole.Member
      });

      newTeam.Members = members.ToArray();
      var savedTeam = this.Database.Teams.Save(newTeam);
      System.Console.WriteLine("saved: " + savedTeam.Id);


      this.TeamId = (ObjectId)newTeam.Id;

      //initialize a session record
      DBModel.Meeting newSession = this.Database.Meetings.Save(
          new DBModel.Meeting
          {
            Name = "test session",
            TeamId = (ObjectId)newTeam.Id,
            Categories = new DBModel.Category[] {
                            new DBModel.Category (1, "category 1"),
                                new DBModel.Category (2, "category 2"),
                                new DBModel.Category (3, "category 3")
                  }
          }
      );
      this.SessionId = (ObjectId)newSession.Id;

      //initialize some comment records
      this.Database.Comments.SaveComment(
          new Retrospective.Data.Model.Comment
          {
            MeetingId = (ObjectId)newSession.Id,
            Text = "comment 1 category 2",
            CategoryNumber = 2
          }
      );

      this.DeleteNote = (ObjectId)this.Database.Comments.SaveComment(
          new Retrospective.Data.Model.Comment
          {
            MeetingId = (ObjectId)newSession.Id,
            Text = "comment to delete in category 2",
            CategoryNumber = 2
          }
      ).Id;

      this.UpdateNote = (ObjectId)this.Database.Comments.SaveComment(
          new Retrospective.Data.Model.Comment
          {
            MeetingId = (ObjectId)newSession.Id,
            Text = "comment to update in category 2",
            CategoryNumber = 2
          }
      ).Id;

      

    }

    public void Dispose()
    {

    }

  }

  [CollectionDefinition("Domain Test collection")]
  public class TextCollection : ICollectionFixture<TestFixture>
  {

  }
}