using System;
using System.Collections.Generic;
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
            
            InitializeDemoRecords(database);
        }

        public static void InitializeDemoRecords (Retrospective.Data.Database database) {

            //initialize the Demo user record
            Retrospective.Data.Model.User newUser = database.Users.SaveUser (
                new Retrospective.Data.Model.User {
                    Name = "Demo User",
                    Email = "demo@localhost",
                    IsDemoUser = true,
                    SubscriptionEnd = null,
                }
            );

            //initialize a team record
            Retrospective.Data.Model.Team newTeam = database.Teams.Save (
                new Retrospective.Data.Model.Team {
                    Name = "Demo Team",
                        Owner = "demo@localhost",
                        Members = new TeamMember[] {
                            new TeamMember {
                                UserName = "demo@localhost",
                                Role = "manager",
                                InviteDate = DateTime.Now,
                                StartDate = DateTime.Now
                            }
                        }
                });

            // update the saved user with the team
            newUser.Teams = new ObjectId[] {
                (ObjectId) newTeam.Id };
            database.Users.SaveUser (newUser);

                Console.WriteLine("initialized demo records");
        }

        private static void CreateSession (Retrospective.Data.Database database,
            ObjectId teamId, int categoryCount) {

            var categories = new List<Retrospective.Data.Model.Category> ();

            for (int i = 1; i <= categoryCount; i++) {
                categories.Add (new Retrospective.Data.Model.Category (i, $"category {i}"));
            }

            //  categories.

            /* 

             {
                            new Category (1, "category 1"),
                                new Category (2, "category 2"),
                                new Category (3, "category 3")
                        }
*/

            string meetingName = $"some meeting with {categoryCount} categories";

            //initialize a session record
            Retrospective.Data.Model.Meeting newSession = database.Meetings.Save (
                new Meeting {
                    Name = $"{meetingName} category meeting",
                        TeamId = teamId,
                        Categories = categories.ToArray ()
                }
            );

            //initialize some comment records
            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment 1 category " + categoryCount,
                        CategoryNumber = categoryCount
                }
            );

            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment to delete in category" + categoryCount,
                        CategoryNumber = categoryCount
                });

            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    RetrospectiveId = (ObjectId) newSession.Id,
                        Text = "comment to update in category " + categoryCount,
                        CategoryNumber = categoryCount
                });
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
            Retrospective.Data.Model.Team newTeam = database.Teams.Save (
                new Retrospective.Data.Model.Team {
                    Name = "test team",
                        Owner = "nobody@127.0.0.1",
                        Members = new TeamMember[] {
                            new TeamMember {
                                UserName = "nobody@127.0.0.1",
                                    InviteDate = DateTime.Now
                            }
                        }
                });

            // update the saved user with the team
            newUser.Teams = new ObjectId[] {
                (ObjectId) newTeam.Id };
            database.Users.SaveUser (newUser);

            CreateSession (database, (ObjectId) newTeam.Id, 1);

            CreateSession (database, (ObjectId) newTeam.Id, 2);

            CreateSession (database, (ObjectId) newTeam.Id, 3);

            CreateSession (database, (ObjectId) newTeam.Id, 4);

            CreateSession (database, (ObjectId) newTeam.Id, 5);

            CreateSession (database, (ObjectId) newTeam.Id, 6);

        }
    }
}