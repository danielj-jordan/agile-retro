using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using Retrospective.Data;
using Retrospective.Data.Model;
using Xunit;

namespace Retrospective.Domain.Test {
    public class TestFixture : IDisposable {
        public ObjectId TeamId { get; private set; }
        public ObjectId SessionId { get; private set; }
        public ObjectId DeleteNote { get; private set; }
        public ObjectId UpdateNote { get; private set; }
        public string SampleUser = "nobody@here.com";
        public string Owner = "owner@here.com";
        public Retrospective.Data.Database Database { get; private set; }

        public TestFixture () {
            string connectionString = "mongodb://localhost:27017";
            string databaseName = "test_domainlayer";

            //start with an empty database
            var client = new MongoClient (connectionString);
            client.DropDatabase (databaseName);

            //connect to the database
            Retrospective.Data.Database database = new Database (databaseName);
            this.Database = database;
            InitializeTestRecords ();
        }

        private void InitializeTestRecords () {

            System.Console.WriteLine("setup TestFixture") ;
            //initialize a user record
            Retrospective.Data.Model.User newUser = this.Database.Users.SaveUser (
                new Retrospective.Data.Model.User {
                    Name = "Joe Smoth",
                        Email = SampleUser
                }
            );

            //initialize a team record
            Retrospective.Data.Model.Team newTeam =
                new Retrospective.Data.Model.Team {
                    Name = "test team A",
                        Owner = this.Owner
                };

            List<TeamMember> members = new List<TeamMember>();
            members.Add(new TeamMember(){
                UserName=this.Owner,
                InviteDate=DateTime.Now
            });

            members.Add(new TeamMember(){
                UserName=this.SampleUser,
                InviteDate=DateTime.Now
            });

            newTeam.Members=members.ToArray();
            var savedTeam = this.Database.Teams.Save(newTeam);
            System.Console.WriteLine("saved: " + savedTeam.Id);
            

            this.TeamId = (ObjectId) newTeam.Id;

            //initialize a session record
            Retrospective.Data.Model.Meeting newSession = this.Database.Meetings.Save (
                new Meeting {
                    Name = "test session",
                        TeamId = (ObjectId) newTeam.Id,
                        Categories = new Retrospective.Data.Model.Category[] {
                            new Category (1, "category 1"),
                                new Category (2, "category 2"),
                                new Category (3, "category 3")
                        }
                }
            );
            this.SessionId = (ObjectId) newSession.Id;

            //initialize some comment records
            this.Database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment 1 category 2",
                        CategoryNumber = 2
                }
            );

            this.DeleteNote = (ObjectId) this.Database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment to delete in category 2",
                        CategoryNumber = 2
                }
            ).Id;

            this.UpdateNote = (ObjectId) this.Database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment to update in category 2",
                        CategoryNumber = 2
                }
            ).Id;

            // update the saved user with the team
            newUser.Teams = new ObjectId[] {
                (ObjectId) newTeam.Id };
            this.Database.Users.SaveUser (newUser);

        }

        public void Dispose () {

        }

    }

    [CollectionDefinition ("Domain Test collection")]
    public class TextCollection : ICollectionFixture<TestFixture> {

    }
}