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

        private static User demoUser;

        static void Main (string[] args) {

            var connectionString =   Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
            var databaseName = Environment.GetEnvironmentVariable("DB_NAME");

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
            demoUser = database.Users.Save (
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
                        Members = new TeamMember[] {
                            new TeamMember {
                                UserId= (ObjectId) demoUser.Id,
                                Role =  TeamRole.Owner,
                                StartDate = DateTime.Now
                            }
                        }
                });   

                Console.WriteLine("initialized demo records");
        }

        private static void CreateMeeting (Retrospective.Data.Database database,
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
            Retrospective.Data.Model.Meeting newMeetingId = database.Meetings.Save (
                new Meeting {
                    Name = $"{meetingName} category meeting",
                        TeamId = teamId,
                        Categories = categories.ToArray ()
                }
            );

            //initialize some comment records
            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    MeetingId = (ObjectId) newMeetingId.Id,
                        Text = "comment 1 category " + categoryCount,
                        CategoryNumber = categoryCount
                }
            );

            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    MeetingId = (ObjectId) newMeetingId.Id,
                        Text = "comment to delete in category" + categoryCount,
                        CategoryNumber = categoryCount
                });

            database.Comments.SaveComment (
                new Retrospective.Data.Model.Comment {
                    MeetingId = (ObjectId) newMeetingId.Id,
                        Text = "comment to update in category " + categoryCount,
                        CategoryNumber = categoryCount
                });
        }

        private static void InitializeTestRecords (Retrospective.Data.Database database) {

            //initialize a user record
            Retrospective.Data.Model.User newUser = database.Users.Save (
                new Retrospective.Data.Model.User {
                    Name = "Joe Smith",
                        Email = "nobody@127.0.0.1"
                }
            );

            //initialize a team record
            Retrospective.Data.Model.Team newTeam = database.Teams.Save (
                new Retrospective.Data.Model.Team {
                    Name = "test team",
                        Members = new TeamMember[] {
                            new TeamMember {
                                UserId=  (ObjectId) newUser.Id,
                                Role= TeamRole.Owner
                            }
                        }
                });

            CreateMeeting (database, (ObjectId) newTeam.Id, 1);

            CreateMeeting (database, (ObjectId) newTeam.Id, 2);

            CreateMeeting (database, (ObjectId) newTeam.Id, 3);

            CreateMeeting (database, (ObjectId) newTeam.Id, 4);

            CreateMeeting (database, (ObjectId) newTeam.Id, 5);

            CreateMeeting (database, (ObjectId) newTeam.Id, 6);

        }
    }
}