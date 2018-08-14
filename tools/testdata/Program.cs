using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using Retrospective.Data;
using Retrospective.Data.Model;

namespace testdata {

    /*
    create some test data for end to end tests
     */
    class Program {
        static void Main (string[] args) {
            string connectionString = "mongodb://localhost:27017";
            string databaseName = "e2e_test";

            //start with an empty database
            var client = new MongoClient (connectionString);
            client.DropDatabase (databaseName);

            //connect to the database
            Retrospective.Data.Database database = new Database (databaseName);
            InitializeTestRecords (database);
        }

        private static void InitializeTestRecords (Retrospective.Data.Database database) {

            //initialize a user record
            Retrospective.Data.Model.User newUser = database.Users.SaveUser (
                new Retrospective.Data.Model.User {
                    Name = "Joe Smith",
                        Email = "nobody@127.0.0.1"
                }
            );

            //initialize a team record
            Retrospective.Data.Model.Team newTeam = database.Teams.SaveTeam (
                new Retrospective.Data.Model.Team {
                    Name = "test team",
                        Owner = "nobody@127.0.0.1",
                        TeamMembers = new String[] { "nobody@127.0.0.1" }
                });

            //initialize a session record
            Retrospective.Data.Model.RetrospectiveSession newSession = database.Sessions.SaveRetrospectiveSession (
                new RetrospectiveSession {
                    Name = "test session",
                        TeamId = (ObjectId) newTeam.Id,
                        Categories = new Retrospective.Data.Model.Category[] {
                            new Category (1, "category 1"),
                                new Category (2, "category 2"),
                                new Category (3, "category 3")
                        }
                }
            );

            //initialize some comment records
            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment 1 category 2",
                        CategoryNumber = 2
                }
            );

            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment to delete in category 2",
                        CategoryNumber = 2
                });

            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment to update in category 2",
                        CategoryNumber = 2
                });

            // update the saved user with the team
            newUser.Teams = new ObjectId[] {
                (ObjectId) newTeam.Id };
            database.Users.SaveUser (newUser);

        }
    }
}