using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using Retrospective.Data;
using Retrospective.Data.Model;
using Xunit;

namespace apptest
{
  public class TestFixture : IDisposable
  {

    public ObjectId TeamId { get; private set; }

    public Retrospective.Data.Model.Team TestTeam { get; set; }

    public ObjectId MeetingId { get; private set; }

    public ObjectId DeleteNote { get; private set; }

    public ObjectId UpdateNote { get; private set; }

    public User SampleUser { get; private set; }

    public User Owner { get; private set; }

    public Retrospective.Data.Database Database { get; private set; }

    public TestFixture()
    {


      string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");

      if (string.IsNullOrEmpty(connectionString))
      {
        connectionString = "mongodb://localhost:27017";
      }
      string databaseName = "test_controller";

      //start with an empty database
      var client = new MongoClient(connectionString);
      client.DropDatabase(databaseName);

      //connect to the database
      Retrospective.Data.Database database = new Database(connectionString, databaseName);
      this.Database = database;
      InitializeTestRecords();
    }

    private void InitializeTestRecords()
    {


      //initialize a user record
      this.SampleUser = this.Database.Users.Save(
           new Retrospective.Data.Model.User
           {
             Name = "Joe Smoth",
             Email = "nobody@here.com"
           }
       );

      this.Owner = this.Database.Users.Save(
          new Retrospective.Data.Model.User
          {
            Name = "Some Owner",
            Email = "owner@here.com"
          }
      );



      //initialize a team record
      Retrospective.Data.Model.Team newTeam = this.Database.Teams.Save(
          new Retrospective.Data.Model.Team
          {
            Name = "main test team",
            Members = new TeamMember[] {
                            new TeamMember(){
                                UserId= (ObjectId) this.SampleUser.Id,
                                StartDate=DateTime.Now,
                                Role = TeamRole.Member
                            },
                            new TeamMember(){
                                UserId=(ObjectId)this.Owner.Id,
                                StartDate=DateTime.Now,
                                Role = TeamRole.Owner
                            }
                  }
          });
      this.TeamId = (ObjectId)newTeam.Id;
      this.TestTeam = newTeam;


      //initialize a session record
      Retrospective.Data.Model.Meeting newMeeting = this.Database.Meetings.Save(
          new Meeting
          {
            Name = "test session",
            TeamId = (ObjectId)newTeam.Id,
            Categories = new Retrospective.Data.Model.Category[] {
                            new Category (1, "category 1"),
                                new Category (2, "category 2"),
                                new Category (3, "category 3")
                  }
          }
      );
      this.MeetingId = (ObjectId)newMeeting.Id;

      //initialize some comment records
      this.Database.Comments.SaveComment(
          new Retrospective.Data.Model.Comment
          {
            MeetingId = (ObjectId)newMeeting.Id,
            Text = "comment 1 category 2",
            CategoryNumber = 2
          }
      );

      this.DeleteNote = (ObjectId)this.Database.Comments.SaveComment(
          new Retrospective.Data.Model.Comment
          {
            MeetingId = (ObjectId)newMeeting.Id,
            Text = "comment to delete in category 2",
            CategoryNumber = 2
          }
      ).Id;

      this.UpdateNote = (ObjectId)this.Database.Comments.SaveComment(
          new Retrospective.Data.Model.Comment
          {
            MeetingId = (ObjectId)newMeeting.Id,
            Text = "comment to update in category 2",
            CategoryNumber = 2
          }
      ).Id;

    }

    public void Dispose()
    {

    }

  }

  [CollectionDefinition("Controller Test collection")]
  public class TextCollection : ICollectionFixture<TestFixture>
  {

  }
}